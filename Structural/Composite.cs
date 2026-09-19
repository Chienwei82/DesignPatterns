namespace DesignPatterns.Structural;

/// PATRÓN COMPOSITE
/// ────────────────
/// Permite tratar objetos INDIVIDUALES y COMPOSICIONES de objetos
/// de manera UNIFORME. Los objetos se organizan en una estructura
/// de árbol (parte-todo), y el cliente puede tratar hojas y
/// compuestos de la misma forma.
///
/// USO REAL: Sistemas de archivos (archivos y carpetas), menús
///           (items y submenús), estructuras organizacionales,
///           árboles de componentes UI en WPF/MAUI.

// --- Componente ---
public interface IElementoMenu
{
    string Nombre { get; }
    decimal GetPrecioTotal();
    int GetCalorias();
    void Mostrar(int indentacion = 0);
}

// --- Hoja: un platillo individual ---
public class PlatilloMenu : IElementoMenu
{
    public string Nombre { get; }
    public decimal Precio { get; }
    public int Calorias { get; }

    public PlatilloMenu(string nombre, decimal precio, int calorias)
    {
        Nombre = nombre;
        Precio = precio;
        Calorias = calorias;
    }

    public decimal GetPrecioTotal() => Precio;

    public int GetCalorias() => Calorias;

    public void Mostrar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}🍽️  {Nombre} — ¢{Precio:N0} ({Calorias} kcal)");
    }
}

// --- Compuesto: un combo que contiene platillos u otros combos ---
public class ComboMenu : IElementoMenu
{
    public string Nombre { get; }
    private readonly List<IElementoMenu> _items = [];

    public ComboMenu(string nombre)
    {
        Nombre = nombre;
    }

    public void Agregar(IElementoMenu elemento)
    {
        _items.Add(elemento);
        Console.WriteLine($"  [Combo {Nombre}] + {elemento.Nombre}");
    }

    public void Remover(IElementoMenu elemento)
    {
        _items.Remove(elemento);
        Console.WriteLine($"  [Combo {Nombre}] - {elemento.Nombre}");
    }

    // El compuesto delega a sus hijos y suma resultados (LINQ)
    public decimal GetPrecioTotal() => _items.Sum(i => i.GetPrecioTotal());

    public int GetCalorias() => _items.Sum(i => i.GetCalorias());

    public void Mostrar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}📦 {Nombre} — ¢{GetPrecioTotal():N0}, {GetCalorias()} kcal");

        foreach (var item in _items)
            item.Mostrar(indentacion + 1);
    }
}

public static class CompositeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌳 COMPOSITE — Estructuras árbol parte-todo\n");
        Console.WriteLine("  Escenario: Menú del restaurante con combos dentro de combos\n");

        // ── Construir menú ──
        Console.WriteLine("  Construyendo menú...\n");

        // Hojas: platillos
        var pizza = new PlatilloMenu("Pizza margarita", 9500m, 800);
        var ensalada = new PlatilloMenu("Ensalada césar", 4500m, 250);
        var pasta = new PlatilloMenu("Pasta alfredo", 7500m, 700);
        var sopa = new PlatilloMenu("Sopa de tomate", 3500m, 180);
        var helado = new PlatilloMenu("Helado de vainilla", 2500m, 300);
        var cafe = new PlatilloMenu("Café expreso", 1800m, 5);

        // Combos (compuestos)
        var comboInfantil = new ComboMenu("Combo Infantil");
        comboInfantil.Agregar(pasta);
        comboInfantil.Agregar(helado);

        var comboPareja = new ComboMenu("Combo Pareja");
        comboPareja.Agregar(pizza);
        comboPareja.Agregar(ensalada);
        comboPareja.Agregar(sopa);

        var comboFamiliar = new ComboMenu("Combo Familiar");
        comboFamiliar.Agregar(comboPareja);
        comboFamiliar.Agregar(comboInfantil);
        comboFamiliar.Agregar(cafe);

        // ── Mostrar menú completo ──
        comboFamiliar.Mostrar();
        Console.WriteLine();
        Console.WriteLine($"  💰 Precio total: ¢{comboFamiliar.GetPrecioTotal():N0}");
        Console.WriteLine($"  🔥 Calorías totales: {comboFamiliar.GetCalorias()}");
        Console.WriteLine();

        // El compuesto también permite quitar elementos
        Console.WriteLine("  ── Quitamos el café del combo familiar ──");
        comboFamiliar.Remover(cafe);
        Console.WriteLine();
        comboFamiliar.Mostrar();
        Console.WriteLine();
        Console.WriteLine($"  💰 Precio total ahora: ¢{comboFamiliar.GetPrecioTotal():N0}");
        Console.WriteLine();

        Console.WriteLine("  ✅ El método GetPrecioTotal() funciona igual para");
        Console.WriteLine("     un platillo o un combo entero.");
        Console.WriteLine("     El cliente no necesita saber si es hoja o compuesto.");
    }
}
