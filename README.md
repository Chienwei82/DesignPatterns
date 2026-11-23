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

| # | Patrón | Categoría | Intención en una frase | Analogía (cultura popular) |
|---|--------|-----------|------------------------|----------|
| 1 | Singleton | Creacional | Una sola instancia global, punto de acceso único | El Anillo Único (ESDLA): solo existe uno |
| 2 | Factory Method | Creacional | Delegar la creación de objetos a subclases | Pokémon: cada laboratorio entrega su inicial |
| 3 | Abstract Factory | Creacional | Crear familias de objetos compatibles | Casas de Hogwarts: estandarte, mascota y sala a juego |
| 4 | Builder | Creacional | Construcción paso a paso de objetos complejos | Forjar un sable de luz (Star Wars) pieza por pieza |
| 5 | Prototype | Creacional | Clonar objetos existentes en vez de crear desde cero | Los soldados clon de Star Wars |
| 6 | Adapter | Estructural | Hacer compatibles interfaces incompatibles | Emulador que juega ROMs de consolas retro |
| 7 | Decorator | Estructural | Añadir responsabilidades dinámicamente | Encantar un arma de RPG con runas apiladas |
| 8 | Facade | Estructural | Interfaz simple sobre un subsistema complejo | Protocolo Vengadores: una orden, 4 subsistemas |
| 9 | Proxy | Estructural | Controlar el acceso a un objeto | Archivos clasificados de S.H.I.E.L.D.: caché + credencial |
| 10 | Composite | Estructural | Tratar individuales y compuestos de forma uniforme | Ejércitos de Poniente (GoT): soldado vs legión |
| 11 | Bridge | Estructural | Separar abstracción de implementación | RPG: armas × elementos mágicos |
| 12 | Flyweight | Estructural | Compartir objetos en gran cantidad | Ejército de droides (Star Wars): 1000 unidades, 3 modelos |
| 13 | Strategy | Comportamiento | Algoritmos intercambiables en tiempo de ejecución | Ryu cambia su estilo de pelea según el rival |
| 14 | Observer | Comportamiento | Notificar a suscriptores cuando cambia el estado | La Batiseñal (DC): todos los aliados reaccionan |
| 15 | Command | Comportamiento | Encapsular solicitudes como objetos (undo/redo) | Rewind de videojuego: rebobinar cada acción |
| 16 | Template Method | Comportamiento | Esqueleto fijo de algoritmo, pasos variables | El Viaje del Héroe: Luke y Frodo, mismo monomito |
| 17 | State | Comportamiento | Cambiar comportamiento según el estado interno | Evolución Pokémon: Charmander → Charizard |
| 18 | Mediator | Comportamiento | Comunicación centralizada mediante intermediario | Party de raid (WoW): todos hablan por el chat |
| 19 | Memento | Comportamiento | Guardar y restaurar estado sin romper encapsulamiento | El Pensadero (Harry Potter): revivir recuerdos |
| 20 | Chain of Responsibility | Comportamiento | Pasar una solicitud por una cadena de manejadores | Escalada de amenazas: Guardián → Héroe → Liga |

---

## Mapa visual de los patrones

```mermaid
graph TD
    %% Estilos por familia
    classDef creacional fill:#f5e6e0,stroke:#c45a3d,stroke-width:2px,color:#1d1b18;
    classDef estructural fill:#e0eaf5,stroke:#3d7ac4,stroke-width:2px,color:#1d1b18;
    classDef comportamiento fill:#dff0e2,stroke:#3d8b4a,stroke-width:2px,color:#1d1b18;

    GOF[Patrones GoF<br/>20 de 23 implementados]

    GOF --> C[Creacionales<br/>5 patrones]
    GOF --> E[Estructurales<br/>7 patrones]
    GOF --> B[Comportamiento<br/>8 patrones]

    C --> S1[1 Singleton]
    C --> S2[2 Factory Method]
    C --> S3[3 Abstract Factory]
    C --> S4[4 Builder]
    C --> S5[5 Prototype]

    E --> S6[6 Adapter]
    E --> S7[7 Decorator]
    E --> S8[8 Facade]
    E --> S9[9 Proxy]
    E --> S10[10 Composite]
    E --> S11[11 Bridge]
    E --> S12[12 Flyweight]

    B --> S13[13 Strategy]
    B --> S14[14 Observer]
    B --> S15[15 Command]
    B --> S16[16 Template Method]
    B --> S17[17 State]
    B --> S18[18 Mediator]
    B --> S19[19 Memento]
    B --> S20[20 Chain of Responsibility]

    class S1,S2,S3,S4,S5 creacional;
    class S6,S7,S8,S9,S10,S11,S12 estructural;
    class S13,S14,S15,S16,S17,S18,S19,S20 comportamiento;
```

## ¿Qué patrón elijo? — Diagrama de decisión

```mermaid
flowchart TD
    A[¿Qué necesitas hacer?] --> B{Crear objetos}
    A --> C{Componer clases u objetos}
    A --> D{Gestionar comunicación / comportamiento}

    %% Creacionales
    B --> B1{¿Una única instancia global?}
    B1 -->|Sí| E1[Singleton]
    B1 -->|No| B2{¿Familias de productos compatibles?}
    B2 -->|Sí| E2[Abstract Factory]
    B2 -->|No| B3{¿Construcción compleja paso a paso?}
    B3 -->|Sí| E3[Builder]
    B3 -->|No| B4{¿Clonar un objeto existente?}
    B4 -->|Sí| E4[Prototype]
    B4 -->|No| E5[Factory Method]

    %% Estructurales
    C --> C1{¿Interfaces incompatibles?}
    C1 -->|Sí| E6[Adapter]
    C1 -->|No| C2{¿Añadir responsabilidades dinámicamente?}
    C2 -->|Sí| E7[Decorator]
    C2 -->|No| C3{¿Interfaz simple sobre subsistema complejo?}
    C3 -->|Sí| E8[Facade]
    C3 -->|No| C4{¿Controlar acceso a un objeto?}
    C4 -->|Sí| E9[Proxy]
    C4 -->|No| C5{¿Tratar simple y compuesto uniformemente?}
    C5 -->|Sí| E10[Composite]
    C5 -->|No| C6{¿Separar abstracción e implementación?}
    C6 -->|Sí| E11[Bridge]
    C6 -->|No| E12[Flyweight]

    %% Comportamiento
    D --> D1{¿Intercambiar algoritmos en tiempo de ejecución?}
    D1 -->|Sí| E13[Strategy]
    D1 -->|No| D2{¿Notificar a varios suscriptores?}
    D2 -->|Sí| E14[Observer]
    D2 -->|No| D3{¿Encapsular solicitudes / undo-redo?}
    D3 -->|Sí| E15[Command]
    D3 -->|No| D4{¿Esqueleto fijo con pasos variables?}
    D4 -->|Sí| E16[Template Method]
    D4 -->|No| D5{¿Cambiar comportamiento según estado?}
    D5 -->|Sí| E17[State]
    D5 -->|No| D6{¿Comunicación centralizada vía intermediario?}
    D6 -->|Sí| E18[Mediator]
    D6 -->|No| D7{¿Guardar y restaurar estado sin romper encapsulamiento?}
    D7 -->|Sí| E19[Memento]
    D7 -->|No| E20[Chain of Responsibility]

    style A fill:#f9f,stroke:#333,stroke-width:2px
    style E1 fill:#f5e6e0,stroke:#c45a3d
    style E2 fill:#f5e6e0,stroke:#c45a3d
    style E3 fill:#f5e6e0,stroke:#c45a3d
    style E4 fill:#f5e6e0,stroke:#c45a3d
    style E5 fill:#f5e6e0,stroke:#c45a3d
```

## Diagramas de clases representativos

### Singleton — clase única con acceso global

```mermaid
classDiagram
    class AnilloUnico {
        - static AnilloUnico _instancia
        - Lazy~AnilloUnico~ _lazy
        + static AnilloUnico Instancia
        - AnilloUnico()
        + string Portador
        + void Reclamar(nombre)
        + void UsarInvisibilidad()
        + void Mostrar()
    }
    class Frodo {
        + void Reclama()
    }
    class Gandalf {
        + void Consulta()
    }
    Frodo --> AnilloUnico : usa la única instancia
    Gandalf --> AnilloUnico : usa la única instancia
```

### Strategy — algoritmos intercambiables

```mermaid
classDiagram
    class IEstiloCombate {
        <<interface>>
        + string DescribirAtaque(rival)
    }
    class EstiloAgresivo {
        + string DescribirAtaque(rival)
    }
    class EstiloDefensivo {
        + string DescribirAtaque(rival)
    }
    class EstiloEquilibrado {
        + string DescribirAtaque(rival)
    }
    class Luchador {
        - IEstiloCombate _estilo
        + CambiarEstilo(nuevo)
        + Pelear(rival)
    }
    IEstiloCombate <|.. EstiloAgresivo
    IEstiloCombate <|.. EstiloDefensivo
    IEstiloCombate <|.. EstiloEquilibrado
    Luchador o-- IEstiloCombate : usa
```

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
