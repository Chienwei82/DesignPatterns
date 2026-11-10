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

import { writeFile } from "fs/promises";

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

// Una entrada por escena: [archivo, texto]
// Los textos resumen el guion de presentación/guion.md.
const ESCENAS = [
  [
    "01-portada.mp3",
    "Los patrones de diseño son soluciones probadas y reutilizables a problemas comunes en el software orientado a objetos. Fueron catalogados por la Gang of Four en 1994 y se dividen en tres familias: creacionales, que resuelven cómo se crean los objetos; estructurales, que resuelven cómo se componen las clases; y de comportamiento, que resuelven cómo se comunican entre sí. En este repositorio encontrarás veinte de ellos implementados en C# y .NET 10, siempre en español.",
  ],
  [
    "02-creacionales.mp3",
    "Los patrones creacionales controlan cómo nacen los objetos. Singleton garantiza una única instancia global, ideal para loggers o configuración. Factory Method deja que las subclases decidan qué clase crear. Abstract Factory crea familias de objetos que deben combinarse entre sí. Builder separa la construcción compleja en pasos encadenables, evitando constructores gigantes. Y Prototype crea objetos copiando un prototipo existente en vez de partir desde cero.",
  ],
  [
    "03-estructurales.mp3",
    "Los patrones estructurales organizan cómo se componen las clases y objetos. Adapter hace compatible una interfaz ajena, como un adaptador de enchufe. Decorator añade responsabilidades nuevas sin modificar la clase original. Facade ofrece una interfaz simplificada sobre un subsistema complejo. Proxy controla el acceso a un objeto, por ejemplo con caché o seguridad. Composite trata objetos simples y compuestos de forma uniforme. Bridge desacopla una abstracción de su implementación. Y Flyweight comparte estado intrínseco para ahorrar memoria cuando hay muchos objetos similares.",
  ],
  [
    "04-comportamiento.mp3",
    "Los patrones de comportamiento gestionan la comunicación y las responsabilidades entre objetos. Strategy permite intercambiar algoritmos en tiempo de ejecución. Observer avisa a varios interesados cuando algo cambia, ideal para eventos de UI. Command encapsula una solicitud como objeto para deshacerla o encolarla. Template Method define el esqueleto de un algoritmo y deja que las subclases rellenen pasos. State hace que un objeto cambie de comportamiento según su estado interno. Mediator centraliza la comunicación mediante un intermediario. Memento captura y restaura estado sin romper el encapsulamiento. Y Chain of Responsibility pasa una solicitud por una cadena de manejadores hasta que alguien la resuelva.",
  ],
  [
    "05-cierre.mp3",
    "Este proyecto incluye veinte de los veintitrés patrones GoF. Faltan a propósito Interpreter, Iterator y Visitor: Iterator ya lo cubre C# con IEnumerable y LINQ, Visitor es muy especializado e Interpreter es raro en la práctica. Lo más poderoso es que los patrones se combinan: un Abstract Factory puede crear Strategies, coordinadas por un Mediator que notifica cambios vía Observer. Pruébalo tú mismo con dotnet run en el repositorio.",
  ],
];

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
  console.log("   Revisa los tiempos frente a guion.md y ajusta index.html si es necesario.");
}

generarAudio();