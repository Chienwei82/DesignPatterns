# DesignPatterns — Patrones de Diseño GoF en C#

> **⚠️ Proyecto de aprendizaje personal**
>
> Este repositorio es una **práctica de estudio personal**. No pretende ser una referencia de producción ni un paquete reutilizable; su único objetivo es consolidar conceptos de los patrones de diseño clásicos (Gang of Four) mediante implementaciones sencillas en C#.

---

## ¿Qué es esto?

Una aplicación de consola interactiva en **.NET 10** que demuestra los **15 patrones de diseño GoF** organizados por categoría:

| Categoría | Patrones |
|-----------|----------|
| **Creacionales** | Singleton, Factory Method, Abstract Factory, Builder, Prototype |
| **Estructurales** | Adapter, Decorator, Facade, Proxy, Composite |
| **De Comportamiento** | Strategy, Observer, Command, Template Method, State |

Cada patrón incluye una implementación mínima y un caso de uso ilustrativo (siempre en español).

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

> Los números `1`–`15` corresponden al orden del menú.

---

## Estructura del proyecto

```
DesignPatterns/
├── Program.cs              # Menú interactivo
├── apuntes.md              # Resumen con casos de uso reales
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
│   └── Composite.cs
└── Behavioral/
    ├── Strategy.cs
    ├── Observer.cs
    ├── Command.cs
    ├── TemplateMethod.cs
    └── State.cs
```

---

## Convenciones

- Todo el código, comentarios y salida por consola están en **español**.
- Se usan **namespaces con scope de archivo** (`namespace X;`).
- Cada archivo contiene la implementación del patrón más una clase `static` `{Patrón}Demo` con un método `Run()` que ejecuta el ejemplo.

---

## Nota final

Si encuentras algo que se pueda mejorar, ¡toda retroalimentación es bienvenida! Pero recuerda: esto es material de estudio, no una biblioteca para usar en producción.
