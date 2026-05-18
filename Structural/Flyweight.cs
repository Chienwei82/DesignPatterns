namespace DesignPatterns.Structural;

/// PATRÓN FLYWEIGHT
/// ───────────────
/// Comparte eficientemente objetos que se usan en GRAN CANTIDAD.
/// Divide el estado en INTRÍNSECO (compartido entre instancias) y
/// EXTRÍNSECO (proporcionado por el cliente/contexto).
///
/// USO REAL: Renderizado de documentos (caracteres con fuente compartida),
///           tiles en videojuegos, iconos en mapas, caché de objetos.

// --- Flyweight: el objeto compartido ---
public class TipoArbol
{
    public string Nombre { get; }
    public string Color { get; }
    public string Textura { get; }

    public TipoArbol(string nombre, string color, string textura)
    {
        Nombre = nombre;
        Color = color;
        Textura = textura;
    }

    public void Dibujar(int x, int y)
    {
        Console.WriteLine($"    🌳 {Nombre} ({Color}, {Textura}) en [{x}, {y}]");
    }
}

// --- Flyweight Factory: administra y reutiliza los flyweights ---
public class FabricaArboles
{
    private readonly Dictionary<string, TipoArbol> _tipos = new();

    public TipoArbol ObtenerTipo(string nombre, string color, string textura)
    {
        var clave = $"{nombre}-{color}-{textura}";
        if (!_tipos.TryGetValue(clave, out var tipo))
        {
            tipo = new TipoArbol(nombre, color, textura);
            _tipos[clave] = tipo;
            Console.WriteLine($"  [Factory] Nuevo tipo creado: {clave}");
        }
        return tipo;
    }

    public int TiposCreados => _tipos.Count;
}

// --- Contexto: cada árbol en el bosque tiene posición única (extrínseco)
//    pero comparte el tipo (intrínseco) ---
public class Arbol
{
    private readonly TipoArbol _tipo;
    private readonly int _x;
    private readonly int _y;

    public Arbol(TipoArbol tipo, int x, int y)
    {
        _tipo = tipo;
        _x = x;
        _y = y;
    }

    public void Dibujar() => _tipo.Dibujar(_x, _y);
}

public static class FlyweightDemo
{
    public static void Run()
    {
        Console.WriteLine("  🍃 FLYWEIGHT — Compartir objetos en gran cantidad\n");
        Console.WriteLine("  Escenario: Bosque con miles de árboles\n");

        var fabrica = new FabricaArboles();
        var bosque = new List<Arbol>();
        var random = new Random(42);

        // Solo 3 tipos de árboles, pero miles de instancias
        string[] nombres = { "Roble", "Pino", "Palma" };
        string[] colores = { "Verde oscuro", "Verde claro", "Verde tropical" };
        string[] texturas = { "Rugosa", "Lisa", "Fibrosa" };

        Console.WriteLine("  Plantando 1,000 árboles...\n");
        for (int i = 0; i < 1000; i++)
        {
            var tipo = fabrica.ObtenerTipo(
                nombres[i % 3],
                colores[i % 3],
                texturas[i % 3]);

            bosque.Add(new Arbol(tipo, random.Next(0, 1000), random.Next(0, 1000)));
        }

        Console.WriteLine($"\n  📊 Resultado:");
        Console.WriteLine($"     Árboles plantados: {bosque.Count}");
        Console.WriteLine($"     Tipos de árbol únicos en memoria: {fabrica.TiposCreados}");
        Console.WriteLine($"     Memoria ahorrada: ~{(1 - (double)fabrica.TiposCreados / bosque.Count) * 100:F1}%");
        Console.WriteLine();

        Console.WriteLine("  Muestra de árboles:");
        for (int i = 0; i < 5; i++) bosque[i].Dibujar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Flyweight evita crear 1,000 objetos idénticos.");
        Console.WriteLine("     Solo se crean 3 tipos compartidos + 1,000 contextos ligeros.");
    }
}
