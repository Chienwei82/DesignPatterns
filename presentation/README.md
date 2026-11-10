# Vídeo · Patrones de Diseño GoF en C#

Plantilla de **vídeo renderizable** con [HyperFrames](https://hyperframes.heygen.com):
1920×1080, 60 s, 5 escenas con crossfades suaves. Explica los **20 patrones de diseño GoF**
implementados en el repositorio `DesignPatterns` (creacionales, estructurales, comportamiento).
Tema, ritmo y estructura están pensados para copiarse tal cual en el próximo vídeo.

## Comandos

| Comando | Qué hace |
|---------|----------|
| `npm run dev` | Studio en local: reproducir la línea de tiempo y editar a mano |
| `npm run check` | lint + runtime + layout + motion + contraste, en un solo mando |
| `npm run lint` | solo lint (`npx hyperframes lint --verbose` para info) |
| `npm run snapshot` | PNG de momentos clave + `snapshots/contact-sheet.jpg` |
| `npm run render` | renderiza video con audio a `renders/video.mp4` (usa ffmpeg para mezclar) |
| `npm run publish` | sube el proyecto y devuelve un enlace compartible |
| `npm run audio` | genera la narración MP3 por escena con fish.audio |

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
  1. Renderiza el video con hyperframes (`renders/video.mp4`, sin audio)
  2. Recorta cada MP3 de escena a 12 s y los concatena en `audio/final-audio.mp3` (60 s)
  3. Mezcla video + audio con ffmpeg (`-c:v copy -c:a aac -shortest`)
- Requiere `ffmpeg` instalado y `audio/final-audio.mp3` generado (ejecuta `npm run audio` primero).

## Estructura del proyecto

```
presentation/
├── index.html            LA composición (fichero único): TEMA · ESCENAS · TIMELINE
├── guion.md              # guion del vídeo: escenas, tiempos, texto narrado
├── hyperframes.json      configuración (registry, rutas, authoringSkill)
├── meta.json             metadatos (id, nombre)
├── package.json          scripts (dev / check / render / …)
├── BRIEF.md              decisiones de este vídeo y de la base reutilizable
├── README.md             este archivo
├── audio/
│   ├── README.md         # cómo obtener la API key y generar el audio
│   ├── voices.json       # reference_id de voz por rol
│   ├── generate-audio.sh # bash + curl a fish.audio
│   └── generate-audio.mjs # Node (fetch nativo) como alternativa
├── renders/              salida MP4 (gitignored)
└── snapshots/            PNGs de snapshot (gitignored)
```

## Cómo está montado (reglas que conviene no romper)
- **Raíz**: `#root` (`data-composition-id="root"`, `data-duration="60"`) fija la duración total del render. Todo lo demás son clips dentro de ella.
- **Escenas = clips que se solapan**: portada `0–12,7` · creacionales `12–24,7` · estructurales `24–36,7` · comportamiento `36–48,7` · cierre `48–58`. Ese solape de 0,7 s ES la transición: el crossfade se anima sobre `.capa`.
- **Nunca se anima la visibilidad de un clip** (ni `display`/`visibility`/`autoAlpha`): el framework gestiona la visibilidad con `data-start`. Se funde con `opacity` sobre `.capa`.
- **Una sola línea de tiempo**: `window.__timelines["root"]`, pausada. Cada escena se cuelga con `tlRaiz.add(tlEscena, data-start)` y dentro de cada `tlEscena` los tiempos son LOCALES.
- **Las escenas llevan `data-composition-id`** (sub-composición inline): da identidad propia a cada escena.
- **Determinismo**: sin `Date.now()`/`performance.now()`, sin `Math.random()` sin semilla, sin red en render, sin `repeat: -1`.
- **Velos** `#velo-inicio` / `#velo-final`: fundido desde negro (0–0,6 s) y a negro (57,4–58 s, con negro puro sostenido hasta 60).
- **Tipografías**: Caveat/Kalam/JetBrains Mono por Google Fonts en el `<head>`; la CLI las incrusta en el build y las cachea en `~/.cache/hyperframes/fonts`.

## Linting — SIEMPRE EJECUTAR DESPUÉS DE CAMBIOS
Después de crear o editar cualquier `.html` composición, **siempre** ejecutar el check completo antes de considerar la tarea completa:

```bash
npm run check
```

Corregir todos los errores antes de presentar el resultado. Las revisiones (warnings) deben revisarse antes de renderizar.

> `renders/` y `snapshots/` están en `.gitignore`: no se versionan binarios.