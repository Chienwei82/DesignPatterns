/**
 * build-timeline.mjs — Sincroniza el vídeo con la narración real.
 *
 *   npm run timing
 *
 * Qué hace:
 *   1. Mide cada MP3 de `audio/` (duración + pausas con ffmpeg silencedetect).
 *   2. Reparte la narración en el tiempo y calcula, para cada elemento marcado
 *      con `data-mencion="..."` en index.html, el instante en que se nombra.
 *   3. Reescribe en index.html la duración de #root, el `data-start` /
 *      `data-duration` de cada escena y el bloque `GUION` (entre los marcadores
 *      `@timing:start` y `@timing:end`).
 *   4. Guarda `audio/timing.json` para que el montaje de audio use los mismos
 *      tiempos (audio/build-audio.mjs).
 *
 * Reglas de montaje (ajustables por variables de entorno):
 *   HF_PAD=1.2      silencio entre el final de una narración y la siguiente
 *   HF_FUNDIDO=0.8  duración del crossfade entre escenas
 *   HF_COLA=1.6     cola tras la última palabra (fundido a negro)
 *   HF_HOLD=6       resalte máximo de un elemento
 */

import { execFileSync, spawnSync } from "node:child_process";
import { readFileSync, writeFileSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const AQUI = path.dirname(fileURLToPath(import.meta.url));
const PROYECTO = path.resolve(AQUI, "..");
const INDEX = path.join(PROYECTO, "index.html");

const PAD = Number(process.env.HF_PAD ?? 1.2);
const FUNDIDO = Number(process.env.HF_FUNDIDO ?? 0.8);
const COLA = Number(process.env.HF_COLA ?? 1.6);
const HOLD = Number(process.env.HF_HOLD ?? 6);
const MIN_HOLD = 1.1;
const TOLERANCIA_ANCLA = 1.2;

const r3 = (n) => Math.round(n * 1000) / 1000;

/* ── Medición del audio ─────────────────────────────────────────────────── */

function duracion(archivo) {
  const salida = execFileSync("ffprobe", [
    "-v", "error",
    "-show_entries", "format=duration",
    "-of", "default=noprint_wrappers=1:nokey=1",
    archivo,
  ], { encoding: "utf8" });
  return Number(salida.trim());
}

/** Pares {inicio, fin} de silencio detectados por ffmpeg en el MP3. */
function medirSilencios(archivo, total) {
  const { stderr } = spawnSync("ffmpeg", [
    "-hide_banner", "-nostats", "-i", archivo,
    "-af", "silencedetect=noise=-35dB:d=0.25",
    "-f", "null", "-",
  ], { encoding: "utf8" });

  const pausas = [];
  let abierto = null;
  for (const linea of String(stderr ?? "").split("\n")) {
    const ini = linea.match(/silence_start: (-?[\d.]+)/);
    const fin = linea.match(/silence_end: ([\d.]+)/);
    if (ini) abierto = Number(ini[1]);
    else if (fin && abierto !== null) {
      pausas.push({ inicio: Math.max(0, abierto), fin: Number(fin[1]) });
      abierto = null;
    }
  }
  if (abierto !== null) pausas.push({ inicio: Math.max(0, abierto), fin: total });
  return pausas;
}

/* ── Alineado narración ↔ tiempo ────────────────────────────────────────── */

const normaliza = (texto) =>
  texto.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();

/**
 * Devuelve una función índiceDeCarácter → segundos. Usa las pausas detectadas
 * en el audio como anclas y, entre anclas, interpola proporcionalmente al
 * texto: así el resalte cae dentro de la frase que nombra cada patrón.
 */
function relojDeNarracion(texto, inicio, fin, pausas) {
  const norm = normaliza(texto);
  const largo = norm.length;

  const cortes = [];
  const re = /[,;:.!?]\s+/g;
  let encontrado;
  while ((encontrado = re.exec(norm)) !== null) cortes.push(encontrado.index + encontrado[0].length);

  const anclas = [{ idx: 0, t: inicio }];
  let ultimo = 0;

  for (const pausa of pausas) {
    const medio = (pausa.inicio + pausa.fin) / 2;
    let elegido = null;
    let mejor = Infinity;
    for (const corte of cortes) {
      if (corte <= ultimo) continue;
      const estimado = inicio + (corte / largo) * (fin - inicio);
      const diferencia = Math.abs(estimado - medio);
      if (diferencia < mejor) {
        mejor = diferencia;
        elegido = corte;
      }
    }
    if (elegido !== null && mejor <= TOLERANCIA_ANCLA) {
      anclas.push({ idx: elegido, t: medio });
      ultimo = elegido;
    }
  }
  anclas.push({ idx: largo, t: fin });

  return (idx) => {
    for (let i = 1; i < anclas.length; i += 1) {
      const a = anclas[i - 1];
      const b = anclas[i];
      if (idx <= b.idx) {
        const span = Math.max(1, b.idx - a.idx);
        return a.t + ((idx - a.idx) / span) * (b.t - a.t);
      }
    }
    return fin;
  };
}

/* ── Lectura del HTML ───────────────────────────────────────────────────── */

/** Elementos anotados con data-mencion, agrupados por escena. */
function mencionesDe(html) {
  const porEscena = {};
  const secciones = /<section[^>]*id="(escena-[a-z]+)"[\s\S]*?<\/section>/g;
  let seccion;
  while ((seccion = secciones.exec(html)) !== null) {
    const lista = [];
    const etiquetas = /<(?:div|span|strong)\b[^>]*data-mencion="([^"]+)"[^>]*data-mencion-tipo="([^"]+)"[^>]*>/g;
    let etiqueta;
    while ((etiqueta = etiquetas.exec(seccion[0])) !== null) {
      lista.push({ texto: etiqueta[1], tipo: etiqueta[2] });
    }
    porEscena[seccion[1]] = lista;
  }
  return porEscena;
}

/* ── Cálculo del guion ──────────────────────────────────────────────────── */

const narracion = JSON.parse(readFileSync(path.join(AQUI, "narracion.json"), "utf8"));
const htmlOriginal = readFileSync(INDEX, "utf8");
const menciones = mencionesDe(htmlOriginal);

const escenas = [];
let cursor = 0;

narracion.escenas.forEach((escena) => {
  const archivo = path.join(AQUI, escena.audio);
  const total = duracion(archivo);
  const pausas = medirSilencios(archivo, total);

  // Ventana real de voz: descuenta silencios de entrada y de salida.
  let hablaInicio = 0;
  let hablaFin = total;
  if (pausas.length && pausas[0].inicio < 0.05) hablaInicio = pausas[0].fin;
  const ultima = pausas[pausas.length - 1];
  if (ultima && ultima.fin > total - 0.05) hablaFin = ultima.inicio;

  const inicio = r3(cursor);
  const reloj = relojDeNarracion(escena.texto, hablaInicio, hablaFin, pausas);
  const norm = normaliza(escena.texto);

  const propias = (menciones[escena.id] ?? []).map((mencion) => {
    const idx = norm.indexOf(normaliza(mencion.texto));
    if (idx < 0) {
      throw new Error(`No encuentro "${mencion.texto}" en la narración de ${escena.id}`);
    }
    return {
      sel: `#${escena.id} [data-mencion="${mencion.texto}"]`,
      tipo: mencion.tipo,
      inicio: r3(inicio + reloj(idx)),
      fin: null,
    };
  });

  propias.sort((a, b) => a.inicio - b.inicio);

  propias.forEach((mencion, j) => {
    if (mencion.tipo === "texto") return; // el marcador de texto se queda puesto
    const siguiente = propias[j + 1];
    const hasta = Math.min(inicio + hablaFin, mencion.inicio + HOLD);
    const libre = siguiente ? siguiente.inicio - 0.25 : hasta;
    mencion.fin = r3(Math.max(mencion.inicio + MIN_HOLD, Math.min(hasta, libre)));
  });

  escenas.push({
    id: escena.id,
    audio: escena.audio,
    inicio,
    narracion: r3(total),
    finDeVoz: r3(inicio + hablaFin),
    clipInicio: 0,
    clipFin: 0,
    clipDuracion: 0,
    menciones: propias,
  });

  cursor = inicio + total + PAD;
});

const total = r3(
  escenas[escenas.length - 1].inicio + escenas[escenas.length - 1].narracion + COLA,
);
const cortes = escenas.map((e) => (e.inicio === 0 ? 0 : r3(e.inicio - FUNDIDO)));
cortes.push(total);

escenas.forEach((escena, i) => {
  escena.clipInicio = cortes[i];
  // La escena saliente sigue viva durante su propio fundido de salida.
  escena.clipFin = i === escenas.length - 1 ? cortes[i + 1] : r3(cortes[i + 1] + FUNDIDO);
  escena.clipDuracion = r3(escena.clipFin - escena.clipInicio);
});

const guion = {
  total,
  fundido: FUNDIDO,
  escenas: escenas.map((e) => ({
    id: e.id,
    inicio: e.inicio,
    narracion: e.narracion,
    clipInicio: e.clipInicio,
    clipDuracion: e.clipDuracion,
    menciones: e.menciones,
  })),
};

/* ── Escritura ──────────────────────────────────────────────────────────── */

writeFileSync(
  path.join(AQUI, "timing.json"),
  `${JSON.stringify({ pad: PAD, cola: COLA, fundido: FUNDIDO, ...guion }, null, 2)}\n`,
  "utf8",
);

let html = htmlOriginal;
if (!/<div[^>]*id="root"[^>]*data-duration="[\d.]+"/.test(html)) {
  throw new Error("No encuentro data-duration en #root");
}
html = html.replace(/(<div[^>]*id="root"[^>]*data-duration=")[\d.]+(")/, `$1${total}$2`);

escenas.forEach((escena) => {
  const re = new RegExp(
    `(id="${escena.id}"[^>]*data-start=")[\\d.]+("[^>]*data-duration=")[\\d.]+(")`,
  );
  if (!re.test(html)) throw new Error(`No encuentro la sección ${escena.id} en index.html`);
  html = html.replace(re, `$1${escena.clipInicio}$2${escena.clipDuracion}$3`);
});

const cuerpo = JSON.stringify(guion, null, 2)
  .split("\n")
  .map((linea) => `        ${linea}`)
  .join("\n");

if (!/\/\* @timing:start \*\//.test(html) || !/\/\* @timing:end \*\//.test(html)) {
  throw new Error("Faltan los marcadores @timing:start / @timing:end en index.html");
}
html = html.replace(
  /(\/\* @timing:start \*\/)[\s\S]*?(\/\* @timing:end \*\/)/,
  `$1\n${cuerpo}\n        $2`,
);

writeFileSync(INDEX, html, "utf8");

/* ── Informe ────────────────────────────────────────────────────────────── */

console.log(`\n🎬 Guion sincronizado · ${total.toFixed(1)} s (${(total / 60).toFixed(2)} min)\n`);
console.log("  escena                   audio   clip               resaltes");
for (const escena of escenas) {
  const tarjetas = escena.menciones.filter((m) => m.tipo !== "texto").length;
  const marcas = escena.menciones.length - tarjetas;
  console.log(
    `  ${escena.id.padEnd(22)} ${escena.narracion.toFixed(1).padStart(5)}s  ` +
    `${String(escena.clipInicio).padStart(7)} → ${String(escena.clipFin).padStart(7)}  ` +
    `${tarjetas} tarjetas · ${marcas} marcas`,
  );
}
console.log(`\n  pad=${PAD}s · fundido=${FUNDIDO}s · cola=${COLA}s · hold=${HOLD}s`);
console.log("  escrito: audio/timing.json + index.html\n");
