# Vídeo · Patrones de Diseño GoF en C#

Decisiones de este vídeo y base reutilizable (copiar para el próximo).

## Intención
Un vídeo **faceless explainer** de 60 s que repasa los **20 patrones de diseño GoF** implementados en el repositorio `DesignPatterns` (C# / .NET 10), organizados por familia: creacionales, estructurales y de comportamiento. Cada patrón se presenta con **intención + analogía + cuándo usar / cuándo no**, tal como se explica en `apuntes.md` y `notebooklm-patrones-diseno.md`.

## Estilo (al estilo hyperframes-video)
- **Composición única** `index.html`: TEMA (CSS) · ESCENAS (HTML) · TIMELINE (script final).
- Resolución **1920×1080**, duración total **la de la narración** (171,7 s con el audio actual) en `#root`.
- Tema visual: papel crudo (`--papel`), tinta casi negra (`--tinta`), acento terracota (`--acento`), reglas suaves (`--regla`), pizarra oscura para el cierre (`--tiza`), tres fuentes manuscritas (Caveat, Kalam, JetBrains Mono).
- Escenas = clips con `data-start` / `data-duration` solapados 0,8 s (ese solape ES el crossfade).
- Una sola línea de tiempo `window.__timelines["root"]` pausada; `tlRaiz.add(tlEscena, escena.inicio)` con tiempos locales por escena.
- **Resaltes sincronizados**: cada patrón (y cada término clave) lleva `data-mencion`; el guion marca cuándo se ilumina.
- Velos `#velo-inicio` / `#velo-final` (fundido desde/hacia negro).
- **Determinista**: sin `Date.now()`, `Math.random()`, fetches en render, `repeat: -1`.

## Ritmo (171,7 s = 2m51,7s)
| Tiempo | Qué pasa |
|--------|----------|
| 0 – 0,8 | velo de apertura (fundido desde negro) |
| 0 – 30,9 | Escena 1 · portada: qué son los patrones GoF, 3 familias (cada una se resalta al nombrarla), C#/.NET 10 |
| 31,25 – 32,05 | crossfade → escena 2 |
| 32,05 – 60,3 | Escena 2 · creacionales: Singleton, Factory Method, Abstract Factory, Builder, Prototype (card resaltada al nombrarla) |
| 60,66 – 61,46 | crossfade → escena 3 |
| 61,46 – 96,4 | Escena 3 · estructurales: Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight |
| 96,84 – 97,64 | crossfade → escena 4 |
| 97,64 – 143,8 | Escena 4 · comportamiento: Strategy, Observer, Command, Template Method, State, Mediator, Memento, Chain of Responsibility |
| 144,20 – 145,00 | crossfade → escena 5 |
| 145,00 – 170,1 | Escena 5 · cierre: no incluidos, combinaciones, call to action "dotnet run" |
| 170,50 – 171,30 | velo final (fundido a negro; negro puro hasta el final) |

Cada escena dura exactamente su MP3 + 1,2 s de silencio, así que **ninguna frase queda cortada**.
Los tiempos los calcula `npm run timing` a partir de los MP3: si regeneras el audio, vuelve a
ejecutarlo y todo (escenas, crossfades, resaltes y audio final) se recoloca solo.

## Cómo hacer el próximo vídeo
1. Duplicar esta carpeta.
2. Editar `TEMA` (variables CSS) y `ESCENAS` (HTML de cada `<section class="clip">`).
3. Ajustar `data-start` / `data-duration` y `tlRaiz.add(...)`.
4. `npm run check` → `npm run snapshot` → `npm run render`.

## Audio (narración)
Generado con la API de **https://fish.audio** (`POST /v1/tts`). Ver `audio/README.md`.