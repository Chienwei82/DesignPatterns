using DesignPatterns.Creational;
using DesignPatterns.Structural;
using DesignPatterns.Behavioral;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var patterns = new Dictionary<string, (string Category, Action Demo)>
{
    ["1"]  = ("Creacionales",    SingletonDemo.Run),
    ["2"]  = ("Creacionales",    FactoryMethodDemo.Run),
    ["3"]  = ("Creacionales",    AbstractFactoryDemo.Run),
    ["4"]  = ("Creacionales",    BuilderDemo.Run),
    ["5"]  = ("Creacionales",    PrototypeDemo.Run),
    ["6"]  = ("Estructurales",   AdapterDemo.Run),
    ["7"]  = ("Estructurales",   DecoratorDemo.Run),
    ["8"]  = ("Estructurales",   FacadeDemo.Run),
    ["9"]  = ("Estructurales",   ProxyDemo.Run),
    ["10"] = ("Estructurales",   CompositeDemo.Run),
    ["11"] = ("Comportamiento",  StrategyDemo.Run),
    ["12"] = ("Comportamiento",  ObserverDemo.Run),
    ["13"] = ("Comportamiento",  CommandDemo.Run),
    ["14"] = ("Comportamiento",  TemplateMethodDemo.Run),
    ["15"] = ("Comportamiento",  StateDemo.Run),
};

var patternNames = new Dictionary<string, string>
{
    ["1"]  = "Singleton",
    ["2"]  = "Factory Method",
    ["3"]  = "Abstract Factory",
    ["4"]  = "Builder",
    ["5"]  = "Prototype",
    ["6"]  = "Adapter",
    ["7"]  = "Decorator",
    ["8"]  = "Facade",
    ["9"]  = "Proxy",
    ["10"] = "Composite",
    ["11"] = "Strategy",
    ["12"] = "Observer",
    ["13"] = "Command",
    ["14"] = "Template Method",
    ["15"] = "State",
};

while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════════╗");
    Console.WriteLine("║   PATRONES DE DISEÑO — .NET 10          ║");
    Console.WriteLine("╚══════════════════════════════════════════╝");
    Console.WriteLine();

    string? lastCategory = null;
    foreach (var (key, (category, _)) in patterns)
    {
        if (category != lastCategory)
        {
            Console.WriteLine($"  ─── {category.ToUpper()} ───");
            lastCategory = category;
        }
        Console.WriteLine($"  [{key.PadLeft(2)}] {patternNames[key]}");
    }
    Console.WriteLine();
    Console.WriteLine("  [Q] Salir");
    Console.WriteLine();
    Console.Write("Selecciona un patrón (1-15): ");
    var input = Console.ReadLine()?.Trim().ToUpper();

    if (input is "Q" or "SALIR") break;

    if (input != null && patterns.TryGetValue(input, out var selected))
    {
        Console.Clear();
        Console.WriteLine($"\n  >>> {patternNames[input]} <<<\n");
        Console.WriteLine(new string('─', 60));
        selected.Demo();
        Console.WriteLine(new string('─', 60));
        Console.WriteLine("\nPresiona ENTER para volver al menú...");
        Console.ReadLine();
    }
}
