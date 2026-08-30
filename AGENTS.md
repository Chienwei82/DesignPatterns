# Agents.md — DesignPatterns

## Repo

Single .NET 10 console app (`net10.0`) demonstrating 20 GoF design patterns. No tests, no CI, no linters.

## Commands

```bash
dotnet run            # interactive menu (stdin)
echo 13 | dotnet run  # run specific pattern by number (1-20)
```

## Structure

- `Program.cs` — menu, maps `"1"–"20"` to `{Category}Demo.Run()`
- `Creational/` — Singleton, FactoryMethod, AbstractFactory, Builder, Prototype
- `Structural/` — Adapter, Decorator, Facade, Proxy, Composite, Bridge, Flyweight
- `Behavioral/` — Strategy, Observer, Command, TemplateMethod, State, Mediator, Memento, ChainOfResponsibility
- `apuntes.md` — study notes: intention, analogy, when to use / when not, per pattern
- `notebooklm-patrones-diseno.md` — full GoF reference optimized for AI ingestion (NotebookLM)

## Conventions

- All code/output is in **Spanish**
- File-scoped namespaces (`namespace DesignPatterns.Creational;`)
- Each pattern file contains the implementation + a `static class *Demo { public static void Run() }`
- Console uses UTF-8 encoding (`Console.OutputEncoding = Encoding.UTF8`)
- No `.sln` file — project is standalone `.csproj`
- No tests, no formatter config, no build scripts
- Educational focus: clarity and simplicity over performance or production concerns

## Not implemented (intentionally)

Interpreter, Iterator, Visitor — specialized or already covered by C# (`IEnumerable`/LINQ, expression trees).
