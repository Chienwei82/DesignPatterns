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
public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instancia =
        new(() => new AppSettings());

    public static AppSettings Instancia => _instancia.Value;

    // Constructor privado: nadie más puede crear otra instancia
    private AppSettings()
    {
        Console.WriteLine("  [AppSettings] Cargando la configuración global de la app...");
        CargarValores();
    }

    public string Entorno { get; private set; } = "";
    public string CadenaConexion { get; private set; } = "";
    public int TimeoutSegundos { get; private set; }

    private void CargarValores()
    {
        Entorno = "Producción";
        CadenaConexion = "Server=db.local;Database=App;Integrated Security=true";
        TimeoutSegundos = 30;
    }

    public void Mostrar()
    {
        Console.WriteLine($"  Entorno:       {Entorno}");
        Console.WriteLine($"  Conexión:      {CadenaConexion}");
        Console.WriteLine($"  Timeout:       {TimeoutSegundos} s");
    }
}

public static class SingletonDemo
{
    public static void Run()
    {
        Console.WriteLine("  ⚙️ SINGLETON — Una sola configuración para toda la app\n");

        // Primera llamada → crea la instancia
        Console.WriteLine("  1. El módulo de logging pide la configuración:");
        var config1 = AppSettings.Instancia;
        config1.Mostrar();

        Console.WriteLine();
        Console.WriteLine("  2. El módulo de datos pide la configuración:");
        var config2 = AppSettings.Instancia;
        Console.WriteLine($"     Encuentra el mismo entorno: {config2.Entorno}");

        Console.WriteLine();
        Console.WriteLine($"  3. ¿Es la misma instancia? {ReferenceEquals(config1, config2)}");
        Console.WriteLine();
        Console.WriteLine("  ✅ Conclusión: Solo existe UNA configuración. Todos los módulos");
        Console.WriteLine("     leen los mismos valores, sin duplicar estado en memoria.");
    }
}
