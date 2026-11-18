# Vídeo · Patrones de Diseño GoF en C#

Plantilla de **vídeo renderizable** con [HyperFrames](https://hyperframes.heygen.com):
1920×1080, **duración variable = la de la narración** (~2m52s con el audio actual), 5 escenas
con crossfades suaves. Explica los **20 patrones de diseño GoF** implementados en el
repositorio `DesignPatterns` (creacionales, estructurales, comportamiento).

La línea de tiempo **no se escribe a mano**: se genera a partir de los MP3 reales, de modo
que cada escena dura lo que dura su narración (nunca se corta una frase) y **cada patrón se
resalta en pantalla justo cuando se le nombra**.

## Comandos

| Comando | Qué hace |
|---------|----------|
| `npm run dev` | Studio en local: reproducir la línea de tiempo y editar a mano |
| `npm run check` | lint + runtime + layout + motion + contraste, en un solo mando |
| `npm run lint` | solo lint (`npx hyperframes lint --verbose` para info) |
| `npm run snapshot` | PNG de momentos clave (`-- --at 36.5,105` para fijar tiempos) |
| `npm run render` | recalcula guion, monta el audio completo y renderiza `renders/video.mp4` |
| `npm run timing` | mide los MP3 y reescribe escenas + resaltes en `index.html` y `audio/timing.json` |
| `npm run audio` | genera la narración MP3 por escena con fish.audio |
| `npm run audio:compose` | monta `audio/final-audio.mp3` con la narración completa |
| `npm run publish` | sube el proyecto y devuelve un enlace compartible |

Render avanzado:
```bash
npx hyperframes render --quality draft              # iterando (rápido, sin audio)
npx hyperframes render                              # calidad por defecto (looks, sin audio)
npx hyperframes render --quality delivery --fps 60  # entrega final (sin audio)
ffprobe -v error -show_format renders/video.mp4     # verificar duración/fps

# Para render con audio (usa ffmpeg para mezclar video + audio):
npm run render                                      # renderiza + mezcla audio automáticamente
```

Audio en el render:
- `npm run render` ejecuta `audio/render-with-audio.sh`, que:
  1. Recalcula el guion (`audio/build-timeline.mjs`) → `audio/timing.json` + tiempos en `index.html`.
  2. Monta `audio/final-audio.mp3` (`audio/build-audio.mjs`): cada MP3 **entero**, con
     `pad` (1,2 s) de silencio entre escenas y relleno final hasta la duración del vídeo.
  3. Renderiza el vídeo con hyperframes (`renders/video.mp4`, sin audio).
  4. Mezcla vídeo + audio con ffmpeg (`-c:v copy -c:a aac -af apad -shortest`), así el audio
     se rellena con silencio si hiciera falta y **nunca trunca una frase**.
- Requiere `ffmpeg` instalado y los MP3 de escena en `audio/`.

## Estructura del proyecto

```
presentation/
├── index.html              LA composición (fichero único): TEMA · ESCENAS · TIMELINE
├── guion.md                # guion del vídeo: escenas, tiempos, texto narrado
├── hyperframes.json        configuración (registry, rutas, authoringSkill)
├── meta.json               metadatos (id, nombre)
├── package.json            scripts (dev / check / render / timing / …)
├── BRIEF.md                decisiones de este vídeo y de la base reutilizable
├── README.md               este archivo
├── audio/
│   ├── README.md           # cómo obtener la API key y generar el audio
│   ├── narracion.json      # texto de cada escena (fuente única para audio y tiempos)
│   ├── timing.json         # GENERADO: duración/inicio de cada escena y cada resalte
│   ├── voices.json         # reference_id de voz por rol
│   ├── generate-audio.mjs  # llama a fish.audio y escribe los MP3
│   ├── build-timeline.mjs  # sincroniza index.html con los MP3 (npm run timing)
│   ├── build-audio.mjs     # monta final-audio.mp3 sin recortes (npm run audio:compose)
│   └── render-with-audio.sh# render + mezcla final
├── renders/                salida MP4 (gitignored)
└── snapshots/              PNGs de snapshot (gitignored)
```

## Cómo está montado (reglas que conviene no romper)
- **Raíz**: `#root` (`data-composition-id="root"`, `data-duration` = duración total) fija la duración del render. Todo lo demás son clips dentro de ella.
- **Escenas = clips que se solapan**: cada escena empieza `fundido` (0,8 s) antes de su narración
  y termina al acabar el crossfade siguiente; esos 0,8 s de solape SON la transición (se funde
  `opacity` sobre `.capa`). Los valores los escribe `npm run timing`, no se editan a mano.
- **Nunca se anima la visibilidad de un clip** (ni `display`/`visibility`/`autoAlpha`): el framework gestiona la visibilidad con `data-start`.
- **Una sola línea de tiempo**: `window.__timelines["root"]`, pausada. Cada escena se cuelga con `tlRaiz.add(tlEscena, escena.inicio)` y dentro de cada `tlEscena` los tiempos son LOCALES.
- **Las escenas llevan `data-composition-id`** (sub-composición inline): da identidad propia a cada escena.
- **Resaltes**: cada elemento que la narración menciona lleva `data-mencion="<término>"` y
  `data-mencion-tipo="tarjeta|bloque|texto"`. `npm run timing` busca ese término en el texto de
  la escena, lo sitúa en el tiempo (proporcional al texto, anclado a las pausas reales del MP3)
  y escribe el bloque `GUION` de `index.html`. Tipos: `tarjeta` (se eleva con anillo y vuelve),
  `bloque` (igual, para las tarjetas de familia de la portada) y `texto` (subrayado tipo
  marcador, se queda puesto).
- **Determinismo**: sin `Date.now()`/`performance.now()`, sin `Math.random()` sin semilla, sin red en render, sin `repeat: -1`. Sin `transition` CSS en elementos que anima GSAP (rompería el seek).
- **Velos** `#velo-inicio` / `#velo-final`: fundido desde negro y a negro al final (arranca 1,2 s antes del total; negro puro hasta el final).
- **Tipografías**: Caveat/Kalam/JetBrains Mono por Google Fonts en el `<head>`; la CLI las incrusta en el build y las cachea en `~/.cache/hyperframes/fonts`.

## Linting — SIEMPRE EJECUTAR DESPUÉS DE CAMBIOS
Después de crear o editar cualquier `.html` composición, **siempre** ejecutar el check completo antes de considerar la tarea completa:

```bash
npm run check
```

Corregir todos los errores antes de presentar el resultado. Las revisiones (warnings) deben revisarse antes de renderizar.

> `renders/` y `snapshots/` están en `.gitignore`: no se versionan binarios.