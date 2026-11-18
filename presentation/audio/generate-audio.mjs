/**
 * generate-audio.mjs — Genera la narración MP3 por escena con fish.audio (https://fish.audio).
 * Usa fetch nativo de Node (sin dependencias externas).
 *
 * Requisitos:
 *   - Variable de entorno FISH_API_KEY (consíguela en https://fish.audio).
 *   - Node 18+ (fetch nativo).
 *
 * Uso:
 *   export FISH_API_KEY=tu_clave
 *   node audio/generate-audio.mjs
 *   # o desde la carpeta presentation/: npm run audio
 */

import { readFile, writeFile } from "fs/promises";

const API_BASE = "https://api.fish.audio/v1/tts";
const MODEL = process.env.FISH_MODEL ?? "s2.1-pro-free";
const FORMAT = process.env.FISH_FORMAT ?? "mp3";
const OUT_DIR = new URL(".", import.meta.url).pathname;

const API_KEY = process.env.FISH_API_KEY;
if (!API_KEY) {
  console.error("❌ Falta la variable de entorno FISH_API_KEY.");
  console.error("   Consíguela en https://fish.audio (Get Started / API Keys) y ejecuta:");
  console.error("   export FISH_API_KEY=tu_clave");
  process.exit(1);
}

// Los textos viven en audio/narracion.json (fuente unica para audio y tiempos).
const NARRACION = JSON.parse(await readFile(new URL("./narracion.json", import.meta.url), "utf8"));
const ESCENAS = NARRACION.escenas.map((escena) => [escena.audio, escena.texto]);

async function generarAudio() {
  console.log(`🎙️  Generando audio con fish.audio (modelo: ${MODEL}, formato: ${FORMAT})...`);

  for (const [archivo, texto] of ESCENAS) {
    const salida = `${OUT_DIR}/${archivo}`;
    console.log(`▶ ${archivo}`);

    try {
      const respuesta = await fetch(API_BASE, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${API_KEY}`,
          "Content-Type": "application/json",
          model: MODEL,
        },
        body: JSON.stringify({ text: texto, format: FORMAT }),
      });

      if (!respuesta.ok) {
        const cuerpo = await respuesta.text().catch(() => "");
        throw new Error(`HTTP ${respuesta.status}: ${cuerpo}`);
      }

      const buffer = Buffer.from(await respuesta.arrayBuffer());
      await writeFile(salida, buffer);

      if (buffer.length > 0) {
        console.log(`   ✅ Generado: ${salida} (${buffer.length} bytes)`);
      } else {
        throw new Error("Respuesta vacía");
      }
    } catch (error) {
      console.error(`   ❌ Falló la generación de ${archivo}: ${error.message}`);
      process.exit(1);
    }
  }

  console.log(`🎧  Todo el audio generado en ${OUT_DIR}`);
  console.log("   Ejecuta `npm run timing` para recalcular el guion y los resaltes.");
}

generarAudio();