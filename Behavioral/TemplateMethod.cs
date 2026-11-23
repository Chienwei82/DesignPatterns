namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN TEMPLATE METHOD — Esqueleto de algoritmo
/// ============================================================================
/// Intención: definir el esqueleto de un algoritmo en un método, dejando que
/// las subclases implementen ciertos pasos sin cambiar la estructura del
/// algoritmo. "Hollywood Principle": "No nos llames, nosotros te llamamos".
///
/// Analogía: receta de pan: los pasos son fijos (mezclar, amasar, hornear);
/// el tipo de harina lo eliges tú.
///
/// Cuándo usarlo: pipelines con pasos comunes que varían en el detalle
/// (procesar CSV/JSON, generar reportes).
///
/// Cuándo NO usarlo: si cada implementación difiere mucho en el flujo, mejor
/// composición (Strategy).
///
/// USO REAL: Pipelines de procesamiento de datos, generación de reportes
/// (PDF/Excel/HTML), frameworks de testing, procesos ETL.
/// ============================================================================

// --- Clase abstracta con Template Method ---
public abstract class DataPipeline
{
    // Template Method: define la estructura del algoritmo. No es virtual ni
    // abstract, por lo que las subclases NO pueden sobrescribirlo.
    public void Procesar(string fuente)
    {
        Console.WriteLine("\n  ⚙️  Iniciando pipeline de datos...\n");

        // Paso 1: Validar (hook opcional)
        if (!ValidarFuente(fuente))
        {
            Console.WriteLine("  ❌ Fuente inválida. Abortando.");
            return;
        }

        // Paso 2: Extraer (varía según la fuente)
        var datos = Extraer(fuente);

        // Paso 3: Transformar (varía según la fuente)
        Transformar(datos);

        // Paso 4: Cargar (varía según la fuente)
        Cargar(datos);

        // Paso 5: Notificar (hook opcional)
        NotificarListo(fuente);

        Console.WriteLine("  ✅ Procesamiento completado.\n");
    }

    // Pasos abstractos — las subclases DEBEN implementarlos
    protected abstract string Extraer(string fuente);
    protected abstract void Transformar(string datos);
    protected abstract void Cargar(string datos);

    // Paso con implementación por defecto (común a todos)
    protected virtual bool ValidarFuente(string fuente)
    {
        if (string.IsNullOrEmpty(fuente))
        {
            Console.WriteLine("  [Validación] No hay fuente especificada");
            return false;
        }
        Console.WriteLine($"  [Validación] Fuente válida: {fuente}");
        return true;
    }

    // Hook — las subclases PUEDEN sobrescribirlo, pero no es obligatorio
    protected virtual void NotificarListo(string fuente)
    {
        Console.WriteLine($"  [Notificación] Proceso desde '{fuente}' listo.");
    }
}

// --- Subclase concreta: CSV Pipeline ---
public class CsvPipeline : DataPipeline
{
    protected override string Extraer(string fuente)
    {
        Console.WriteLine("  [CSV] Leyendo archivo .csv...");
        return "datos CSV: id,nombre,valor\n1,Ana,100\n2,Luis,200";
    }

    protected override void Transformar(string datos) =>
        Console.WriteLine($"  [CSV] Convirtiendo a JSON: {datos}\n  → JSON generado");

    protected override void Cargar(string datos) =>
        Console.WriteLine("  [CSV] Cargando en base de datos SQL...");
}

// --- Subclase concreta: JSON API Pipeline ---
public class JsonPipeline : DataPipeline
{
    protected override string Extraer(string fuente)
    {
        Console.WriteLine("  [JSON] Consumiendo API REST...");
        return "{\"status\":200,\"data\":[...]}";
    }

    protected override void Transformar(string datos) =>
        Console.WriteLine($"  [JSON] Validando schema JSON: {datos}\n  → Schema OK");

    protected override void Cargar(string datos) =>
        Console.WriteLine("  [JSON] Guardando en almacén NoSQL (MongoDB)...");

    // Hook sobrescrito: el pipeline JSON tiene su propia notificación
    protected override void NotificarListo(string fuente) =>
        Console.WriteLine($"  [JSON] 🍣 '{fuente}' procesado. ¡Listo para consumir!");
}

public static class TemplateMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  📋 TEMPLATE METHOD — Esqueleto de algoritmo\n");
        Console.WriteLine("  Escenario: Pipelines de procesamiento de datos (CSV/JSON)\n");
        Console.WriteLine("  El flujo es siempre el mismo, pero cada paso varía");

        var pipelines = new (string nombre, DataPipeline pipeline)[]
        {
            ("CSV de ventas", new CsvPipeline()),
            ("API de productos", new JsonPipeline()),
        };

        foreach (var (nombre, pipeline) in pipelines)
        {
            Console.WriteLine($"\n  ══ Procesando: {nombre} ══");
            pipeline.Procesar(nombre);
        }

        Console.WriteLine("  ✅ El Template Method define el QUÉ y el ORDEN.");
        Console.WriteLine("     Las subclases definen el CÓMO de cada paso.");
        Console.WriteLine("     El algoritmo NUNCA cambia su estructura.");
    }
}
