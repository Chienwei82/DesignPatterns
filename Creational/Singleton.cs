namespace DesignPatterns.Creational;

/// PATRÓN SINGLETON
/// ─────────────────
/// Garantiza que una clase tenga UNA SOLA instancia en toda la aplicación
/// y provee un punto de acceso global a ella.
///
/// USO REAL: Logger, manejador de configuración, pool de conexiones,
///           caché en memoria.
///
/// ⚠️ PELIGROS: No es thread-safe por defecto; difícil de testear
///    (inyección de dependencias es mejor alternativa).

// --- Versión thread-safe con Lazy<T> ---
public sealed class ConfiguracionApp
{
    // Lazy<T> garantiza una sola creación, incluso con hilos concurrentes
    private static readonly Lazy<ConfiguracionApp> _instancia =
        new(() => new ConfiguracionApp());

    public static ConfiguracionApp Instancia => _instancia.Value;

    // Constructor privado — nadie más puede instanciar
    private ConfiguracionApp()
    {
        Console.WriteLine("  [ConfiguracionApp] Creando instancia única...");
        CargarDesdeArchivo();
    }

    public string AppName { get; private set; } = "";
    public string Version { get; private set; } = "";
    public string DbConnectionString { get; private set; } = "";

    private void CargarDesdeArchivo()
    {
        // Simula lectura de appsettings.json
        Thread.Sleep(200); // demora simulada de I/O
        AppName = "Sistema de Ventas";
        Version = "3.2.1";
        DbConnectionString = "Server=db01;Database=ventas;Trusted=true;";
    }

    public void MostrarInfo()
    {
        Console.WriteLine($"  App: {AppName} v{Version}");
        Console.WriteLine($"  DB:  {DbConnectionString}");
    }
}

public static class SingletonDemo
{
    public static void Run()
    {
        Console.WriteLine("  📦 SINGLETON — Una sola instancia global\n");

        // Primera llamada → crea la instancia
        Console.WriteLine("  1. Accediendo a ConfiguracionApp.Instancia (1ra vez):");
        var cfg1 = ConfiguracionApp.Instancia;
        cfg1.MostrarInfo();

        Console.WriteLine();
        Console.WriteLine("  2. Accediendo a ConfiguracionApp.Instancia (2da vez):");
        var cfg2 = ConfiguracionApp.Instancia;
        cfg2.MostrarInfo();

        Console.WriteLine();
        Console.WriteLine($"  3. ¿Es la misma instancia? {ReferenceEquals(cfg1, cfg2)}");
        Console.WriteLine($"     HashCode #1: {cfg1.GetHashCode()}");
        Console.WriteLine($"     HashCode #2: {cfg2.GetHashCode()}");
        Console.WriteLine();
        Console.WriteLine("  ✅ Conclusión: Solo se creó UNA vez, ambas variables");
        Console.WriteLine("     apuntan al mismo objeto en memoria.");
    }
}
