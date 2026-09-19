namespace DesignPatterns.Creational;

/// PATRÓN SINGLETON
/// ─────────────────
/// Garantiza que una clase tenga UNA SOLA instancia en toda la aplicación
/// y provee un punto de acceso global a ella.
///
/// USO REAL: Logger, configuración global, pool de conexiones, caché.
///
/// ⚠️ CUIDADO: las implementaciones ingenuas NO son thread-safe;
///    por eso esta versión usa Lazy<T>. También dificulta los tests
///    (la inyección de dependencias suele ser mejor alternativa).

// --- Versión thread-safe con Lazy<T> ---
public sealed class DespensaCentral
{
    // Lazy<T> garantiza una sola creación, incluso con hilos concurrentes
    private static readonly Lazy<DespensaCentral> _instancia =
        new(() => new DespensaCentral());

    public static DespensaCentral Instancia => _instancia.Value;

    // Constructor privado — nadie más puede abrir otra despensa
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
        Thread.Sleep(200); // demora simulada de I/O
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
        Console.WriteLine("  1. El primer cocinero entra a la despensa (1ra vez):");
        var despensa1 = DespensaCentral.Instancia;
        despensa1.MostrarInventario();

        Console.WriteLine();
        Console.WriteLine("  2. Otro cocinero entra a la despensa (2da vez):");
        var despensa2 = DespensaCentral.Instancia;
        Console.WriteLine($"     Encuentra a la misma encargada: {despensa2.Encargada}");

        Console.WriteLine();
        Console.WriteLine($"  3. ¿Es la misma despensa? {ReferenceEquals(despensa1, despensa2)}");
        Console.WriteLine();
        Console.WriteLine("  ✅ Conclusión: Solo existe UNA despensa. Todos los cocineros");
        Console.WriteLine("     ven el mismo inventario, sin duplicar recursos.");
    }
}
