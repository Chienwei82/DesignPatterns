/**
 * build-audio.mjs — Monta `audio/final-audio.mp3` respetando la narración
 * completa de cada escena (nada de recortes) y los mismos tiempos que usa el
 * vídeo.
 *
 *   node audio/build-audio.mjs        # o: npm run audio:compose
 *
 * Lee `audio/timing.json` (lo genera `npm run timing`) y `audio/narracion.json`.
 * Entre escena y escena inserta `pad` segundos de silencio y rellena el final
 * hasta `total`, de forma que el audio dura exactamente lo que el vídeo.
 */

import { spawnSync } from "node:child_process";
import { readFileSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const AQUI = path.dirname(fileURLToPath(import.meta.url));
const PROYECTO = path.resolve(AQUI, "..");

const timing = JSON.parse(readFileSync(path.join(AQUI, "timing.json"), "utf8"));
const narracion = JSON.parse(readFileSync(path.join(AQUI, "narracion.json"), "utf8"));

const PAD = Number(timing.pad ?? 1.2);
const TOTAL = Number(timing.total);
const SALIDA = path.join(AQUI, "final-audio.mp3");

if (!(TOTAL > 0)) {
  console.error("❌ audio/timing.json no tiene una duración válida. Ejecuta `npm run timing`.");
  process.exit(1);
}

const escenas = timing.escenas.map((escena, i) => {
  const fuente = narracion.escenas[i];
  if (!fuente || fuente.id !== escena.id) {
    console.error(
      `❌ audio/timing.json y audio/narracion.json no coinciden en la escena ${i + 1}. ` +
      "Ejecuta `npm run timing`.",
    );
    process.exit(1);
  }
  return {
    archivo: path.join(AQUI, fuente.audio),
    inicio: escena.inicio,
    narracion: escena.narracion,
  };
});

console.log("\n🎵 Montando la narración completa (sin recortes):\n");
for (const escena of escenas) {
  console.log(
    `   ${path.basename(escena.archivo).padEnd(26)} ${String(escena.inicio).padStart(7)}s` +
    ` → ${String(Math.round((escena.inicio + escena.narracion) * 1000) / 1000).padStart(7)}s  (${escena.narracion}s)`,
  );
}

const entradas = escenas.flatMap((escena) => ["-i", escena.archivo]);
const relleno = escenas.map((_, i) => `[${i}:a]apad=pad_dur=${PAD}[a${i}]`).join(";");
const etiquetas = escenas.map((_, i) => `[a${i}]`).join("");
const grafo =
  `${relleno};${etiquetas}concat=n=${escenas.length}:v=0:a=1[mezcla];[mezcla]apad[out]`;

const ffmpeg = spawnSync("ffmpeg", [
  "-hide_banner", "-loglevel", "error", "-y",
  ...entradas,
  "-filter_complex", grafo,
  "-map", "[out]",
  "-t", String(TOTAL),
  "-c:a", "libmp3lame", "-b:a", "192k", "-ar", "44100", "-ac", "2",
  SALIDA,
], { encoding: "utf8" });

if (ffmpeg.status !== 0) {
  console.error("❌ ffmpeg falló al montar el audio:\n", ffmpeg.stderr);
  process.exit(1);
}

const medida = spawnSync("ffprobe", [
  "-v", "error", "-show_entries", "format=duration",
  "-of", "default=noprint_wrappers=1:nokey=1", SALIDA,
], { encoding: "utf8" });

const duracion = Number(String(medida.stdout).trim());
const desvio = Math.abs(duracion - TOTAL);
console.log(`\n✅ ${path.relative(PROYECTO, SALIDA)} · ${duracion.toFixed(3)}s (vídeo: ${TOTAL}s)`);
if (desvio > 0.1) {
  console.warn(`⚠️  Desvío de ${desvio.toFixed(3)}s respecto al vídeo.`);
}
