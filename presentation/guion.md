# Guion del vídeo · Patrones de Diseño GoF en C#

Duración total: 171,7 s (~2m52s). Cada escena dura lo que dura su MP3 + 1,2 s de silencio. Texto de narración por escena (al estilo `apuntes.md`: intención, analogía, cuándo usar / cuándo no).

---

## Escena 1 · Portada (0 – 30,9 s)

**Visual:** Diagrama de las **3 familias de patrones GoF** (creacionales, estructurales, comportamiento) con sus 20 patrones listados en bloques conectados. Título grande, subtítulo y tres bloques que representan cada familia con su cantidad de patrones y analogía principal.

**Narración:**
"Los patrones de diseño son soluciones probadas y reutilizables a problemas comunes en el software orientado a objetos. Fueron catalogados por la Gang of Four en 1994 y se dividen en tres familias: creacionales, que resuelven cómo se crean los objetos; estructurales, que resuelven cómo se componen las clases; y de comportamiento, que resuelven cómo se comunican entre sí. En este repositorio encontrarás veinte de ellos implementados en C# y .NET 10, siempre en español."

---

## Escena 2 · Creacionales (32,1 – 60,3 s)

**Visual:** Diagrama de la **familia creacional** con 5 tarjetas conectadas (Singleton → Factory Method → Abstract Factory → Builder → Prototype), cada una con su nombre, intención en una frase y analogía. Las tarjetas usan el color terracota de la familia creacional.

**Narración:**
"Los patrones creacionales controlan cómo nacen los objetos. Singleton garantiza una única instancia global, ideal para loggers o configuración. Factory Method deja que las subclases decidan qué clase crear. Abstract Factory crea familias de objetos que deben combinarse entre sí. Builder separa la construcción compleja en pasos encadenables, evitando constructores gigantes. Y Prototype crea objetos copiando un prototipo existente en vez de partir desde cero."

---

## Escena 3 · Estructurales (61,5 – 96,4 s)

**Visual:** Diagrama de la **familia estructural** con 7 tarjetas conectadas (Adapter → Decorator → Facade → Proxy → Composite → Bridge → Flyweight), cada una con su nombre, intención en una frase y analogía. Las tarjetas usan el color azul de la familia estructural.

**Narración:**
"Los patrones estructurales organizan cómo se componen las clases y objetos. Adapter hace compatible una interfaz ajena, como un adaptador de enchufe. Decorator añade responsabilidades nuevas sin modificar la clase original. Facade ofrece una interfaz simplificada sobre un subsistema complejo. Proxy controla el acceso a un objeto, por ejemplo con caché o seguridad. Composite trata objetos simples y compuestos de forma uniforme. Bridge desacopla una abstracción de su implementación. Y Flyweight comparte estado intrínseco para ahorrar memoria cuando hay muchos objetos similares."

---

## Escena 4 · Comportamiento (97,6 – 143,8 s)

**Visual:** Diagrama de la **familia de comportamiento** con 8 tarjetas conectadas (Strategy → Observer → Command → Template Method → State → Mediator → Memento → Chain of Responsibility), cada una con su nombre, intención en una frase y analogía. Las tarjetas usan el color verde de la familia de comportamiento.

**Narración:**
"Los patrones de comportamiento gestionan la comunicación y las responsabilidades entre objetos. Strategy permite intercambiar algoritmos en tiempo de ejecución. Observer avisa a varios interesados cuando algo cambia, ideal para eventos de UI. Command encapsula una solicitud como objeto para deshacerla o encolarla. Template Method define el esqueleto de un algoritmo y deja que las subclases rellenen pasos. State hace que un objeto cambie de comportamiento según su estado interno. Mediator centraliza la comunicación mediante un intermediario. Memento captura y restaura estado sin romper el encapsulamiento. Y Chain of Responsibility pasa una solicitud por una cadena de manejadores hasta que alguien la resuelva."

---

## Escena 5 · Cierre (145,0 – 170,1 s)

**Visual:** Pizarra oscura con dos diagramas: arriba, los **3 bloques de familias** (creacionales, estructurales, comportamiento) con sus cantidades; abajo, un **diagrama de combinación** que muestra cómo Abstract Factory → Strategies → Mediator → Observer se conectan entre sí. Texto de los patrones no incluidos y CTA "dotnet run".

**Narración:**
"Este proyecto incluye veinte de los veintitrés patrones GoF. Faltan a propósito Interpreter, Iterator y Visitor: Iterator ya lo cubre C# con IEnumerable y LINQ, Visitor es muy especializado e Interpreter es raro en la práctica. Lo más poderoso es que los patrones se combinan: un Abstract Factory puede crear Strategies, coordinadas por un Mediator que notifica cambios vía Observer. Pruébalo tú mismo con dotnet run en el repositorio."

---

## Notas de producción
- Voz: narración clara y pausada, español neutro.
- Música de fondo opcional (no generada por fish.audio en este guion).
- Los tiempos de `data-start` / `data-duration` y de los resaltes NO se editan a mano: los
  calcula `npm run timing` a partir de los MP3 (ver `README.md`).