namespace DesignPatterns.Structural;

/// PATRÓN BRIDGE
/// ─────────────
/// Separa una ABSTRACCIÓN de su IMPLEMENTACIÓN, permitiendo que ambas
/// evolucionen independientemente. Es útil cuando una clase tiene
/// múltiples dimensiones de variación (ej. forma + color).
///
/// USO REAL: Drivers de base de datos (abstracción: consulta SQL,
///             implementación: driver MySQL/PostgreSQL/SQLite),
///             controles UI multiplataforma, APIs de pago.

// --- Implementación (la parte que varía) ---
public interface IMetodoCoccion
{
    string Nombre { get; }
    void Cocinar(string alimento);
}

public class Horno : IMetodoCoccion
{
    public string Nombre => "Horno";

    public void Cocinar(string alimento) =>
        Console.WriteLine($"    [Horno] {alimento} horneado a 180°C por 25 min");
}

public class Freidora : IMetodoCoccion
{
    public string Nombre => "Freidora";

    public void Cocinar(string alimento) =>
        Console.WriteLine($"    [Freidora] {alimento} frito en aceite bien caliente");
}

public class Parrilla : IMetodoCoccion
{
    public string Nombre => "Parrilla";

    public void Cocinar(string alimento) =>
        Console.WriteLine($"    [Parrilla] {alimento} asado a la llama con marcas doradas");
}

// --- Abstracción (la parte que el cliente usa) ---
public abstract class RecetaBase
{
    protected readonly IMetodoCoccion _metodo;

    protected RecetaBase(IMetodoCoccion metodo)
    {
        _metodo = metodo;
    }

    public abstract void Preparar();
}

public class RecetaCasera : RecetaBase
{
    public RecetaCasera(IMetodoCoccion metodo) : base(metodo) { }

    public override void Preparar()
    {
        Console.WriteLine($"  [Receta Casera] Sazonando con sal y limón");
        _metodo.Cocinar("Papa");
    }
}

public class RecetaGourmet : RecetaBase
{
    public RecetaGourmet(IMetodoCoccion metodo) : base(metodo) { }

    public override void Preparar()
    {
        Console.WriteLine($"  [Receta Gourmet] Marinando con hierbas finas");
        _metodo.Cocinar("Salmón");
    }
}

public static class BridgeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌉 BRIDGE — Abstracción e implementación independientes\n");
        Console.WriteLine("  Escenario: Recetas × métodos de cocción\n");

        var metodos = new IMetodoCoccion[]
        {
            new Horno(),
            new Freidora(),
            new Parrilla()
        };

        foreach (var metodo in metodos)
        {
            Console.WriteLine($"  ── Método: {metodo.Nombre} ──");

            // La misma receta casera funciona con cualquier método
            var casera = new RecetaCasera(metodo);
            casera.Preparar();
            Console.WriteLine();
        }

        Console.WriteLine("  ── Receta gourmet en horno ──");
        var gourmet = new RecetaGourmet(new Horno());
        gourmet.Preparar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Agregar un método de cocción NO requiere cambiar las recetas.");
        Console.WriteLine("     Agregar una receta NO requiere cambiar los métodos.");
    }
}
