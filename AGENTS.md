# Agents.md — DesignPatterns

## Repo

Single .NET 10 console app (`net10.0`) demonstrating 15 GoF design patterns. No tests, no CI, no linters.

## Commands

```bash
dotnet run          # interactive menu (stdin)
echo 13 | dotnet run  # run specific pattern by number
```

## Structure

- `Program.cs` — menu, maps `"1"–"15"` to `{Category}Demo.Run()`
- `Creational/` — Singleton, FactoryMethod, AbstractFactory, Builder, Prototype
- `Structural/` — Adapter, Decorator, Facade, Proxy, Composite
- `Behavioral/` — Strategy, Observer, Command, TemplateMethod, State
- `apuntes.md` — full reference with real-world use cases

## Conventions

- All code/output is in **Spanish**
- File-scoped namespaces (`namespace DesignPatterns.Creational;`)
- Each pattern file contains the implementation + a `static class *Demo { public static void Run() }`
- Console uses UTF-8 encoding (`Console.OutputEncoding = Encoding.UTF8`)
- No `.sln` file — project is standalone `.csproj`
- No tests, no formatter config, no build scripts
