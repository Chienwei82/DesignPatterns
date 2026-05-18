using DesignPatterns.Creational;
using DesignPatterns.Structural;
using DesignPatterns.Behavioral;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var patterns = new Dictionary<string, (string Category, string Name, Action Demo)>
{
    ["1"]  = ("Creacionales",    "Singleton",          SingletonDemo.Run),
    ["2"]  = ("Creacionales",    "Factory Method",   FactoryMethodDemo.Run),
    ["3"]  = ("Creacionales",    "Abstract Factory", AbstractFactoryDemo.Run),
    ["4"]  = ("Creacionales",    "Builder",          BuilderDemo.Run),
    ["5"]  = ("Creacionales",    "Prototype",        PrototypeDemo.Run),
    ["6"]  = ("Estructurales",   "Adapter",          AdapterDemo.Run),
    ["7"]  = ("Estructurales",   "Decorator",        DecoratorDemo.Run),
    ["8"]  = ("Estructurales",   "Facade",           FacadeDemo.Run),
    ["9"]  = ("Estructurales",   "Proxy",            ProxyDemo.Run),
    ["10"] = ("Estructurales",   "Composite",        CompositeDemo.Run),
    ["11"] = ("Estructurales",   "Bridge",           BridgeDemo.Run),
    ["12"] = ("Estructurales",   "Flyweight",        FlyweightDemo.Run),
    ["13"] = ("Comportamiento",  "Strategy",         StrategyDemo.Run),
    ["14"] = ("Comportamiento",  "Observer",         ObserverDemo.Run),
    ["15"] = ("Comportamiento",  "Command",          CommandDemo.Run),
    ["16"] = ("Comportamiento",  "Template Method",  TemplateMethodDemo.Run),
    ["17"] = ("Comportamiento",  "State",            StateDemo.Run),
    ["18"] = ("Comportamiento",  "Mediator",         MediatorDemo.Run),
    ["19"] = ("Comportamiento",  "Memento",          MementoDemo.Run),
    ["20"] = ("Comportamiento",  "Chain of Responsibility", ChainOfResponsibilityDemo.Run),
};

while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════════╗");
    Console.WriteLine("║   PATRONES DE DISEÑO — .NET 10          ║");
    Console.WriteLine("╚══════════════════════════════════════════╝");
    Console.WriteLine();

    string? lastCategory = null;
    foreach (var (key, (category, name, _)) in patterns)
    {
        if (category != lastCategory)
        {
            Console.WriteLine($"  ─── {category.ToUpper()} ───");
            lastCategory = category;
        }
        Console.WriteLine($"  [{key.PadLeft(2)}] {name}");
    }
    Console.WriteLine();
    Console.WriteLine("  [Q] Salir");
    Console.WriteLine();
    Console.Write($"Selecciona un patrón (1-{patterns.Count}): ");
    var input = Console.ReadLine()?.Trim().ToUpper();

    if (input is "Q" or "SALIR") break;

    if (input != null && patterns.TryGetValue(input, out var selected))
    {
        Console.Clear();
        Console.WriteLine($"\n  >>> {selected.Name} <<<\n");
        Console.WriteLine(new string('─', 60));
        selected.Demo();
        Console.WriteLine(new string('─', 60));
        Console.WriteLine("\nPresiona ENTER para volver al menú...");
        Console.ReadLine();
    }
}
