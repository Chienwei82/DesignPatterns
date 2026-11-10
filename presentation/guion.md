# Guion del vídeo · Patrones de Diseño GoF en C#

Duración total: ~60 s. Texto de narración por escena (al estilo `apuntes.md`: intención, analogía, cuándo usar / cuándo no).

---

## Escena 1 · Portada (0 – 12 s)

**Visual:** título grande "Patrones de Diseño GoF en C#", subtítulo "20 patrones, 3 familias, un mismo propósito: escribir software más mantenible". Listado de las 3 familias.

**Narración:**
"Los patrones de diseño son soluciones probadas y reutilizables a problemas comunes en el software orientado a objetos. Fueron catalogados por la Gang of Four en 1994 y se dividen en tres familias: creacionales, que resuelven cómo se crean los objetos; estructurales, que resuelven cómo se componen las clases; y de comportamiento, que resuelven cómo se comunican entre sí. En este repositorio encontrarás veinte de ellos implementados en C# y .NET 10, siempre en español."

---

## Escena 2 · Creacionales (12 – 24 s)

**Visual:** tarjetas por patrón: Singleton ("una sola instancia"), Factory Method ("delegar la creación"), Abstract Factory ("familias compatibles"), Builder ("paso a paso"), Prototype ("clonar").

**Narración:**
"Los patrones creacionales controlan cómo nacen los objetos. Singleton garantiza una única instancia global, ideal para loggers o configuración. Factory Method deja que las subclases decidan qué clase crear. Abstract Factory crea familias de objetos que deben combinarse entre sí. Builder separa la construcción compleja en pasos encadenables, evitando constructores gigantes. Y Prototype crea objetos copiando un prototipo existente en vez de partir desde cero."

---

## Escena 3 · Estructurales (24 – 36 s)

**Visual:** tarjetas: Adapter ("enchufe europeo a americano"), Decorator ("añadir responsabilidades"), Facade ("ventanilla única"), Proxy ("controlar acceso"), Composite ("árbol de objetos"), Bridge ("desacoplar abstracción"), Flyweight ("compartir estado").

**Narración:**
"Los patrones estructurales organizan cómo se componen las clases y objetos. Adapter hace compatible una interfaz ajena, como un adaptador de enchufe. Decorator añade responsabilidades nuevas sin modificar la clase original. Facade ofrece una interfaz simplificada sobre un subsistema complejo. Proxy controla el acceso a un objeto, por ejemplo con caché o seguridad. Composite trata objetos simples y compuestos de forma uniforme. Bridge desacopla una abstracción de su implementación. Y Flyweight comparte estado intrínseco para ahorrar memoria cuando hay muchos objetos similares."

---

## Escena 4 · Comportamiento (36 – 48 s)

**Visual:** tarjetas: Strategy ("cambiar algoritmo en vuelo"), Observer ("suscribirse y notificar"), Command ("pedido como objeto"), Template Method ("esqueleto fijo"), State ("cambiar según estado"), Mediator ("torre de control"), Memento ("guardar partida"), Chain of Responsibility ("cadena de aprobación").

**Narración:**
"Los patrones de comportamiento gestionan la comunicación y las responsabilidades entre objetos. Strategy permite intercambiar algoritmos en tiempo de ejecución. Observer avisa a varios interesados cuando algo cambia, ideal para eventos de UI. Command encapsula una solicitud como objeto para deshacerla o encolarla. Template Method define el esqueleto de un algoritmo y deja que las subclases rellenen pasos. State hace que un objeto cambie de comportamiento según su estado interno. Mediator centraliza la comunicación mediante un intermediario. Memento captura y restaura estado sin romper el encapsulamiento. Y Chain of Responsibility pasa una solicitud por una cadena de manejadores hasta que alguien la resuelva."

---

## Escena 5 · Cierre (48 – 58 s)

**Visual:** pizarra oscura. Texto: "No están: Interpreter, Iterator, Visitor. ¿Por qué? Iterator ya lo cubre C# con IEnumerable y LINQ. Visitor es muy especializado. Interpreter es raro en la práctica." Y abajo: "Los patrones se combinan: un Abstract Factory puede crear Strategies manejadas por un Mediator. Prueba los ejemplos: dotnet run."

**Narración:**
"Este proyecto incluye veinte de los veintitrés patrones GoF. Faltan a propósito Interpreter, Iterator y Visitor: Iterator ya lo cubre C# con IEnumerable y LINQ, Visitor es muy especializado e Interpreter es raro en la práctica. Lo más poderoso es que los patrones se combinan: un Abstract Factory puede crear Strategies, coordinadas por un Mediator que notifica cambios vía Observer. Pruébalo tú mismo con dotnet run en el repositorio."

---

## Notas de producción
- Voz: narración clara y pausada, español neutro.
- Música de fondo opcional (no generada por fish.audio en este guion).
- Los tiempos de `data-start` / `data-duration` en `index.html` deben coincidir con el ritmo del guion.