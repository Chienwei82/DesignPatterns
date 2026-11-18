# DesignPatterns — Patrones de Diseño GoF en C#

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download)
[![C# 14](https://img.shields.io/badge/C%23-14-68217A?logo=csharp)](https://dotnet.microsoft.com/languages)
[![Español](https://img.shields.io/badge/idioma-español-ff69b4)](https://github.com/Chienwei82/DesignPatterns)
[![Educativo](https://img.shields.io/badge/enfoque-educativo-blue)](https://github.com/Chienwei82/DesignPatterns)

> **⚠️ Proyecto de aprendizaje personal**
>
> Este repositorio es una **práctica de estudio personal**. No pretende ser una referencia de producción ni un paquete reutilizable; su único objetivo es consolidar conceptos de los patrones de diseño clásicos (Gang of Four) mediante implementaciones sencillas en C#.

---

## ¿Qué es esto?

Una aplicación de consola interactiva en **.NET 10** que demuestra **20 de los 23 patrones de diseño GoF** (Gang of Four), organizados por categoría. Cada patrón incluye su implementación, un caso de uso ilustrativo y documentación interna con **intención, analogía, cuándo usar y cuándo NO usar** —todo en español.

| Categoría | Patrones |
|-----------|----------|
| **Creacionales** (cómo se crean los objetos) | Singleton, Factory Method, Abstract Factory, Builder, Prototype |
| **Estructurales** (cómo se componen las clases) | Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight |
| **De Comportamiento** (cómo se comunican los objetos) | Strategy, Observer, Command, Template Method, State, Mediator, Memento, Chain of Responsibility |

> **Nota:** los 23 patrones GoF originales incluyen también **Interpreter**, **Iterator** y **Visitor**. Se omiten a propósito: Iterator y Visitor ya los cubre C# (`IEnumerable`/LINQ), y Interpreter es raro en la práctica.

---

## Tabla rápida de patrones

| # | Patrón | Categoría | Intención en una frase | Analogía |
|---|--------|-----------|------------------------|----------|
| 1 | Singleton | Creacional | Una sola instancia global, punto de acceso único | El presidente del país: solo hay uno |
| 2 | Factory Method | Creacional | Delegar la creación de objetos a subclases | Franquicia de muebles: cada taller elige el material |
| 3 | Abstract Factory | Creacional | Crear familias de objetos compatibles | Menú completo por cocina: todo combina |
| 4 | Builder | Creacional | Construcción paso a paso de objetos complejos | Armar una hamburguesa por pasos |
| 5 | Prototype | Creacional | Clonar objetos existentes en vez de crear desde cero | Fotocopia de un formulario prellenado |
| 6 | Adapter | Estructural | Hacer compatibles interfaces incompatibles | Adaptador de enchufe europeo → americano |
| 7 | Decorator | Estructural | Añadir responsabilidades dinámicamente | Vestirse en capas: camiseta, suéter, abrigo |
| 8 | Facade | Estructural | Interfaz simple sobre un subsistema complejo | Recepción de un hotel: un solo punto para todo |
| 9 | Proxy | Estructural | Controlar el acceso a un objeto | Tarjeta de crédito: intermediario antes del dinero |
| 10 | Composite | Estructural | Tratar individuales y compuestos de forma uniforme | Carpeta con archivos y subcarpetas |
| 11 | Bridge | Estructural | Separar abstracción de implementación | Receta × método de cocción: dos perillas independientes |
| 12 | Flyweight | Estructural | Compartir objetos en gran cantidad | Un solo molde de bebida para mil comandas |
| 13 | Strategy | Comportamiento | Algoritmos intercambiables en tiempo de ejecución | Auto, bici o bus: mismo destino, medio distinto |
| 14 | Observer | Comportamiento | Notificar a suscriptores cuando cambia el estado | Canal de YouTube: te suscribes y recibes videos |
| 15 | Command | Comportamiento | Encapsular solicitudes como objetos (undo/redo) | Pedido en restaurante: se anota, se ejecuta, se anula |
| 16 | Template Method | Comportamiento | Esqueleto fijo de algoritmo, pasos variables | Receta de pan: pasos fijos, harina variable |
| 17 | State | Comportamiento | Cambiar comportamiento según el estado interno | Botón del teléfono: actúa distinto según el estado |
| 18 | Mediator | Comportamiento | Comunicación centralizada mediante intermediario | Torre de control: nadie habla con nadie directamente |
| 19 | Memento | Comportamiento | Guardar y restaurar estado sin romper encapsulamiento | Guardar partida en un videojuego |
| 20 | Chain of Responsibility | Comportamiento | Pasar una solicitud por una cadena de manejadores | Queja que escala: mesero → jefe → gerente → dueño |

---

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

---

## Ejecución

### Menú interactivo
```bash
dotnet run
```

### Ejecutar un patrón específico por número
```bash
echo 13 | dotnet run
```

> Los números `1`–`20` corresponden al orden del menú. Usa `Q` o `SALIR` para salir del menú interactivo.

---

## Estructura del proyecto

```
DesignPatterns/
├── Program.cs                         # Menú interactivo (opciones 1-20 + salir)
├── apuntes.md                         # Apuntes: intención, analogía, cuándo usar/no
├── notebooklm-patrones-diseno.md      # Guía GoF extendida para NotebookLM
├── README.md                          # Este archivo
├── DesignPatterns.csproj              # Proyecto .NET 10 (standalone, sin .sln)
├── Creational/
│   ├── Singleton.cs
│   ├── FactoryMethod.cs
│   ├── AbstractFactory.cs
│   ├── Builder.cs
│   └── Prototype.cs
├── Structural/
│   ├── Adapter.cs
│   ├── Decorator.cs
│   ├── Facade.cs
│   ├── Proxy.cs
│   ├── Composite.cs
│   ├── Bridge.cs
│   └── Flyweight.cs
└── Behavioral/
    ├── Strategy.cs
    ├── Observer.cs
    ├── Command.cs
    ├── TemplateMethod.cs
    ├── State.cs
    ├── Mediator.cs
    ├── Memento.cs
    └── ChainOfResponsibility.cs
```

---

## Convenciones de código

- Todo el código, comentarios y salida por consola están en **español**.
- Se usan **namespaces con scope de archivo** (`namespace X;`).
- Cada archivo contiene la implementación del patrón más una clase `static` `{Patrón}Demo` con un método `Run()` que ejecuta el ejemplo.
- Cada archivo incluye un bloque de documentación al inicio con: **Intención**, **Analogía**, **Cuándo usarlo**, **Cuándo NO usarlo** y **Uso real**.
- Claridad y simplicidad sobre rendimiento o preocupaciones de producción (enfoque educativo).

---

## Documentación adicional

| Archivo | Contenido |
|---------|-----------|
| `apuntes.md` | Apuntes de estudio por patrón: intención, analogía, cuándo usar / cuándo NO usar |
| `notebooklm-patrones-diseno.md` | Guía GoF extendida optimizada para NotebookLM (definición, participantes, relaciones, FAQs, anti-patrones) |

---

## Nota final

Si encuentras algo que se pueda mejorar, ¡toda retroalimentación es bienvenida! Pero recuerda: esto es material de estudio, no una biblioteca para usar en producción.
