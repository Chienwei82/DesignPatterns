# Guía Completa de Patrones de Diseño GoF — Material para NotebookLM

> Este documento resume los 23 patrones clásicos del libro *Design Patterns: Elements of Reusable Object-Oriented Software* (Gang of Four), implementados en C# 10/.NET 10. Está optimizado para ingestión por herramientas de IA como NotebookLM.

---

## 1. Introducción: ¿Qué son los Patrones de Diseño?

Los patrones de diseño son **soluciones probadas y reutilizables** a problemas comunes en el diseño de software orientado a objetos. No son librerías ni código copy-paste, sino **plantillas conceptuales** que guían la estructura de clases y objetos.

Fueron catalogados por **Erich Gamma, Richard Helm, Ralph Johnson y John Vlissides** ("Gang of Four" o GoF) en 1994. Se dividen en tres familias según su propósito:

| Familia | Propósito | Patrones incluidos |
|---------|-----------|-------------------|
| **Creacionales** | Cómo se crean los objetos | Singleton, Factory Method, Abstract Factory, Builder, Prototype |
| **Estructurales** | Cómo se componen las clases y objetos | Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight |
| **De Comportamiento** | Cómo se comunican y distribuyen responsabilidades | Strategy, Observer, Command, Template Method, State, Mediator, Memento, Chain of Responsibility |

**Nota importante:** Este proyecto contiene implementaciones educativas de **20 de los 23 patrones GoF**. Faltan intencionalmente: Interpreter, Iterator y Visitor (más especializados o ya cubiertos por el lenguaje C#).

---

## 2. Claves para entender cualquier patrón

Antes de estudiar cada uno, estos son los conceptos transversales que aparecen en casi todos:

- **Composición sobre herencia:** Muchos patrones prefieren "tener un objeto" (composición) en vez de "ser un objeto" (herencia).
- **Programar contra interfaces, no contra implementaciones:** Depender de abstracciones (`interface`, `abstract class`) en lugar de clases concretas.
- **Principio de responsabilidad única (SRP):** Cada clase debe tener una sola razón para cambiar.
- **Principio abierto/cerrado (OCP):** Abierto a extensión, cerrado a modificación.

---

## 3. Patrones Creacionales (Cómo nacen los objetos)

### 3.1 Singleton

**Definición:** Garantiza que una clase tenga **una única instancia** en toda la aplicación y provee un punto de acceso global a ella.

**Propósito:** Controlar el acceso a recursos compartidos (configuración, conexiones a BD, logger, caché).

**Cuándo usarlo:**
- Necesitas exactamente un objeto coordinando acciones en todo el sistema.
- El costo de crear el objeto es alto y se usa frecuentemente.

**Participantes:**
- `Singleton`: la clase con constructor privado y método de acceso estático.

**Flujo típico:**
1. La primera vez que se solicita la instancia, se crea.
2. Las siguientes veces, se devuelve la misma referencia.
3. En .NET moderno, `Lazy<T>` garantiza thread-safety sin locks explícitos.

**Analogía:** El presidente de un país. Solo puede haber uno en ejercicio, y todos los ciudadanos (objetos) se dirigen a esa única figura.

**Ejemplo real del proyecto:** Una clase `ConfiguracionApp` que carga parámetros desde un archivo JSON. Usar `Lazy<T>` asegura que, aunque dos hilos la soliciten simultáneamente, solo se crea una vez.

**Relaciones:** A menudo se usa con **Factory Method** (la factory puede ser Singleton) o **Abstract Factory**.

---

### 3.2 Factory Method

**Definición:** Define una interfaz para crear objetos, pero **delega a las subclases** la decisión de qué clase concreta instanciar.

**Propósito:** Desacoplar el código cliente del proceso de creación. El cliente depende de una abstracción, no de una clase concreta.

**Cuándo usarlo:**
- No sabes de antemano los tipos exactos de objetos que creará tu código.
- Quieres que los usuarios de tu librería extiendan los tipos sin modificar tu código.

**Participantes:**
- `Product`: interfaz común de los objetos creados.
- `ConcreteProduct`: implementaciones específicas.
- `Creator`: clase abstracta con el Factory Method declarado.
- `ConcreteCreator`: subclases que deciden qué `ConcreteProduct` crear.

**Flujo típico:**
1. El cliente crea un `ConcreteCreator`.
2. Llama a un método de la clase `Creator` que internamente invoca `FactoryMethod()`.
3. El `ConcreteCreator` devuelve su `ConcreteProduct` correspondiente.

**Analogía:** Una fábrica de muebles que tiene una línea de producción estándar. Las sucursales específicas (Madrid, Barcelona) deciden si usan madera de roble o pino, pero el proceso general es el mismo.

**Ejemplo real del proyecto:** Un sistema de notificaciones donde `NotificadorFactory` es la clase abstracta y existen `FactoryEmail`, `FactorySMS` y `FactoryPush`. Cada una configura parámetros específicos (servidor SMTP, número origen, App ID) antes de devolver el notificador concreto.

**Relaciones:** Se combina frecuentemente con **Singleton** (la factory puede ser única) o con **Strategy** (los productos creados suelen ser estrategias intercambiables).

---

### 3.3 Abstract Factory

**Definición:** Proporciona una interfaz para crear **familias de objetos relacionados** sin especificar sus clases concretas.

**Propósito:** Asegurar que los productos creados sean compatibles entre sí. Si creas un botón de un tema oscuro, la ventana también debe ser oscura.

**Cuándo usarlo:**
- Tu sistema debe ser independiente de cómo se crean, componen y representan sus productos.
- Necesitas garantizar que los objetos de una familia se usen juntos (temas UI, drivers de BD, toolkit multiplataforma).

**Participantes:**
- `AbstractFactory`: declara métodos para crear cada producto abstracto.
- `ConcreteFactory`: implementa esos métodos para una familia específica.
- `AbstractProduct`: interfaces de cada tipo de producto.
- `ConcreteProduct`: implementaciones de una familia.

**Flujo típico:**
1. El cliente recibe una `AbstractFactory` (no sabe cuál concreta).
2. Pide un `Boton` y una `Ventana`.
3. La factory concreta devuelve siempre productos del mismo tema/familia.

**Analogía:** Un restaurante de comida rápida con menús para niños y adultos. El menú infantil siempre incluye: hamburguesa pequeña, papas pequeñas y juguete. El menú adulto incluye: hamburguesa grande, papas grandes y bebida. Nunca mezclas papas pequeñas con hamburguesa grande.

**Ejemplo real del proyecto:** `IUIFactory` con métodos `CrearBoton()` y `CrearVentana()`. Las implementaciones `TemaClaroFactory`, `TemaOscuroFactory` y `TemaAltoContrasteFactory` garantizan que botón y ventana siempre vayan del mismo tema.

**Relaciones:** Abstract Factory suele implementarse con **Factory Methods** (cada método de la factory es un factory method). A menudo la Abstract Factory misma es un **Singleton**.

---

### 3.4 Builder

**Definición:** Separa la construcción de un objeto complejo de su representación, permitiendo que el **mismo proceso de construcción** cree **diferentes representaciones**.

**Propósito:** Evitar constructores con 10+ parámetros. Construir objetos paso a paso, opcionalmente, con una sintaxis legible.

**Cuándo usarlo:**
- El algoritmo para crear un objeto debe ser independiente de las partes que lo componen.
- El proceso de construcción debe permitir distintas representaciones del objeto final.

**Participantes:**
- `Builder`: interfaz abstracta con pasos de construcción.
- `ConcreteBuilder`: implementa los pasos y expone el producto final.
- `Director` (opcional): orquesta el orden de los pasos para recetas comunes.
- `Product`: el objeto complejo resultante.

**Flujo típico:**
1. Creas un `ConcreteBuilder`.
2. Opcionalmente, pasas un `Director` con una receta predefinida (ej. "pedido premium").
3. El director llama a los pasos en orden: `AgregarCliente()`, `AgregarProducto()`, etc.
4. Finalmente se llama `Construir()` para obtener el producto terminado.

**Analogía:** Un chef en un restaurante. El cliente puede pedir paso a paso: base de masa, salsa, queso, pepperoni, orégano. O puede pedir "pizza pepperoni" que el chef (Director) sabe cómo armar.

**Ejemplo real del proyecto:** `PedidoBuilder` construye un `PedidoComplejo` paso a paso. `DirectorPedidos` tiene recetas como `ConstruirPedidoSimple()` y `ConstruirPedidoPremium()`. Tras `Construir()`, el builder se resetea automáticamente para poder reutilizarse.

**Relaciones:** A menudo se usa junto a **Composite** (el producto final es una estructura compuesta) o **Bridge** (el producto puede usar diferentes implementaciones).

---

### 3.5 Prototype

**Definición:** Permite copiar objetos existentes sin que el código dependa de sus clases concretas. El objeto original sirve como "prototipo".

**Propósito:** Crear nuevos objetos clonando uno existente, especialmente cuando la inicialización es costosa o compleja.

**Cuándo usarlo:**
- Crear un objeto desde cero es más costoso que copiar uno existente.
- Necesitas objetos similares pero con pequeñas variaciones.
- Quieres evitar la complejidad de subclases solo para variar estados.

**Participantes:**
- `Prototype`: interfaz con método `Clone()`.
- `ConcretePrototype`: implementa la clonación (shallow o deep).
- `Client`: solicita la clonación.

**Flujo típico:**
1. Creas un objeto base (prototipo) y lo configuras.
2. Cuando necesitas uno similar, llamas `Clone()` en vez de `new`.
3. Ajustas las propiedades que difieren.

**Analogía:** Un formulario pre-llenado. En vez de pedirle a un usuario que llene todos los campos desde cero, le das una copia de otro formulario similar y solo cambia los datos que difieren.

**Ejemplo real del proyecto:** `Factura` implementa `ICloneable`. Se demuestra la diferencia entre `Clone()` (shallow copy, comparte la `List<string>` y `DatosFiscales`) y `DeepClone()` (copia profunda, todo es independiente). `DatosFiscales` se implementó como un `record` inmutable.

**Relaciones:** El **Prototype** puede usarse para inicializar **Singletons** o puede ser creado por una **Factory**.

---

## 4. Patrones Estructurales (Cómo se componen las clases)

### 4.1 Adapter

**Definición:** Permite que clases con interfaces **incompatibles** trabajen juntas. Actúa como traductor.

**Propósito:** Integrar código legacy, APIs de terceros o bibliotecas cuyas interfaces no coinciden con las que espera tu sistema.

**Cuándo usarlo:**
- Quieres usar una clase existente pero su interfaz no coincide con la que necesitas.
- Necesitas crear una clase reutilizable que coopere con clases no previstas.

**Participantes:**
- `Target`: la interfaz que el cliente espera.
- `Adaptee`: la clase existente con interfaz incompatible.
- `Adapter`: implementa `Target` y traduce llamadas hacia `Adaptee`.

**Flujo típico:**
1. El cliente trabaja con `Target`.
2. El `Adapter` implementa `Target`.
3. Internamente, el `Adapter` convierte los datos y llama a `Adaptee`.

**Analogía:** Un adaptador de corriente. Tu cargador tiene enchufe tipo A (plano), pero el hotel solo tiene tipo C (redondo). El adaptador convierte uno en otro sin modificar ni el cargador ni la toma de pared.

**Ejemplo real del proyecto:** `IPagoProcesador` es la interfaz esperada. Existen dos `Adaptees`: `GatewayPagoInternacional` (simulando Stripe) y `PasarelaPagoLegacy` (sistema antiguo). `PagoAdapter` y `LegacyAdapter` traducen las llamadas.

**Relaciones:** El **Adapter** puede adaptar no solo una clase, sino todo un subsistema (convirtiéndose en un tipo de **Facade**). También se parece al **Proxy**, pero el Adapter cambia la interfaz; el Proxy mantiene la misma interfaz.

---

### 4.2 Decorator

**Definición:** Permite añadir comportamientos a objetos de forma **dinámica**, envolviéndolos en objetos decorador.

**Propósito:** Extender funcionalidad sin herencia. Combinar comportamientos en tiempo de ejecución.

**Cuándo usarlo:**
- Necesitas agregar responsabilidades a objetos de forma dinámica y transparente.
- La extensión por subclases sería impracticable (explosión combinatoria de clases).

**Participantes:**
- `Component`: interfaz común.
- `ConcreteComponent`: el objeto base.
- `Decorator`: clase abstracta que delega al componente.
- `ConcreteDecorator`: añade comportamiento específico.

**Flujo típico:**
1. Creas un `ConcreteComponent`.
2. Lo envuelves en un `ConcreteDecorator`.
3. Ese resultado lo envuelves en otro `ConcreteDecorator`.
4. Cada capa añade su funcionalidad antes/después de delegar al interior.

**Analogía:** Un árbol de Navidad. El pino base es el componente. Le agregas luces (decorador), luego esferas (otro decorador), luego un ángel en la punta. Puedes quitar o cambiar cualquier capa sin tocar el pino.

**Ejemplo real del proyecto:** `ICafe` es el componente. `CafeSimple` es la base. `ConLeche`, `ConCrema`, `ConCaramelo` y `ConCanela` son decoradores que suman costo y descripción. Un café puede ser `new ConCaramelo(new ConLeche(new CafeSimple()))`.

**Relaciones:** El **Decorator** tiene la misma estructura de objetos que **Composite** (árbol), pero el Decorator solo tiene un hijo. También se asemeja al **Proxy** (ambos envuelven objetos), pero el Proxy controla acceso, mientras el Decorator añade responsabilidades.

---

### 4.3 Facade

**Definición:** Proporciona una **interfaz simplificada** para un subsistema complejo.

**Propósito:** Reducir la curva de aprendizaje y el acoplamiento al ocultar la complejidad interna de múltiples subsistemas.

**Cuándo usarlo:**
- Necesitas proporcionar una interfaz simple para un subsistema complejo.
- Hay muchas dependencias entre clientes y clases de implementación.
- Quieres desacoplar los subsistemas de los clientes.

**Participantes:**
- `Facade`: la clase que expone la interfaz simple.
- `Subsystem classes`: las clases complejas que la Facade coordina.

**Flujo típico:**
1. El cliente llama un solo método de la `Facade`.
2. La `Facade` coordina múltiples llamadas internas a los subsistemas.
3. El cliente nunca interactúa directamente con los subsistemas.

**Analogía:** El mostrador de atención al cliente en un banco. Tú solo hablas con el cajero (Facade). Internamente, él consulta el sistema de cuentas, el sistema de préstamos, el sistema de seguridad y te da una respuesta única.

**Ejemplo real del proyecto:** `FacadePedido` orquesta 4 subsistemas: `SubsistemaInventario`, `SubsistemaPagos`, `SubsistemaEnvios` y `SubsistemaNotificaciones`. El cliente llama `RealizarPedido()` y la Facade maneja el flujo completo. Además incluye un constructor con inyección de dependencias para testear con mocks.

**Relaciones:** Una **Facade** puede usar internamente **Factory Methods** para crear objetos del subsistema. Puede interactuar con **Singletons** (si los subsistemas son únicos). No transforma la interfaz como el **Adapter**, solo la simplifica.

---

### 4.4 Proxy

**Definición:** Proporciona un sustituto o representante de otro objeto para **controlar el acceso** a él.

**Propósito:** Interponerse entre el cliente y el objeto real para agregar lógica: lazy loading, control de acceso, logging, caché, o llamadas remotas.

**Cuándo usarlo:**
- El objeto es costoso de crear y quieres posponer su creación.
- Necesitas controlar acceso según permisos (seguridad).
- Quieres agregar logging o caché sin modificar el objeto real.

**Participantes:**
- `Subject`: interfaz común.
- `RealSubject`: el objeto costoso o sensible.
- `Proxy`: implementa `Subject`, intercepta llamadas y delega a `RealSubject`.

**Flujo típico:**
1. El cliente trabaja con `Subject`.
2. El `Proxy` intercepta la llamada.
3. El `Proxy` decide: devolver caché, rechazar por permisos, o delegar al `RealSubject`.

**Analogía:** Un abogado. Tú (cliente) no hablas directamente con el juez (objeto real), sino con tu abogado (proxy). Él decide si tu solicitud procede, prepara la documentación y la presenta al juez.

**Ejemplo real del proyecto:** Se implementan dos proxies sobre `IServicioDatos`:
- `ProxyCache`: guarda resultados de `ObtenerDatos()` por 30 segundos usando `DateTimeOffset.UtcNow`. La segunda llamada con el mismo ID es instantánea.
- `ProxySeguridad`: verifica el rol del usuario antes de permitir el acceso. Muestra tanto el caso denegado (usuario normal) como el permitido (administrador).

**Relaciones:** El **Proxy** tiene la misma interfaz que el objeto real, a diferencia del **Adapter**. Un **Proxy** de caché se parece al **Flyweight** (ambos evitan crear objetos), pero el Proxy controla un único objeto real, mientras el Flyweight comparte muchos objetos pequeños.

---

### 4.5 Composite

**Definición:** Permite tratar objetos **individuales y composiciones** de objetos de manera uniforme. Organiza objetos en estructuras de árbol (parte-todo).

**Propósito:** Representar jerarquías parte-todo donde el cliente no necesita distinguir entre hojas y compuestos.

**Cuándo usarlo:**
- Quieres representar una jerarquía de objetos tipo árbol.
- Quieres que el cliente ignore la diferencia entre composiciones de objetos y objetos individuales.

**Participantes:**
- `Component`: interfaz común para hojas y compuestos.
- `Leaf`: objeto individual sin hijos.
- `Composite`: objeto que contiene hijos (otros Components).

**Flujo típico:**
1. El cliente trabaja con `Component`.
2. Puede llamar `Operacion()` tanto en una hoja como en un compuesto.
3. El `Composite` delega la operación a todos sus hijos y agrega los resultados.

**Analogía:** Una empresa. Un empleado individual (hoja) tiene un salario. Un departamento (composite) contiene empleados y sub-departamentos. Para calcular el presupuesto total, simplemente sumas el salario de todos, sin importar si es una persona o un departamento entero.

**Ejemplo real del proyecto:** `IComponenteOrganigrama` con `Empleado` (hoja) y `Departamento` (composite). El composite usa LINQ (`Sum`) para calcular costos totales y contar personas. El cliente llama `Mostrar()` o `GetCostoTotal()` sin saber si es hoja o compuesto.

**Relaciones:** El **Composite** es estructuralmente similar al **Decorator** (ambos usan composición recursiva), pero el Composite modela jerarquías parte-todo, mientras el Decorator añade responsabilidades. Frecuentemente se usa junto al **Iterator** para recorrer la estructura.

---

### 4.6 Bridge

**Definición:** Separa una **abstracción** de su **implementación**, permitiendo que ambas evolucionen independientemente.

**Propósito:** Evitar la "explosión de subclases" cuando tienes múltiples dimensiones de variación. Ejemplo: si tienes 3 formas y 3 colores, sin Bridge necesitas 9 clases; con Bridge, 3 + 3 = 6.

**Cuándo usarlo:**
- Quieres evitar un vínculo permanente entre una abstracción y su implementación.
- Tanto la abstracción como la implementación deben poder extenderse por subclases.
- Quieres ocultar la implementación completamente a los clientes.

**Participantes:**
- `Abstraction`: define la interfaz del cliente, mantiene referencia a `Implementor`.
- `RefinedAbstraction`: extiende la abstracción.
- `Implementor`: interfaz de la implementación.
- `ConcreteImplementor`: implementaciones concretas.

**Flujo típico:**
1. El cliente crea una `Abstraction` pasándole un `Implementor`.
2. Llama a métodos de la abstracción.
3. La abstracción delega las operaciones de bajo nivel al `Implementor`.

**Analogía:** Un control remoto universal. El control (abstracción) puede aprender comandos de cualquier TV (implementación). Agregar una nueva marca de TV no requiere cambiar el control remoto. Agregar funciones al control (voz, app) no requiere cambiar las TVs.

**Ejemplo real del proyecto:** `ControlRemoto` es la abstracción con subclases `ControlBasico` y `ControlAvanzado`. `IPlataformaTV` es el implementor con `SamsungTV`, `LgTV` y `SonyTV`. Un `ControlAvanzado` puede usar cualquier TV.

**Relaciones:** El **Bridge** es similar al **Adapter**, pero el Bridge desacopla desde el diseño (pre-planificado), mientras el Adapter corrige incompatibilidades existentes (post-hoc). A menudo el producto de un **Builder** usa un **Bridge** para ser independiente de su implementación.

---

### 4.7 Flyweight

**Definición:** Comparte eficientemente objetos que se usan en gran cantidad. Divide el estado en **intrínseco** (compartido) y **extrínseco** (proporcionado por el contexto).

**Propósito:** Reducir el uso de memoria cuando un programa contiene una enorme cantidad de objetos similares.

**Cuándo usarlo:**
- Tu aplicación usa una gran cantidad de objetos.
- Los costos de almacenamiento son altos por la cantidad de objetos.
- La mayoría del estado de los objetos puede volverse extrínseco.

**Participantes:**
- `Flyweight`: la clase con estado intrínseco compartido.
- `FlyweightFactory`: crea y administra los flyweights, asegurando que se compartan.
- `Client`: mantiene el estado extrínseco y lo pasa al flyweight cuando opera.

**Flujo típico:**
1. El cliente pide un objeto al `FlyweightFactory`.
2. Si ya existe uno con esas características intrínsecas, se reutiliza.
3. Si no, se crea y se registra.
4. El cliente pasa el estado extrínseco (posición, contexto) en cada llamada.

**Analogía:** Un cine con 500 butacas. En vez de crear 500 objetos "Butaca" con su propio proyector, cada butaca solo guarda su fila/columna (extrínseco). El proyector, la película y la pantalla son compartidos (intrínsecos) por todas.

**Ejemplo real del proyecto:** `TipoArbol` es el flyweight con nombre, color y textura (intrínseco). `Arbol` es el contexto con posición X/Y (extrínseco). `FabricaArboles` administra los tipos. Al plantar 1,000 árboles solo se crean 3 instancias de `TipoArbol` en memoria.

**Relaciones:** El **Flyweight** se parece al **Singleton** (ambos controlan instancias), pero el Flyweight maneja múltiples instancias compartidas por clave. A menudo se usa dentro de **Composite** (las hojas pueden ser flyweights).

---

## 5. Patrones de Comportamiento (Cómo se comunican los objetos)

### 5.1 Strategy

**Definición:** Define una familia de algoritmos, los encapsula y los hace **intercambiables**. Permite que el algoritmo varíe independientemente del cliente.

**Propósito:** Eliminar condicionales largos (`if/else` o `switch`) al encapsular cada algoritmo en su propia clase.

**Cuándo usarlo:**
- Tienes muchas clases relacionadas que difieren solo en su comportamiento.
- Necesitas diferentes variantes de un algoritmo.
- Un algoritmo usa datos que el cliente no debería conocer.

**Participantes:**
- `Strategy`: interfaz común de todos los algoritmos.
- `ConcreteStrategy`: implementaciones específicas.
- `Context`: mantiene una referencia a una estrategia y la ejecuta.

**Flujo típico:**
1. El `Context` recibe una estrategia concreta.
2. El cliente solicita una operación al `Context`.
3. El `Context` delega el cálculo a la estrategia.
4. En tiempo de ejecución, se puede cambiar la estrategia.

**Analogía:** Estrategias de navegación en Google Maps. El destino es el mismo, pero puedes elegir: en coche (ruta más rápida), a pie (caminos peatonales), o transporte público (horarios de bus). El mapa (contexto) te muestra la ruta según la estrategia seleccionada.

**Ejemplo real del proyecto:** `ICalculadorImpuesto` es la estrategia con implementaciones para Costa Rica (13%), Panamá (7%), México (16%) y Zona Franca (0%). `Facturador` es el contexto que permite cambiar de estrategia en tiempo de ejecución con `CambiarEstrategia()`.

**Relaciones:** El **Strategy** es similar al **State** (ambos usan composición para cambiar comportamiento), pero el Strategy cambia algoritmos por decisión del cliente, mientras el State cambia automáticamente según el estado interno. A menudo los objetos creados por **Factory Method** son estrategias.

---

### 5.2 Observer

**Definición:** Define una dependencia **uno-a-muchos** entre objetos, de modo que cuando uno cambia de estado, todos sus dependientes son notificados automáticamente.

**Propósito:** Desacoplar el emisor de los receptores. Un sujeto puede tener cualquier número de observadores que reaccionan a eventos sin que el sujeto los conozca directamente.

**Cuándo usarlo:**
- Un cambio en un objeto requiere cambiar otros, pero no sabes de antemano cuántos ni cuáles.
- Un objeto debe notificar a otros sin hacer suposiciones sobre quiénes son.

**Participantes:**
- `Subject`: mantiene la lista de observadores y los notifica.
- `Observer`: interfaz con método de actualización.
- `ConcreteObserver`: implementa la reacción a la notificación.

**Flujo típico:**
1. Los observadores se suscriben al `Subject`.
2. El `Subject` cambia de estado.
3. El `Subject` recorre su lista y llama `Notificar()` en cada observador.
4. Cada observador reacciona según su lógica.

**Analogía:** Un canal de YouTube. Tú te suscribes (observer). Cuando el creador sube un video (cambio de estado), YouTube notifica a todos los suscriptores simultáneamente. El creador no sabe cuántos suscriptores tiene ni cómo reciben la notificación.

**Ejemplo real del proyecto:** `AgenciaNoticias` es el sujeto. Existen `SuscriptorEmail`, `SuscriptorSMS`, `SuscriptorApp` y `SuscriptorFiltrado` (que solo recibe ciertas categorías). El método `Notificar()` usa `try-catch` individual por suscriptor para evitar que un fallo de un receptor afecte a los demás.

**Relaciones:** El **Observer** es la base del patrón **MVC** (Modelo notifica a las Vistas). En C#, los `events` y `delegates` son implementaciones nativas del patrón Observer. Frecuentemente se usa con **Mediator** (el mediator puede actuar como subject centralizado).

---

### 5.3 Command

**Definición:** Encapsula una solicitud como un **objeto**, permitiendo parametrizar clientes con diferentes solicitudes, encolar operaciones y soportar operaciones reversibles (UNDO/REDO).

**Propósito:** Desacoplar el objeto que invoca una operación del objeto que la ejecuta. Convertir acciones en objetos manipulables.

**Cuándo usarlo:**
- Quieres parametrizar objetos con una acción a ejecutar.
- Necesitas encolar, ejecutar o programar operaciones.
- Necesitas soportar deshacer (undo).

**Participantes:**
- `Command`: interfaz con `Ejecutar()` y `Deshacer()`.
- `ConcreteCommand`: vincula un Receiver con una acción.
- `Receiver`: el objeto que sabe cómo realizar la operación.
- `Invoker`: solicita el comando y mantiene el historial.

**Flujo típico:**
1. El cliente crea un `ConcreteCommand` y le pasa el `Receiver`.
2. El `Invoker` recibe el comando.
3. El `Invoker` llama `Ejecutar()`.
4. El comando delega al `Receiver`.
5. Para deshacer, el `Invoker` llama `Deshacer()` usando el historial.

**Analogía:** Un control remoto de TV. Cada botón es un comando. El botón "Volumen +" no sabe cómo funciona el circuito de audio de la TV; solo envía la orden. El botón "Deshacer" puede revertir la última acción.

**Ejemplo real del proyecto:** `EditorTexto` es el receiver. Existen `ComandoInsertar`, `ComandoCortar` y `ComandoPegar`. `HistorialComandos` es el invoker con dos stacks: uno para `UNDO` y otro para `REDO`. Cada comando guarda estado suficiente para poder deshacerse.

**Relaciones:** El **Command** se combina con **Memento** para guardar el estado necesario para deshacer. También se usa con **Composite** para crear macros (comandos compuestos). El **Chain of Responsibility** puede usarse para encadenar comandos.

---

### 5.4 Template Method

**Definición:** Define el **esqueleto de un algoritmo** en un método de una clase base, dejando que las subclases implementen ciertos pasos sin cambiar la estructura.

**Propósito:** Reutilizar el flujo general de un algoritmo mientras las subclases personalizan detalles específicos.

**Cuándo usarlo:**
- Tienes un algoritmo invariante pero algunos pasos varían.
- Quieres controlar dónde las subclases pueden intervenir (hooks).
- Quieres evitar duplicación de código entre clases similares.

**Participantes:**
- `AbstractClass`: define el Template Method y los pasos abstractos/hooks.
- `ConcreteClass`: implementa los pasos específicos.

**Flujo típico:**
1. El Template Method en la clase base llama los pasos en orden fijo.
2. Algunos pasos son comunes (implementados en base).
3. Otros pasos son `abstract` o `virtual` (sobrescritos por subclases).
4. Las subclases no pueden cambiar el orden ni omitir pasos obligatorios.

**Analogía:** Una receta de cocina. El esqueleto es siempre: preparar ingredientes → cocinar → servir. Pero "cocinar" varía: para pasta hierve agua; para carne, dora en sartén. La estructura de la receta no cambia, solo los pasos concretos.

**Ejemplo real del proyecto:** `ProcesadorArchivo` define el flujo: `Validar` → `Abrir` → `Extraer` → `Transformar` → `Guardar` → `Notificar`. Las subclases `ProcesadorCSV`, `ProcesadorJSON` y `ProcesadorPDF` implementan `AbrirArchivo`, `ExtraerDatos` y `GuardarResultado`. `ValidarArchivo` y `NotificarCompletado` son hooks opcionales.

**Relaciones:** El **Template Method** usa herencia para variar comportamiento, mientras **Strategy** usa composición. Son alternativas: usa Template Method cuando controlas la jerarquía; usa Strategy cuando necesitas flexibilidad total en tiempo de ejecución.

---

### 5.5 State

**Definición:** Permite que un objeto **altere su comportamiento** cuando su estado interno cambia. Parecerá que el objeto cambia de clase.

**Propósito:** Organizar código relacionado con estados y transiciones de una máquina de estados finitos, eliminando condicionales monstruosos.

**Cuándo usarlo:**
- El comportamiento de un objeto depende de su estado, y debe cambiar en tiempo de ejecución.
- Tienes operaciones con grandes bloques condicionales que dependen del estado del objeto.

**Participantes:**
- `Context`: mantiene una referencia al estado actual.
- `State`: interfaz que encapsula el comportamiento asociado a un estado.
- `ConcreteState`: implementaciones de cada estado.

**Flujo típico:**
1. El `Context` recibe una solicitud.
2. Delega la operación al `ConcreteState` actual.
3. El estado actual decide si procesa la acción o la rechaza.
4. Si es válida, el estado transiciona al `Context` a un nuevo estado.

**Analogía:** Un semáforo. El semáforo (contexto) está siempre en una de tres estados: Verde, Amarillo o Rojo. Cuando está en Verde, permite avanzar y transiciona a Amarillo. Cuando está en Rojo, rechaza avanzar. El semáforo no decide la lógica; el estado actual sí.

**Ejemplo real del proyecto:** `Pedido` es el contexto con estados: `EstadoNuevo`, `EstadoPagado`, `EstadoEnviado`, `EstadoEntregado` y `EstadoCancelado`. Cada estado implementa `Pagar()`, `Enviar()`, `Entregar()` y `Cancelar()`. El estado solo puede cambiarse a través de métodos del contexto (encapsulación protegida con `CambiarEstado()`). Los estados son singletons estáticos para evitar crear instancias innecesarias.

**Relaciones:** El **State** es estructuralmente similar al **Strategy**, pero las transiciones entre estados suelen ser controladas por los propios estados (circular), mientras que en Strategy el cliente elige la estrategia. El **State** puede usar **Singleton** para estados sin estado propio.

---

### 5.6 Mediator

**Definición:** Define un objeto que **encapsula cómo interactúa un conjunto de objetos**. Promueve el acoplamiento débil al evitar que los objetos se referencien explícitamente entre sí.

**Propósito:** Centralizar la comunicación compleja entre múltiples objetos en un solo lugar, evitando una red caótica de dependencias.

**Cuándo usarlo:**
- Un conjunto de objetos se comunica de formas bien definidas pero complejas.
- Reutilizar un objeto es difícil porque referencia a muchos otros.
- Un comportamiento distribuido entre varias clases debería ser personalizable sin muchas subclases.

**Participantes:**
- `Mediator`: interfaz para la comunicación.
- `ConcreteMediator`: coordina los colegas.
- `Colleague`: objetos que se comunican a través del mediator.

**Flujo típico:**
1. Los colegas se registran en el mediator.
2. Un colega quiere enviar un mensaje; en vez de contactar a otro directamente, le dice al mediator.
3. El mediator decide a quién reenviar el mensaje.

**Analogía:** Un chat grupal de WhatsApp. Ana no envía un mensaje directamente a Carlos y a María; lo manda al grupo. WhatsApp (mediator) se encarga de replicarlo a todos los miembros.

**Ejemplo real del proyecto:** `SalaChatGrupal` implementa `ISalaChat`. `Usuario` es el colega. Cuando `ana.Enviar("Hola")`, el usuario delega a `_sala.EnviarMensaje()`. La sala itera sobre todos los usuarios registrados y llama `Recibir()` excepto al remitente.

**Relaciones:** El **Mediator** a veces implementa el patrón **Observer** (la sala observa a los usuarios). A menudo reemplaza múltiples **Observers** bidireccionales. Es útil en arquitecturas MVC donde el Controller actúa como mediator entre View y Model.

---

### 5.7 Memento

**Definición:** Permite capturar y externalizar el estado interno de un objeto **sin violar su encapsulamiento**, de modo que el objeto pueda ser restaurado a ese estado más tarde.

**Propósito:** Implementar checkpoints, undo profundo, o snapshots de objetos complejos.

**Cuándo usarlo:**
- Necesitas guardar snapshots del estado de un objeto para poder restaurarlo.
- El estado directo del objeto está encapsulado y no debe exponerse.

**Participantes:**
- `Originator`: el objeto cuyo estado se guarda.
- `Memento`: almacena el estado del Originator (inmutable idealmente).
- `Caretaker`: guarda los mementos pero NUNCA los modifica ni inspecciona.

**Flujo típico:**
1. El `Caretaker` pide un memento al `Originator`.
2. El `Originator` crea un objeto `Memento` con su estado actual.
3. El `Caretaker` guarda el memento en una pila o lista.
4. Cuando se necesita restaurar, el `Caretaker` devuelve el memento al `Originator`.

**Analogía:** Los checkpoints de un videojuego. En cualquier momento presionas "Guardar". El juego (Originator) empaqueta tu posición, vidas y puntaje en un archivo de guardado (Memento). Tú (Caretaker) guardas ese archivo en tu disco. Más tarde, eliges "Cargar" y el juego restaura exactamente ese estado.

**Ejemplo real del proyecto:** `Partida` es el originator. `EstadoJuego` es el memento, implementado como `record` inmutable con `DateTimeOffset.UtcNow`. `GestorGuardados` es el caretaker con un `Stack<EstadoJuego>`. Se demuestra guardar, avanzar, recibir daño, y restaurar checkpoints.

**Relaciones:** El **Memento** se usa frecuentemente junto al **Command** (para implementar undo). El **Caretaker** puede ser un **Singleton**.

---

### 5.8 Chain of Responsibility

**Definición:** Permite pasar solicitudes a lo largo de una **cadena de manejadores**. Cada manejador decide si procesa la solicitud o la pasa al siguiente eslabón.

**Propósito:** Desacoplar al emisor de una solicitud de su receptor, dando a múltiples objetos la oportunidad de manejarla.

**Cuándo usarlo:**
- Más de un objeto puede manejar una solicitud, y el manejador no se conoce de antemano.
- Quieres emitir una solicitud a uno de varios objetos sin especificar el receptor explícitamente.
- El conjunto de objetos que pueden manejar una solicitud debe ser dinámico.

**Participantes:**
- `Handler`: define la interfaz para manejar solicitudes y el enlace al siguiente.
- `ConcreteHandler`: maneja las solicitudes que le corresponden; pasa las demás al siguiente.
- `Client`: inicia la solicitud a la cadena.

**Flujo típico:**
1. El cliente pasa la solicitud al primer handler.
2. El handler verifica si puede procesarla.
3. Si sí, la procesa. Si no, la pasa al siguiente.
4. Si nadie la procesa, se reporta como no manejada.

**Analogía:** Un hospital de emergencias. Llegas con una torcedura (leve): el médico general te atiende. Llegas con un infarto (crítico): el general te deriva al cardiólogo, quien te deriva al quirófano. Cada nivel decide si puede o pasa al siguiente.

**Ejemplo real del proyecto:** `SoporteHandler` es la clase abstracta con `EstablecerSiguiente()`. Existen `SoporteNivel1`, `SoporteNivel2`, `SoporteNivel3` y `DirectorSoporte`. `TicketSoporte` es un `record` inmutable que usa el enum `SeveridadTicket` (Leve, Moderada, Critica, Empresarial). Cada nivel maneja hasta cierta severidad.

**Relaciones:** El **Chain of Responsibility** se parece al **Decorator** (ambos pasan solicitudes a lo largo de una cadena), pero el Decorator siempre delega; el Chain decide si procesa o delega. En ASP.NET Core, los middlewares forman una cadena de responsabilidad.

---

## 6. Tablas comparativas

### Por propósito principal

| Si necesitas... | Usa... |
|-----------------|--------|
| Una única instancia global | **Singleton** |
| Delegar creación a subclases | **Factory Method** |
| Familias de objetos compatibles | **Abstract Factory** |
| Construir objetos paso a paso | **Builder** |
| Clonar objetos existentes | **Prototype** |
| Adaptar interfaces incompatibles | **Adapter** |
| Añadir funcionalidad dinámicamente | **Decorator** |
| Simplificar un subsistema complejo | **Facade** |
| Controlar acceso a un objeto | **Proxy** |
| Tratar objetos individuales y compuestos igual | **Composite** |
| Separar abstracción de implementación | **Bridge** |
| Compartir objetos masivos | **Flyweight** |
| Cambiar algoritmos en tiempo de ejecución | **Strategy** |
| Notificar a múltiples objetos de eventos | **Observer** |
| Encapsular acciones con undo/redo | **Command** |
| Reutilizar esqueleto de algoritmo | **Template Method** |
| Modelar una máquina de estados | **State** |
| Centralizar comunicación entre objetos | **Mediator** |
| Guardar/restaurar estado | **Memento** |
| Pasar solicitudes por una cadena | **Chain of Responsibility** |

### Estrategia vs Template Method vs State

Todos tres cambian comportamiento, pero de forma diferente:

| Patrón | ¿Cómo cambia? | ¿Quién decide? | Relación con herencia |
|--------|---------------|----------------|----------------------|
| **Strategy** | Composición (tiene una estrategia) | El cliente elige | Evita herencia |
| **Template Method** | Herencia (subclases sobrescriben pasos) | La clase base define flujo | Usa herencia |
| **State** | El objeto cambia su estado interno | Los propios estados transicionan | Usa composición |

### Proxy vs Adapter vs Decorator

Todos envuelven objetos, pero con intenciones distintas:

| Patrón | ¿Cambia interfaz? | ¿Misma interfaz? | Propósito |
|--------|-------------------|------------------|-----------|
| **Proxy** | No | Sí | Control de acceso, caché, lazy loading |
| **Adapter** | Sí | No (traduce) | Compatibilidad entre interfaces |
| **Decorator** | No | Sí | Añadir responsabilidades dinámicamente |

### Composite vs Decorator vs Flyweight

Todos manejan estructuras de objetos, pero:

| Patrón | Estructura | Número de hijos | Propósito |
|--------|------------|-----------------|-----------|
| **Composite** | Árbol (parte-todo) | Múltiples | Uniformidad hoja/compuesto |
| **Decorator** | Cadena lineal | Uno | Añadir funcionalidad por capas |
| **Flyweight** | Grafo compartido | Referencias compartidas | Ahorrar memoria |

---

## 7. Glosario

| Término | Definición |
|---------|------------|
| **Acoplamiento** | Grado de dependencia entre módulos. Bajo acoplamiento es deseable. |
| **Composición** | Relación "tiene un" (has-a). Un objeto contiene referencias a otros. |
| **Encapsulamiento** | Ocultar el estado interno y exponer solo operaciones controladas. |
| **Herencia** | Relación "es un" (is-a). Una clase deriva de otra. |
| **Hook** | Método opcional en una clase base que las subclases pueden sobrescribir. |
| **Intrínseco** | Estado compartido entre instancias (en Flyweight). |
| **Extrínseco** | Estado único proporcionado por el contexto/cliente (en Flyweight). |
| **Shallow copy** | Copia superficial: copia valores primitivos pero comparte referencias. |
| **Deep copy** | Copia profunda: duplica todo, incluyendo objetos referenciados. |
| **Subject** | En Observer, el objeto observable. En Proxy, la interfaz común. |
| **Context** | En Strategy y State, el objeto que mantiene la referencia al comportamiento actual. |
| **Originator** | En Memento, el objeto cuyo estado se captura. |
| **Caretaker** | En Memento, quien guarda mementos sin modificarlos. |
| **Concrete** | Prefijo que indica una implementación específica de una interfaz o clase abstracta. |

---

## 8. Preguntas frecuentes (FAQs)

**¿Cuál es la diferencia entre Factory Method y Abstract Factory?**
> Factory Method crea **un** tipo de producto y delega a subclases. Abstract Factory crea **familias** de productos relacionados (varios tipos) que deben ser compatibles entre sí.

**¿Strategy y State son lo mismo?**
> No. Ambos usan composición para cambiar comportamiento, pero en **Strategy** el cliente elige explícitamente la estrategia. En **State**, las transiciones ocurren automáticamente según las reglas del estado actual.

**¿Cuándo uso Proxy y cuándo Decorator?**
> Usa **Proxy** cuando quieres controlar el acceso (seguridad, caché, remoto) manteniendo la misma interfaz. Usa **Decorator** cuando quieres añadir responsabilidades o comportamientos nuevos.

**¿Bridge y Adapter son iguales?**
> No. El **Adapter** arregla incompatibilidades entre sistemas ya existentes (post-hoc). El **Bridge** desacopla deliberadamente una abstracción de su implementación desde el diseño (pre-planificado).

**¿Puedo combinar patrones?**
> Sí, y de hecho es común. Por ejemplo: un **Abstract Factory** que usa **Singleton**, creando productos que son **Strategies** manejados por un **Mediator** que notifica cambios vía **Observer**. Los patrones no son excluyentes; son bloques de construcción.

**¿Por qué no hay Iterator en este proyecto?**
> C# ya tiene `IEnumerable<T>` e `IEnumerator<T>`, que son implementaciones nativas y robustas del patrón Iterator. Agregar uno manual sería redundante.

**¿Es Singleton un anti-patrón?**
> Depende. En aplicaciones modernas con inyección de dependencias (DI), el Singleton como patrón de código ha caído en desuso porque el contenedor DI gestiona el ciclo de vida. Sin embargo, entender cómo funciona es valioso para comprender el control de instancias.

**¿Qué es un "record" en C# y por qué se usa en algunos patrones?**
> Un `record` es un tipo de referencia (o valor con `record struct`) inmutable por convención, con value equality, `ToString` automático y sintaxis `with` para crear copias modificadas. Es ideal para objetos de datos puros como `TicketSoporte`, `EstadoJuego` o `DatosFiscales`.

---

## 9. Anti-patrones relacionados

| Anti-patrón | Descripción | Cómo evitarlo |
|-------------|-------------|---------------|
| **God Object** | Una clase que sabe/hace demasiado. Usa **Facade** para delegar. |
| **Spaghetti Code** | Flujo de control caótico. Usa **State** o **Strategy** para organizar. |
| **Copy-Paste Programming** | Duplicación de lógica. Usa **Template Method** para reutilizar esqueletos. |
| **Golden Hammer** | Usar un patrón favorito para todo. Elegir el patrón adecuado para el problema. |
| **Anemic Domain Model** | Clases sin comportamiento, solo datos. Los patrones de comportamiento (State, Strategy) ayudan a enriquecer el modelo. |

---

## 10. Guía de estudio recomendada

### Para principiantes
1. Empieza por **Singleton** (simple) → **Factory Method** → **Strategy**.
2. Continúa con **Observer** (muy visual) → **Decorator** (fácil de entender con analogías).
3. Luego **Adapter** y **Facade** (útiles y comunes).

### Para nivel intermedio
4. Estudia **State** y **Command** (máquinas de estados y undo son fundamentales).
5. Aprende **Template Method** vs **Strategy** (la eterna discusión herencia vs composición).
6. Domina **Proxy** y **Composite**.

### Para nivel avanzado
7. Profundiza en **Abstract Factory**, **Builder** y **Prototype**.
8. Estudia **Bridge**, **Flyweight**, **Mediator**, **Memento** y **Chain of Responsibility**.
9. Comienza a identificar combinaciones de patrones en frameworks reales (ASP.NET Core usa Observer, Chain of Responsibility, Factory Method, Singleton, Proxy, Decorator...).

---

> Documento generado para consumo por IA / NotebookLM. Contiene 20 patrones GoF implementados en C# 10 con casos de uso en español.
