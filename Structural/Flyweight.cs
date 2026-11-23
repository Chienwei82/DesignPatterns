namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN FLYWEIGHT — Compartir objetos en gran cantidad
/// ============================================================================
/// Intención: compartir eficientemente objetos que se usan en gran cantidad,
/// dividiendo el estado en intrínseco (compartido) y extrínseco
/// (proporcionado por el cliente/contexto).
///
/// Analogía: el ejército de droides de batalla de Star Wars: miles de unidades,
/// pero todas del mismo "modelo" comparten el mismo diseño en memoria; solo
/// cambia su posición en el terreno.
///
/// Cuándo usarlo: muchísimos objetos similares en memoria (tiles de
/// videojuegos, caracteres de un documento).
///
/// Cuándo NO usarlo: con pocos objetos o cuando no hay nada compartible; la
/// factory complica sin beneficio.
///
/// USO REAL: Renderizado de documentos (caracteres con fuente compartida),
/// tiles en videojuegos, iconos en mapas, caché de objetos.
/// ============================================================================

// --- Flyweight: el modelo de droide compartido (estado intrínseco) ---
public class ModeloDroide
{
    public string Tipo { get; }
    public string Color { get; }

    public ModeloDroide(string tipo, string color)
    {
        Tipo = tipo;
        Color = color;
    }

    public void Desplegar(int x, int y) =>
        Console.WriteLine($"    🤖 [{Tipo} {Color}] en posición ({x}, {y})");
}

// --- Flyweight Factory: administra y reutiliza los flyweights ---
public class FabricaDroides
{
    private readonly Dictionary<string, ModeloDroide> _modelos = new();

    public ModeloDroide ObtenerModelo(string tipo, string color)
    {
        var clave = $"{tipo}:{color}";
        if (!_modelos.TryGetValue(clave, out var modelo))
        {
            modelo = new ModeloDroide(tipo, color);
            _modelos[clave] = modelo;
            Console.WriteLine($"  [Factory] Nuevo modelo creado: {clave}");
        }
        return modelo;
    }

    public int ModelosCreados => _modelos.Count;
}

// --- Contexto: cada droide apunta a una posición (extrínseco) ---
public class Droide
{
    private readonly ModeloDroide _modelo;
    private readonly int _x;
    private readonly int _y;

    public Droide(ModeloDroide modelo, int x, int y)
    {
        _modelo = modelo;
        _x = x;
        _y = y;
    }

    public void Desplegar() => _modelo.Desplegar(_x, _y);
}

public static class FlyweightDemo
{
    public static void Run()
    {
        Console.WriteLine("  🍃 FLYWEIGHT — Compartir objetos en gran cantidad (ejército de droides)\n");
        Console.WriteLine("  Escenario: La Federación despliega 1,000 droides del mismo modelo\n");

        var fabrica = new FabricaDroides();
        var droides = new List<Droide>();
        var random = new Random(42);

        // Solo 3 tipos de droide, pero 1,000 unidades desplegadas
        var tipos = new (string tipo, string color)[]
        {
            ("B1 de batalla", "bronce"),
            ("Super de combate", "gris"),
            ("Droideka", "azul")
        };

        Console.WriteLine("  Desplegando 1,000 droides...\n");
        for (int i = 0; i < 1000; i++)
        {
            var (tipo, color) = tipos[i % 3];
            var modelo = fabrica.ObtenerModelo(tipo, color);
            droides.Add(new Droide(modelo, random.Next(0, 100), random.Next(0, 100)));
        }

        Console.WriteLine("  📊 Resultado:");
        Console.WriteLine($"     Droides desplegados: {droides.Count}");
        Console.WriteLine($"     Modelos únicos en memoria: {fabrica.ModelosCreados}");
        Console.WriteLine($"     Memoria ahorrada: ~{(1 - (double)fabrica.ModelosCreados / droides.Count) * 100:F1}%");
        Console.WriteLine();

        Console.WriteLine("  Muestra de droides:");
        for (int i = 0; i < 5; i++) droides[i].Desplegar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Flyweight evita crear 1,000 objetos idénticos.");
        Console.WriteLine("     Solo hay 3 modelos compartidos + 1,000 contextos ligeros (posición).");
    }
}
