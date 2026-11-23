namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN FLYWEIGHT — Compartir objetos en gran cantidad
/// ============================================================================
/// Intención: compartir eficientemente objetos que se usan en gran cantidad,
/// dividiendo el estado en intrínseco (compartido) y extrínseco
/// (proporcionado por el cliente/contexto).
///
/// Analogía: un solo molde reutilizado para servir mil porciones en mesas
/// distintas. En este proyecto: un mismo sprite de tile compartido por miles
/// de casillas de un mapa 2D.
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

// --- Flyweight: el tile compartido (estado intrínseco) ---
public class TileSprite
{
    public string Textura { get; }
    public string Color { get; }

    public TileSprite(string textura, string color)
    {
        Textura = textura;
        Color = color;
    }

    public void Dibujar(int x, int y) =>
        Console.WriteLine($"    🎮 [{Textura} {Color}] en ({x}, {y})");
}

// --- Flyweight Factory: administra y reutiliza los flyweights ---
public class FabricaSprites
{
    private readonly Dictionary<string, TileSprite> _tipos = new();

    public TileSprite ObtenerSprite(string textura, string color)
    {
        var clave = $"{textura}:{color}";
        if (!_tipos.TryGetValue(clave, out var sprite))
        {
            sprite = new TileSprite(textura, color);
            _tipos[clave] = sprite;
            Console.WriteLine($"  [Factory] Nuevo sprite creado: {clave}");
        }
        return sprite;
    }

    public int TiposCreados => _tipos.Count;
}

// --- Contexto: cada tile apunta a una posición (extrínseco) ---
public class Tile
{
    private readonly TileSprite _sprite;
    private readonly int _x;
    private readonly int _y;

    public Tile(TileSprite sprite, int x, int y)
    {
        _sprite = sprite;
        _x = x;
        _y = y;
    }

    public void Dibujar() => _sprite.Dibujar(_x, _y);
}

public static class FlyweightDemo
{
    public static void Run()
    {
        Console.WriteLine("  🍃 FLYWEIGHT — Compartir objetos en gran cantidad\n");
        Console.WriteLine("  Escenario: Un juego 2D con miles de tiles del mismo tipo\n");

        var fabrica = new FabricaSprites();
        var tiles = new List<Tile>();
        var random = new Random(42);

        // Solo 3 tipos de tile, pero 1,000 tiles en el mapa
        var tipos = new (string textura, string color)[]
        {
            ("pasto", "verde"),
            ("arena", "amarillo"),
            ("agua", "azul")
        };

        Console.WriteLine("  Generando 1,000 tiles del mapa...\n");
        for (int i = 0; i < 1000; i++)
        {
            var (textura, color) = tipos[i % 3];
            var sprite = fabrica.ObtenerSprite(textura, color);
            tiles.Add(new Tile(sprite, random.Next(0, 100), random.Next(0, 100)));
        }

        Console.WriteLine("  📊 Resultado:");
        Console.WriteLine($"     Tiles creados: {tiles.Count}");
        Console.WriteLine($"     Sprites únicos en memoria: {fabrica.TiposCreados}");
        Console.WriteLine($"     Memoria ahorrada: ~{(1 - (double)fabrica.TiposCreados / tiles.Count) * 100:F1}%");
        Console.WriteLine();

        Console.WriteLine("  Muestra de tiles:");
        for (int i = 0; i < 5; i++) tiles[i].Dibujar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Flyweight evita crear 1,000 objetos idénticos.");
        Console.WriteLine("     Solo se crean 3 sprites compartidos + 1,000 contextos ligeros.");
    }
}
