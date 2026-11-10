# Vídeo · Patrones de Diseño GoF en C#

Decisiones de este vídeo y base reutilizable (copiar para el próximo).

## Intención
Un vídeo **faceless explainer** de 60 s que repasa los **20 patrones de diseño GoF** implementados en el repositorio `DesignPatterns` (C# / .NET 10), organizados por familia: creacionales, estructurales y de comportamiento. Cada patrón se presenta con **intención + analogía + cuándo usar / cuándo no**, tal como se explica en `apuntes.md` y `notebooklm-patrones-diseno.md`.

## Estilo (al estilo hyperframes-video)
- **Composición única** `index.html`: TEMA (CSS) · ESCENAS (HTML) · TIMELINE (script final).
- Resolución **1920×1080**, duración total **60 s** en `#root`.
- Tema visual: papel crudo (`--papel`), tinta casi negra (`--tinta`), acento terracota (`--acento`), reglas suaves (`--regla`), pizarra oscura para el cierre (`--tiza`), tres fuentes manuscritas (Caveat, Kalam, JetBrains Mono).
- Escenas = clips con `data-start` / `data-duration` solapados 0,7 s (ese solape ES el crossfade).
- Una sola línea de tiempo `window.__timelines["root"]` pausada; `tlRaiz.add(tlEscena, data-start)` con tiempos locales por escena.
- Velos `#velo-inicio` / `#velo-final` (fundido desde/hacia negro).
- **Determinista**: sin `Date.now()`, `Math.random()`, fetches en render, `repeat: -1`.

## Ritmo (60 s)
| Tiempo | Qué pasa |
|--------|----------|
| 0 – 0,6 | velo de apertura (fundido desde negro) |
| 0 – 12 | Escena 1 · portada: qué son los patrones GoF, 3 familias, 20 patrones |
| 11,3 – 12 | crossfade → escena 2 |
| 12 – 24 | Escena 2 · creacionales: Singleton, Factory Method, Abstract Factory, Builder, Prototype |
| 23,3 – 24 | crossfade → escena 3 |
| 24 – 36 | Escena 3 · estructurales: Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight |
| 35,3 – 36 | crossfade → escena 4 |
| 36 – 48 | Escena 4 · comportamiento: Strategy, Observer, Command, Template Method, State, Mediator, Memento, Chain of Responsibility |
| 47,3 – 48 | crossfade → escena 5 |
| 48 – 58 | Escena 5 · cierre: no incluidos, combinaciones, call to action "dotnet run" |
| 57,4 – 58 | velo final (fundido a negro; negro puro hasta 60) |

## Cómo hacer el próximo vídeo
1. Duplicar esta carpeta.
2. Editar `TEMA` (variables CSS) y `ESCENAS` (HTML de cada `<section class="clip">`).
3. Ajustar `data-start` / `data-duration` y `tlRaiz.add(...)`.
4. `npm run check` → `npm run snapshot` → `npm run render`.

## Audio (narración)
Generado con la API de **https://fish.audio** (`POST /v1/tts`). Ver `audio/README.md`.