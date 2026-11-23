namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN SINGLETON — Una sola instancia global
/// ============================================================================
/// Intención: garantizar que una clase tenga UNA ÚNICA instancia en toda la
/// aplicación y proveer un punto de acceso global a ella.
///
/// Analogía: el Anillo Único de "El Señor de los Anillos": "Un anillo para
/// gobernarlo todo... una sola instancia a la que todos acuden".
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
public sealed class AnilloUnico
{
    private static readonly Lazy<AnilloUnico> _instancia =
        new(() => new AnilloUnico());

    public static AnilloUnico Instancia => _instancia.Value;

    // Constructor privado: nadie más puede forjar otro Anillo
    private AnilloUnico()
    {
        Console.WriteLine("  💍 Forjando el Anillo Único en el Monte del Destino...");
        Portador = "Nadie";
    }

    public string Portador { get; private set; } = "";
    public bool Invisible { get; private set; }

    public void Reclamar(string nombre)
    {
        Portador = nombre;
        Console.WriteLine($"  {nombre} reclama el Anillo Único.");
    }

    public void UsarInvisibilidad()
    {
        Invisible = !Invisible;
        Console.WriteLine($"  El Anillo vuelve {Portador} {(Invisible ? "invisible" : "visible")}.");
    }

    public void Mostrar()
    {
        Console.WriteLine($"  Portador actual: {Portador}");
        Console.WriteLine($"  ¿Invisible?      {(Invisible ? "Sí" : "No")}");
    }
}

public static class SingletonDemo
{
    public static void Run()
    {
        Console.WriteLine("  💍 SINGLETON — Un Anillo para gobernarlo todo\n");

        // Primera llamada → forja la instancia
        Console.WriteLine("  1. Frodo encuentra el Anillo:");
        var anillo1 = AnilloUnico.Instancia;
        anillo1.Reclamar("Frodo");
        anillo1.Mostrar();

        Console.WriteLine();
        Console.WriteLine("  2. Gandalf consulta el Anillo:");
        var anillo2 = AnilloUnico.Instancia;
        Console.WriteLine($"     Ve el mismo portador: {anillo2.Portador}");
        anillo2.UsarInvisibilidad();

        Console.WriteLine();
        Console.WriteLine($"  3. ¿Es la misma instancia? {ReferenceEquals(anillo1, anillo2)}");
        Console.WriteLine();
        Console.WriteLine("  ✅ Conclusión: Solo existe UNO. No importa quién lo pida,");
        Console.WriteLine("     todos comparten el mismo Anillo y su estado.");
    }
}
