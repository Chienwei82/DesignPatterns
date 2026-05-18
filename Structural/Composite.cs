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
public interface IComponenteOrganigrama
{
    string Nombre { get; }
    decimal GetCostoTotal();
    int GetPersonasContadas();
    void Mostrar(int indentacion = 0);
}

// --- Hoja: un empleado individual ---
public class Empleado : IComponenteOrganigrama
{
    public string Nombre { get; }
    public string Cargo { get; }
    public decimal Salario { get; }

    public Empleado(string nombre, string cargo, decimal salario)
    {
        Nombre = nombre;
        Cargo = cargo;
        Salario = salario;
    }

    public decimal GetCostoTotal() => Salario;

    public int GetPersonasContadas() => 1;

    public void Mostrar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}👤 {Nombre} ({Cargo}) — ¢{Salario:N0}/mes");
    }
}

// --- Compuesto: un departamento que contiene empleados + sub-departamentos ---
public class Departamento : IComponenteOrganigrama
{
    public string Nombre { get; }
    private readonly List<IComponenteOrganigrama> _subordinados = [];

    public Departamento(string nombre)
    {
        Nombre = nombre;
    }

    public void Agregar(IComponenteOrganigrama componente)
    {
        _subordinados.Add(componente);
        Console.WriteLine($"  [Depto {Nombre}] + {componente.Nombre}");
    }

    public void Remover(IComponenteOrganigrama componente)
    {
        _subordinados.Remove(componente);
    }

    // El compuesto delega a sus hijos y suma resultados
    public decimal GetCostoTotal()
    {
        var total = 0m;
        foreach (var sub in _subordinados)
            total += sub.GetCostoTotal();
        return total;
    }

    public int GetPersonasContadas()
    {
        var count = 0;
        foreach (var sub in _subordinados)
            count += sub.GetPersonasContadas();
        return count;
    }

    public void Mostrar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}📁 {Nombre} — Gasto: ¢{GetCostoTotal():N0}/mes, {GetPersonasContadas()} personas");

        foreach (var sub in _subordinados)
            sub.Mostrar(indentacion + 1);
    }
}

public static class CompositeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌳 COMPOSITE — Estructuras árbol parte-todo\n");
        Console.WriteLine("  Escenario: Organigrama empresarial con costo por departamento\n");

        // ── Construir organigrama ──
        Console.WriteLine("  Construyendo organigrama...\n");

        // Hojas: empleados
        var ana = new Empleado("Ana López", "Desarrolladora Sr", 2_500_000m);
        var carlos = new Empleado("Carlos Ruiz", "Desarrollador Jr", 1_200_000m);
        var maria = new Empleado("María Soto", "QA Engineer", 1_800_000m);

        var pedro = new Empleado("Pedro Mora", "Soporte Técnico", 1_000_000m);
        var lucia = new Empleado("Lucía Vega", "SysAdmin", 2_000_000m);

        var rosa = new Empleado("Rosa Martínez", "CEO", 5_000_000m);
        var juan = new Empleado("Juan Castillo", "CFO", 4_000_000m);

        // Departamentos (compuestos)
        var desarrollo = new Departamento("Desarrollo");
        desarrollo.Agregar(ana);
        desarrollo.Agregar(carlos);
        desarrollo.Agregar(maria);

        var infraestructura = new Departamento("Infraestructura");
        infraestructura.Agregar(pedro);
        infraestructura.Agregar(lucia);

        var tecnologia = new Departamento("Tecnología");
        tecnologia.Agregar(desarrollo);
        tecnologia.Agregar(infraestructura);

        var direccion = new Departamento("Dirección");
        direccion.Agregar(rosa);
        direccion.Agregar(juan);

        var empresa = new Departamento("Tech Solutions CR");
        empresa.Agregar(tecnologia);
        empresa.Agregar(direccion);

        // ── Mostrar organigrama completo ──
        empresa.Mostrar();
        Console.WriteLine();
        Console.WriteLine($"  💰 Gasto total en planilla: ¢{empresa.GetCostoTotal():N0}/mes");
        Console.WriteLine($"  👥 Total empleados: {empresa.GetPersonasContadas()}");
        Console.WriteLine();

        // El cliente trata empleados y departamentos igual
        Console.WriteLine("  ✅ El método GetCostoTotal() funciona igual para");
        Console.WriteLine("     un empleado o un departamento completo.");
        Console.WriteLine("     El cliente no necesita saber si es hoja o compuesto.");
    }
}
