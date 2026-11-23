namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN COMPOSITE — Estructuras árbol parte-todo
/// ============================================================================
/// Intención: tratar objetos individuales y composiciones de objetos de manera
/// uniforme. Los objetos se organizan en un árbol (parte-todo) y el cliente
/// trata hojas y compuestos de la misma forma.
///
/// Analogía: los ejércitos de Poniente (Game of Thrones): un soldado suelto y
/// una legión completa responden la misma pregunta ("¿cuántos hombres?").
///
/// Cuándo usarlo: sistemas de archivos, menús, organigramas, árboles de
/// componentes UI.
///
/// Cuándo NO usarlo: si la estructura nunca es anidada (solo hay hojas), el
/// árbol es innecesario.
///
/// USO REAL: Sistemas de archivos (archivos y carpetas), menús (items y
/// submenús), estructuras organizacionales, árboles de componentes UI.
/// ============================================================================

// --- Componente ---
public interface IUnidad
{
    string Nombre { get; }
    int ContarSoldados();
    void Formar(int indentacion = 0);
}

// --- Hoja: un soldado individual ---
public class Soldado : IUnidad
{
    public string Nombre { get; }

    public Soldado(string nombre) => Nombre = nombre;

    public int ContarSoldados() => 1;

    public void Formar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}⚔️ {Nombre}");
    }
}

// --- Compuesto: una legión que contiene soldados u otras legiones ---
public class Legion : IUnidad
{
    public string Nombre { get; }
    private readonly List<IUnidad> _hijos = [];

    public Legion(string nombre) => Nombre = nombre;

    public void Agregar(IUnidad unidad)
    {
        _hijos.Add(unidad);
        Console.WriteLine($"  [Legión {Nombre}] + {unidad.Nombre}");
    }

    public void Remover(IUnidad unidad)
    {
        _hijos.Remove(unidad);
        Console.WriteLine($"  [Legión {Nombre}] - {unidad.Nombre}");
    }

    // El compuesto delega a sus hijos y suma resultados (LINQ)
    public int ContarSoldados() => _hijos.Sum(u => u.ContarSoldados());

    public void Formar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}🛡️ {Nombre} ({ContarSoldados()} hombres)");

        foreach (var hijo in _hijos)
            hijo.Formar(indentacion + 1);
    }
}

public static class CompositeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌳 COMPOSITE — Estructuras árbol parte-todo (ejércitos de Poniente)\n");
        Console.WriteLine("  Escenario: Legiones de Game of Thrones con legiones dentro de legiones\n");

        // ── Hojas: soldados ──
        var arya = new Soldado("Arya Stark");
        var jon = new Soldado("Jon Snow");
        var brienne = new Soldado("Brienne de Tarth");
        var drogon = new Soldado("Drogon");
        var greyjoy = new Soldado("Yara Greyjoy");
        var dothraki = new Soldado("Khal Drogo");

        // ── Compuestos: legiones ──
        var norte = new Legion("Guardia de la Noche");
        norte.Agregar(arya);
        norte.Agregar(jon);

        var targaryen = new Legion("Hueste Targaryen");
        targaryen.Agregar(drogon);

        var alianza = new Legion("Gran Alianza del Norte");
        alianza.Agregar(norte);
        alianza.Agregar(targaryen);
        alianza.Agregar(brienne);
        alianza.Agregar(greyjoy);
        alianza.Agregar(dothraki);

        // ── Mostrar estructura completa ──
        Console.WriteLine("\n  Formación del ejército:\n");
        alianza.Formar();
        Console.WriteLine();
        Console.WriteLine($"  💀 Total de hombres: {alianza.ContarSoldados()}");
        Console.WriteLine();

        // El compuesto también permite quitar elementos
        Console.WriteLine("  ── Yara Greyjoy retira sus barcos ──");
        alianza.Remover(greyjoy);
        Console.WriteLine();
        alianza.Formar();
        Console.WriteLine();
        Console.WriteLine($"  💀 Total de hombres ahora: {alianza.ContarSoldados()}");
        Console.WriteLine();

        Console.WriteLine("  ✅ ContarSoldados() funciona igual para un soldado o una legión.");
        Console.WriteLine("     El cliente no necesita saber si es hoja o compuesto.");
    }
}
