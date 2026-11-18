namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN SINGLETON — Una sola instancia global
/// ============================================================================
/// Intención: garantizar que una clase tenga UNA ÚNICA instancia en toda la
/// aplicación y proveer un punto de acceso global a ella.
///
/// Analogía: el presidente del país: solo puede haber uno y todos lo consultan.
///
/// Cuándo usarlo: configuración global, logger, caché compartida, pool de
/// conexiones.
///
/// Cuándo NO usarlo: cuando necesitas varias instancias en tests (acopla el
/// código y dificulta el testing). En apps reales, la inyección de dependencias
/// suele ser mejor alternativa.
///
/// USO REAL: Logger, configuración global, pool de conexiones, caché.
/// ============================================================================

// --- Versión thread-safe con Lazy<T> (segura con hilos concurrentes) ---
public sealed class DespensaCentral
{
    private static readonly Lazy<DespensaCentral> _instancia =
        new(() => new DespensaCentral());

    public static DespensaCentral Instancia => _instancia.Value;

    // Constructor privado: nadie más puede crear otra instancia
    private DespensaCentral()
    {
        Console.WriteLine("  [DespensaCentral] Abriendo la única despensa del restaurante...");
        CargarInventario();
    }

    public string Encargada { get; private set; } = "";
    public int Tomates { get; private set; }
    public int Quesos { get; private set; }

    private void CargarInventario()
    {
        Encargada = "Doña Marta";
        Tomates = 40;
        Quesos = 25;
    }

    public void MostrarInventario()
    {
        Console.WriteLine($"  Encargada: {Encargada}");
        Console.WriteLine($"  Inventario: {Tomates} tomates, {Quesos} quesos");
    }
}

public static class SingletonDemo
{
    public static void Run()
    {
        Console.WriteLine("  📦 SINGLETON — Una sola despensa para todo el restaurante\n");

        // Primera llamada → crea la instancia
        Console.WriteLine("  1. El primer cocinero entra a la despensa:");
        var despensa1 = DespensaCentral.Instancia;
        despensa1.MostrarInventario();

        Console.WriteLine();
        Console.WriteLine("  2. Otro cocinero entra a la despensa:");
        var despensa2 = DespensaCentral.Instancia;
        Console.WriteLine($"     Encuentra a la misma encargada: {despensa2.Encargada}");

        Console.WriteLine();
        Console.WriteLine($"  3. ¿Es la misma despensa? {ReferenceEquals(despensa1, despensa2)}");
        Console.WriteLine();
        Console.WriteLine("  ✅ Conclusión: Solo existe UNA despensa. Todos los cocineros");
        Console.WriteLine("     ven el mismo inventario, sin duplicar recursos.");
    }
}
