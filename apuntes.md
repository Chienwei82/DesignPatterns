# Patrones de Diseno — .NET 10 - Apuntes

Proyecto educativo instalado en **devbox** (192.168.64.199)

## Ubicacion

```
~/mycode/DesignPatterns/
```

## Estructura del proyecto

```
~/mycode/DesignPatterns/
├── DesignPatterns.sln
├── DesignPatterns.csproj
├── Program.cs                    ← Menu interactivo
├── Creational/
│   ├── Singleton.cs              ← 1 instancia global
│   ├── FactoryMethod.cs          ← Creacion delegada a subclases
│   ├── AbstractFactory.cs        ← Familias de objetos relacionados
│   ├── Builder.cs                ← Objetos complejos paso a paso
│   └── Prototype.cs              ← Clonacion superficial vs profunda
├── Structural/
│   ├── Adapter.cs                ← Interfaces incompatibles → compatibles
│   ├── Decorator.cs              ← Anyadir funcionalidad dinamicamente
│   ├── Facade.cs                 ← Fachada para subsistemas complejos
│   ├── Proxy.cs                  ← Control de acceso / cache
│   └── Composite.cs              ← Estructuras arbol (organigrama)
└── Behavioral/
    ├── Strategy.cs               ← Algoritmos intercambiables (impuestos)
    ├── Observer.cs               ← Suscripcion / notificaciones
    ├── Command.cs                ← Comandos con UNDO/REDO
    ├── TemplateMethod.cs         ← Esqueleto de algoritmo
    └── State.cs                  ← Maquina de estados (pedidos)
```

## Como usar

```bash
cd ~/mycode/DesignPatterns
dotnet run
```

Sale un menu interactivo donde eliges que patron ver (1-15).

Para ejecutar un patron directo sin menu:

```bash
echo 1 | dotnet run   # Singleton
echo 7 | dotnet run   # Decorator
echo 13 | dotnet run  # Command
```

## Resumen de patrones

### Creacionales (como se crean los objetos)

| # | Patron | Que resuelve |
|---|--------|-------------|
| 1 | Singleton | Una sola instancia global (configuracion, logger) |
| 2 | Factory Method | Delegar creacion a subclases (notificaciones multi-canal) |
| 3 | Abstract Factory | Familias de objetos compatibles (temas claro/oscuro) |
| 4 | Builder | Objetos complejos paso a paso (pedidos, consultas SQL) |
| 5 | Prototype | Clonar objetos (facturas, configuraciones) |

### Estructurales (como se componen las clases)

| # | Patron | Que resuelve |
|---|--------|-------------|
| 6 | Adapter | Interfaces incompatibles (pasarelas de pago) |
| 7 | Decorator | Anyadir funcionalidad dinamicamente (cafe con extras) |
| 8 | Facade | Simplificar subsistemas complejos (tienda online) |
| 9 | Proxy | Control de acceso / cache (permisos, lazy loading) |
| 10 | Composite | Estructuras arbol parte-todo (organigrama) |

### Comportamiento (como se comunican los objetos)

| # | Patron | Que resuelve |
|---|--------|-------------|
| 11 | Strategy | Algoritmos intercambiables (impuestos por pais) |
| 12 | Observer | Suscripcion 1 a muchos (noticias, eventos) |
| 13 | Command | Comandos como objetos con UNDO/REDO (editor texto) |
| 14 | Template Method | Esqueleto de algoritmo (procesar CSV/JSON/PDF) |
| 15 | State | Maquina de estados (ciclo de vida de pedidos) |

## Ambiente

- OS: Ubuntu 24.04
- Dotnet: 10.0.107
- Code-server: 4.109.2
- SSH key instalada desde homelab
