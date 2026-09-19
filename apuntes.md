# Patrones de Diseño — Apuntes de estudio

Apuntes rápidos de los 20 patrones GoF implementados en este proyecto.
Para cada patrón: **intención** (qué resuelve), **analogía** (para recordarlo)
y **cuándo usarlo / cuándo NO usarlo**.

## Cómo ejecutar

```bash
dotnet run            # menú interactivo (elige 1-20)
echo 7 | dotnet run   # ejecutar un patrón directamente por número
```

---

## Creacionales (cómo se crean los objetos)

| # | Patrón | Analogía |
|---|--------|----------|
| 1 | Singleton | El presidente del país: solo hay uno |
| 2 | Factory Method | Fábrica de muebles; cada sucursal decide el material |
| 3 | Abstract Factory | Menú completo por cocina: italiano o mexicano, todo combina |
| 4 | Builder | Armar una hamburguesa por pasos (pan, carne, extras) |
| 5 | Prototype | Fotocopia de un formulario que luego personalizas |

### 1. Singleton — una sola instancia global

- **Intención:** garantizar que una clase tenga **una única instancia** y un punto de acceso global a ella.
- **Analogía:** el presidente del país: solo puede haber uno y todos lo consultan a él.
- **Cuándo usarlo:** configuración global, logger, caché compartida.
- **Cuándo NO usarlo:** cuando necesitas varias instancias en tests (acopla el código y dificulta el testing). En apps reales, la inyección de dependencias suele ser mejor opción.

### 2. Factory Method — creación delegada a subclases

- **Intención:** definir una interfaz para crear objetos, pero dejar que las **subclases decidan qué clase concreta crear**.
- **Analogía:** una franquicia de muebles: el proceso es estándar, pero cada taller decide si usa roble o pino.
- **Cuándo usarlo:** no sabes de antemano los tipos exactos que crearás; quieres que otros extiendan tu librería sin modificarla.
- **Cuándo NO usarlo:** si solo hay un tipo de producto y no habrá variaciones, un simple `new` es más simple.

### 3. Abstract Factory — familias de objetos compatibles

- **Intención:** crear **familias de objetos relacionados** sin acoplarse a sus clases concretas.
- **Analogía:** tienda de muebles con líneas completa "moderna" o "vintage": compras el set y todo combina.
- **Cuándo usarlo:** menús completos de distintas cocinas (italiano/mexicano), temas de UI, conectores a distintas bases de datos.
- **Cuándo NO usarlo:** si solo hay **un** producto por familia (para eso es Factory Method, más simple).

### 4. Builder — construcción paso a paso

- **Intención:** separar la construcción de un objeto complejo en **pasos encadenables**, evitando constructores gigantes ("constructores telescópicos").
- **Analogía:** armar un pedido en una app de comida: eliges entradas, plato fuerte, bebida, postre... paso a paso.
- **Cuándo usarlo:** objetos con muchos parámetros opcionales (consultas SQL, HTTP requests, pedidos).
- **Cuándo NO usarlo:** con 2-3 parámetros simples; el patrón agrega más código que el problema que resuelve.

### 5. Prototype — clonación de objetos

- **Intención:** crear nuevos objetos **copiando un prototipo existente**, en vez de construirlos desde cero.
- **Analogía:** fotocopia de un formulario en blanco prellenado.
- **Cuándo usarlo:** duplicar recetas/documentos plantilla, crear muchos enemigos similares en un videojuego.
- **Cuidado:** distingue **copia superficial** (comparte referencias: ¡la lista copiada es la misma!) de **copia profunda** (todo nuevo). Este proyecto demuestra ambas.

---

## Estructurales (cómo se componen las clases)

| # | Patrón | Analogía |
|---|--------|----------|
| 6 | Adapter | Adaptador de enchufe europeo → americano |
| 7 | Decorator | Ropa: se pone encima y agrega abrigo sin cambiar a la persona |
| 8 | Facade | Recepción de un hotel: un solo punto para todo |
| 9 | Proxy | Tarjeta de crédito: intermediario antes de tocar tu dinero |
| 10 | Composite | Carpeta que contiene archivos y otras carpetas |
| 11 | Bridge | Receta (casera/gourmet) independiente del método de cocción |
| 12 | Flyweight | Un solo molde de bebida para servir mil comandas |

### 6. Adapter — interfaces incompatibles → compatibles

- **Intención:** hacer que dos interfaces **incompatibles trabajen juntas** traduciendo llamadas.
- **Analogía:** adaptador de corriente entre un enchufe europeo y uno americano.
- **Cuándo usarlo:** integrar librerías de terceros o sistemas legacy cuya interfaz no puedes cambiar.
- **Cuándo NO usarlo:** si controlas ambas interfaces y puedes cambiarlas, mejor ajustarlas directamente.

### 7. Decorator — añadir funcionalidad dinámicamente

- **Intención:** **envolver** un objeto con otros que le agregan comportamiento, sin herencia ni modificar la clase original.
- **Analogía:** vestirse en capas: camiseta, suéter, abrigo — la persona es la misma, se agregan capas.
- **Cuándo usarlo:** cuando tendrías una explosión de subclases (pastel con glaseado+chispas+velitas...). Es la base de streams y middleware en .NET.
- **Cuándo NO usarlo:** si solo hay una variante posible, la herencia simple es más directa.

### 8. Facade — interfaz simple para subsistemas complejos

- **Intención:** exponer **una interfaz simple** que orquesta un subsistema complejo.
- **Analogía:** atención al cliente por teléfono: hablas con una persona que coordina facturación, envío y soporte por ti.
- **Cuándo usarlo:** cuando usar un sistema requiere llamar a varios subsistemas en orden (procesar un pedido: inventario → pago → envío → notificación).
- **Cuándo NO usarlo:** si los clientes siempre necesitan el control fino del subsistema; una fachada innecesaria es una capa más de indirección.

### 9. Proxy — control de acceso a un objeto

- **Intención:** poner un **sustituto** delante del objeto real para controlar el acceso (caché, permisos, lazy loading).
- **Analogía:** el asistente personal que filtra llamadas antes de pasarlas al jefe.
- **Cuándo usarlo:** cachear llamadas costosas, verificar permisos, lazy loading (Entity Framework lo usa).
- **Cuándo NO usarlo:** si el objeto es barato y no hay reglas de acceso; el proxy solo agrega complejidad.

### 10. Composite — árboles parte-todo

- **Intención:** tratar **objetos individuales y composiciones de objetos de forma uniforme** (estructura de árbol).
- **Analogía:** menú: un platillo y un combo entero responden la misma pregunta ("¿cuánto cuesta?").
- **Cuándo usarlo:** sistemas de archivos, menús, orgánigramas, árboles de componentes UI.
- **Cuándo NO usarlo:** si la estructura nunca es anidada (solo hay hojas), el árbol es innecesario.

### 11. Bridge — abstracción e implementación independientes

- **Intención:** separar una **abstracción** de su **implementación** para que ambas dimensiones (ej. receta / método de cocción) evolucionen sin multiplicarse.
- **Analogía:** receta (casera/gourmet) × método (horno/freidora/parrilla): dos perillas independientes.
- **Cuándo usarlo:** cuando tienes dos dimensiones de variación (sin Bridge: 2 recetas × 3 métodos = 6 clases; con Bridge: 2 + 3 = 5).
- **Cuándo NO usarlo:** con una sola dimensión, herencia simple basta.

### 12. Flyweight — compartir objetos en gran cantidad

- **Intención:** compartir el estado **intrínseco** (común) entre miles de objetos, dejando lo **extrínseco** (posición) en el cliente.
- **Analogía:** un solo molde de limonada reutilizado para servir mil comandas en mesas distintas.
- **Cuándo usarlo:** muchísimos objetos similares en memoria (tiles de videojuegos, caracteres de un documento).
- **Cuándo NO usarlo:** con pocos objetos o cuando no hay nada compartible; la factory complica sin beneficio.

---

## Comportamiento (cómo se comunican los objetos)

| # | Patrón | Analogía |
|---|--------|----------|
| 13 | Strategy | Elegir la ruta: auto, bici o caminar |
| 14 | Observer | Suscripción a un canal de YouTube |
| 15 | Command | Pedidos en una cocina: se encolan y se pueden anular |
| 16 | Template Method | Receta de cocina: pasos fijos, ingredientes variables |
| 17 | State | Estado de ánimo: cambia cómo reaccionas |
| 18 | Mediator | Torre de control: nadie coordina con nadie directamente |
| 19 | Memento | Guardar la pizza a medio armar por si la arruinas |
| 20 | Chain of Responsibility | La queja escala del mesero al dueño |

### 13. Strategy — algoritmos intercambiables

- **Intención:** encapsular **familias de algoritmos** intercambiables en tiempo de ejecución.
- **Analogía:** elegir cómo ir al trabajo: auto, bici o bus. El destino es el mismo; el medio cambia.
- **Cuándo usarlo:** varios "motores" intercambiables (impuestos por país, métodos de pago, compresión).
- **Cuándo NO usarlo:** si el algoritmo nunca cambia, inyectarlo es complejidad gratis.

### 14. Observer — suscripción uno-a-muchos

- **Intención:** cuando un objeto cambia, **todos sus suscriptores se notifican** automáticamente.
- **Analogía:** canal de YouTube: te suscribes y recibes cada video nuevo sin preguntar.
- **Cuándo usarlo:** eventos de UI, notificaciones push, sincronizar vistas.
- **Cuándo NO usarlo:** si un solo receptor escucha siempre, un callback directo es más simple. Cuidado con el orden de desuscripción (memory leaks).

### 15. Command — solicitudes como objetos

- **Intención:** encapsular una solicitud como objeto: se puede **parametrizar, encolar y deshacer** (undo/redo).
- **Analogía:** pedir en un restaurante: el mesero escribe el pedido (comando), el cocinero lo ejecuta; el pedido se puede anular.
- **Cuándo usarlo:** undo/redo (Ctrl+Z), colas de trabajos, macros, logging de acciones.
- **Cuándo NO usarlo:** si la operación nunca se deshace, encola ni parametriza, una llamada directa es más simple.

### 16. Template Method — esqueleto de algoritmo

- **Intención:** definir la **estructura fija** del algoritmo en la clase base; las subclases rellenan pasos concretos.
- **Analogía:** receta de pan: los pasos son fijos (mezclar, amasar, hornear); el tipo de harina lo eliges tú.
- **Cuándo usarlo:** pipelines con pasos comunes que varían en el detalle (preparar pizza/sushi/pasta).
- **Cuándo NO usarlo:** si cada implementación difiere mucho en el flujo, mejor composición (Strategy).

### 17. State — comportamiento según el estado

- **Intención:** un objeto **cambia de comportamiento según su estado interno**; cada estado es una clase.
- **Analogía:** el mismo botón de teléfono hace cosas distintas según si suena, estás hablando o en espera.
- **Cuándo usarlo:** flujos de pedidos, máquinas de estados de videojuegos, workflows de aprobación.
- **Cuándo NO usarlo:** con 2 estados triviales, un `enum` + `switch` es más simple y suficiente.

### 18. Mediator — comunicación centralizada

- **Intención:** evitar que N objetos se referencien entre sí; **todos se comunican a través de un mediador**.
- **Analogía:** torre de control: ningún piloto coordina con otro directamente; todo pasa por la torre.
- **Cuándo usarlo:** salas de chat, paneles UI que se afectan entre sí, coordinación de formularios.
- **Cuándo NO usarlo:** si solo dos objetos se comunican, el mediador es un intermediario innecesario.

### 19. Memento — guardar/restaurar estado

- **Intención:** capturar el estado interno de un objeto **sin violar su encapsulamiento** para restaurarlo después.
- **Analogía:** guardar partida en un videojuego y volver al checkpoint.
- **Cuándo usarlo:** checkpoints, undo de estado completo, snapshots.
- **Cuándo NO usarlo:** si el objeto es pequeño, guardar una copia simple basta; el patrón brilla con estado grande o privado.

### 20. Chain of Responsibility — cadena de manejadores

- **Intención:** pasar una solicitud por una **cadena de manejadores**; cada uno la procesa o la pasa al siguiente.
- **Analogía:** una queja en el restaurante: el mesero intenta resolverla; si no puede, escala al jefe de cocina, luego al gerente y por último al dueño.
- **Cuándo usarlo:** middleware, validaciones encadenadas, escalado de aprobaciones.
- **Cuándo NO usarlo:** si siempre sabes quién maneja la solicitud, un `switch` o llamada directa es más claro.

---

## Patrones GoF no incluidos (a propósito)

- **Interpreter** — raro en la práctica; en C# se cubre con expression trees / parsers.
- **Iterator** — C# ya lo trae de fábrica: `IEnumerable<T>`, `foreach`, LINQ.
- **Visitor** — especializado en árboles de sintaxis; poco útil en un proyecto introductorio.

## Material relacionado

- `notebooklm-patrones-diseno.md` — guía extendida por patrón (definición, participantes, comparativas, ejercicios), optimizada para NotebookLM.
- `README.md` — cómo ejecutar el proyecto.
