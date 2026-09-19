namespace DesignPatterns.Structural;

/// PATRÓN FLYWEIGHT
/// ───────────────
/// Comparte eficientemente objetos que se usan en GRAN CANTIDAD.
/// Divide el estado en INTRÍNSECO (compartido entre instancias) y
/// EXTRÍNSECO (proporcionado por el cliente/contexto).
///
/// USO REAL: Renderizado de documentos (caracteres con fuente compartida),
///           tiles en videojuegos, iconos en mapas, caché de objetos.

// --- Flyweight: la bebida compartida ---
public class TipoBebida
{
    public string Nombre { get; }
    public decimal Precio { get; }
    public string Color { get; }

    public TipoBebida(string nombre, decimal precio, string color)
    {
        Nombre = nombre;
        Precio = precio;
        Color = color;
    }

    public void Servir(int mesa)
    {
        Console.WriteLine($"    🥤 {Nombre} ({Color}) para la mesa {mesa}");
    }
}

// --- Flyweight Factory: administra y reutiliza los flyweights ---
public class FabricaBebidas
{
    private readonly Dictionary<string, TipoBebida> _tipos = new();

    public TipoBebida ObtenerTipo(string nombre, decimal precio, string color)
    {
        if (!_tipos.TryGetValue(nombre, out var tipo))
        {
            tipo = new TipoBebida(nombre, precio, color);
            _tipos[nombre] = tipo;
            Console.WriteLine($"  [Factory] Nuevo tipo de bebida creado: {nombre}");
        }
        return tipo;
    }

    public int TiposCreados => _tipos.Count;
}

// --- Contexto: cada comanda apunta a una mesa (extrínseco)
//     pero comparte el tipo de bebida (intrínseco) ---
public class Comanda
{
    private readonly TipoBebida _bebida;
    private readonly int _mesa;

    public Comanda(TipoBebida bebida, int mesa)
    {
        _bebida = bebida;
        _mesa = mesa;
    }

    public void Servir() => _bebida.Servir(_mesa);
}

public static class FlyweightDemo
{
    public static void Run()
    {
        Console.WriteLine("  🍃 FLYWEIGHT — Compartir objetos en gran cantidad\n");
        Console.WriteLine("  Escenario: Un restaurante con miles de comandas de bebida\n");

        var fabrica = new FabricaBebidas();
        var comandas = new List<Comanda>();
        var random = new Random(42);

        // Solo 3 tipos de bebida, pero miles de comandas
        var tipos = new (string nombre, decimal precio, string color)[]
        {
            ("Limonada", 1500m, "amarillo"),
            ("Café frío", 2000m, "marrón"),
            ("Agua mineral", 1000m, "transparente")
        };

        Console.WriteLine("  Levantando 1,000 comandas...\n");
        for (int i = 0; i < 1000; i++)
        {
            var (nombre, precio, color) = tipos[i % 3];
            var bebida = fabrica.ObtenerTipo(nombre, precio, color);
            comandas.Add(new Comanda(bebida, random.Next(1, 50)));
        }

        Console.WriteLine();
        Console.WriteLine("  📊 Resultado:");
        Console.WriteLine($"     Comandas levantadas: {comandas.Count}");
        Console.WriteLine($"     Tipos de bebida únicos en memoria: {fabrica.TiposCreados}");
        Console.WriteLine($"     Memoria ahorrada: ~{(1 - (double)fabrica.TiposCreados / comandas.Count) * 100:F1}%");
        Console.WriteLine();

        Console.WriteLine("  Muestra de comandas:");
        for (int i = 0; i < 5; i++) comandas[i].Servir();
        Console.WriteLine();

        Console.WriteLine("  ✅ Flyweight evita crear 1,000 objetos idénticos.");
        Console.WriteLine("     Solo se crean 3 tipos compartidos + 1,000 contextos ligeros.");
    }
}
