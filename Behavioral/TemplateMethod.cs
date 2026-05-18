namespace DesignPatterns.Behavioral;

/// PATRÓN TEMPLATE METHOD
/// ──────────────────────
/// Define el ESQUELETO de un algoritmo en un método, dejando que
/// las subclases implementen ciertos pasos SIN cambiar la estructura
/// del algoritmo. "Invertir el control" — Hollywood Principle:
/// "No nos llames, nosotros te llamamos".
///
/// USO REAL: Pipelines de procesamiento de datos, generación de
///           reportes (PDF/Excel/HTML), frameworks de testing,
///           procesos ETL, recetas de cocina.

// --- Clase abstracta con Template Method ---
public abstract class ProcesadorArchivo
{
    /// Template Method — define la estructura del algoritmo
    /// Las subclases NO pueden sobrescribir este método (en C# usamos sealed)
    /// pero en este demo lo dejamos como virtual para simplicidad educativa.
    public void Procesar(string rutaArchivo)
    {
        Console.WriteLine("\n  ⚙️  Iniciando procesamiento...\n");

        // Paso 1: Validar (hook opcional)
        if (!ValidarArchivo(rutaArchivo))
        {
            Console.WriteLine("  ❌ Archivo inválido. Abortando.");
            return;
        }

        // Paso 2: Abrir (varía según tipo de archivo)
        var datos = AbrirArchivo(rutaArchivo);

        // Paso 3: Extraer datos (varía según tipo)
        var contenido = ExtraerDatos(datos);

        // Paso 4: Transformar (común para todos)
        var transformado = Transformar(contenido);

        // Paso 5: Guardar resultado (varía según tipo)
        GuardarResultado(transformado);

        // Paso 6: Notificar (hook opcional)
        NotificarCompletado(rutaArchivo);

        Console.WriteLine("  ✅ Procesamiento completado.\n");
    }

    // Pasos abstractos — las subclases DEBEN implementarlos
    protected abstract byte[] AbrirArchivo(string ruta);
    protected abstract string ExtraerDatos(byte[] datos);
    protected abstract void GuardarResultado(string contenido);

    // Pasos con implementación por defecto (comunes a todos)
    protected virtual bool ValidarArchivo(string ruta)
    {
        if (string.IsNullOrEmpty(ruta))
        {
            Console.WriteLine("  [Validación] Ruta vacía");
            return false;
        }
        Console.WriteLine($"  [Validación] Archivo válido: {ruta}");
        return true;
    }

    protected string Transformar(string contenido)
    {
        Console.WriteLine("  [Transformación] Normalizando datos...");
        return contenido.Trim().ToUpperInvariant();
    }

    // Hook — las subclases PUEDEN sobrescribirlo, pero no es obligatorio
    protected virtual void NotificarCompletado(string ruta)
    {
        Console.WriteLine($"  [Notificación] Procesamiento de '{ruta}' finalizado.");
    }
}

// --- Subclase concreta: Procesador CSV ---
public class ProcesadorCSV : ProcesadorArchivo
{
    protected override byte[] AbrirArchivo(string ruta)
    {
        Console.WriteLine($"  [CSV] Abriendo archivo CSV: {ruta}");
        // Simula lectura
        return System.Text.Encoding.UTF8.GetBytes("nombre,edad\nAna,30\nCarlos,25");
    }

    protected override string ExtraerDatos(byte[] datos)
    {
        Console.WriteLine("  [CSV] Parseando filas y columnas...");
        var texto = System.Text.Encoding.UTF8.GetString(datos);
        return string.Join(" | ", texto.Split('\n'));
    }

    protected override void GuardarResultado(string contenido)
    {
        Console.WriteLine($"  [CSV] Guardando como CSV procesado...");
        Console.WriteLine($"  [CSV] Resultado: {contenido}");
    }

    // Hook sobrescrito
    protected override void NotificarCompletado(string ruta)
    {
        Console.WriteLine($"  [CSV] ✅ Archivo '{ruta}' procesado. Filas: 3");
    }
}

// --- Subclase concreta: Procesador JSON ---
public class ProcesadorJSON : ProcesadorArchivo
{
    protected override byte[] AbrirArchivo(string ruta)
    {
        Console.WriteLine($"  [JSON] Abriendo archivo JSON: {ruta}");
        return System.Text.Encoding.UTF8.GetBytes("{\"nombre\":\"Ana\",\"edad\":30}");
    }

    protected override string ExtraerDatos(byte[] datos)
    {
        Console.WriteLine("  [JSON] Parseando objeto JSON...");
        var texto = System.Text.Encoding.UTF8.GetString(datos);
        return texto.Replace("\"", "").Replace("{", "").Replace("}", "");
    }

    protected override void GuardarResultado(string contenido)
    {
        Console.WriteLine($"  [JSON] Guardando como JSON procesado...");
        Console.WriteLine($"  [JSON] Resultado: {contenido}");
    }

    // También podemos sobrescribir el hook ValidarArchivo
    protected override bool ValidarArchivo(string ruta)
    {
        if (!ruta.EndsWith(".json"))
        {
            Console.WriteLine($"  [JSON] ⚠️  Extensión no esperada: {ruta}");
            return false;
        }
        return base.ValidarArchivo(ruta);
    }
}

// --- Subclase concreta: Procesador PDF ---
public class ProcesadorPDF : ProcesadorArchivo
{
    protected override byte[] AbrirArchivo(string ruta)
    {
        Console.WriteLine($"  [PDF] Abriendo PDF: {ruta} (requiere librería externa)");
        return "PDF:Datos simulados"u8.ToArray();
    }

    protected override string ExtraerDatos(byte[] datos)
    {
        Console.WriteLine("  [PDF] Extrayendo texto del PDF (OCR si es escaneado)...");
        return "Texto extraído del documento PDF";
    }

    protected override void GuardarResultado(string contenido)
    {
        Console.WriteLine($"  [PDF] Exportando resultado como PDF procesado...");
        Console.WriteLine($"  [PDF] Contenido extraído: {contenido}");
    }
}

public static class TemplateMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  📋 TEMPLATE METHOD — Esqueleto de algoritmo\n");
        Console.WriteLine("  Escenario: Procesamiento de archivos (CSV/JSON/PDF)\n");
        Console.WriteLine("  El flujo es siempre el mismo, pero cada paso varía");

        // Procesar usando el template method
        var archivos = new (string nombre, string ruta, ProcesadorArchivo procesador)[]
        {
            ("CSV",  "datos.csv",  new ProcesadorCSV()),
            ("JSON", "config.json", new ProcesadorJSON()),
            ("PDF",  "reporte.pdf", new ProcesadorPDF()),
        };

        foreach (var (nombre, ruta, proc) in archivos)
        {
            Console.WriteLine($"\n  ══ Procesando archivo {nombre} ══");
            proc.Procesar(ruta);
        }

        Console.WriteLine("  ✅ El Template Method define el QUÉ y el ORDEN.");
        Console.WriteLine("     Las subclases definen el CÓMO de cada paso.");
        Console.WriteLine("     El algoritmo NUNCA cambia su estructura.");
    }
}
