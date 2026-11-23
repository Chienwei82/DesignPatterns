namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN PROTOTYPE — Clonar objetos existentes
/// ============================================================================
/// Intención: crear nuevos objetos copiando un prototipo existente, en vez de
/// construirlos desde cero. La copia puede ser superficial (comparte
/// referencias) o profunda (todo nuevo).
///
/// Analogía: fotocopia de un formulario prellenado que luego personalizas.
///
/// Cuándo usarlo: duplicar recetas/documentos plantilla, crear muchos
/// enemigos similares en un videojuego.
///
/// Cuándo NO usarlo: cuando el objeto es simple y barato de crear con `new`.
///
/// Cuidado: diferencia copia superficial (shallow: comparte listas) de copia
/// profunda (deep: duplica todo).
///
/// USO REAL: Clonación de configuraciones, duplicación de documentos,
/// creación de enemigos en videojuegos, evitar costos de inicialización.
/// ============================================================================

// ICloneable es la interfaz nativa de .NET para clonación, aunque devuelve
// object (no genérica). Hoy se prefiere un método propio como DeepClone().

// --- Prototipo concreto: Documento ---
public class Documento : ICloneable
{
    public string Titulo { get; set; } = "";
    public string Autor { get; set; } = "";
    public List<string> Clausulas { get; set; } = [];
    public Firmante Firmante { get; set; } = new();

    public Documento(string titulo)
    {
        Titulo = titulo;
        Firmante = new Firmante();
    }

    // Clone(): copia superficial (shallow) — listas y objetos se COMPARTEN
    public object Clone() => MemberwiseClone();

    // DeepClone(): copia profunda — TODO se duplica
    public Documento DeepClone()
    {
        var copia = (Documento)MemberwiseClone();
        copia.Clausulas = new List<string>(Clausulas);
        copia.Firmante = Firmante with { }; // record inmutable: `with` crea uno nuevo
        return copia;
    }

    public void Mostrar()
    {
        Console.WriteLine($"  Documento:  {Titulo}");
        Console.WriteLine($"  Autor:      {Autor}");
        Console.WriteLine($"  Cláusulas:  {(Clausulas.Count > 0 ? string.Join(", ", Clausulas) : "(ninguna)")}");
        Console.WriteLine($"  Firmante:   {Firmante.Nombre} ({Firmante.Empresa})");
    }
}

public record Firmante(string Nombre = "", string Empresa = "", string Email = "");

public static class PrototypeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧬 PROTOTYPE — Clonar documentos plantilla\n");
        Console.WriteLine("  Escenario: El equipo legal clona su plantilla maestra para crear");
        Console.WriteLine("  variantes sin empezar de cero\n");

        // ── Documento original (prototipo base) ──
        var docBase = new Documento("Contrato de servicios")
        {
            Autor = "Departamento Legal",
            Clausulas = ["Confidencialidad", "Propiedad intelectual", "Jurisdicción"],
            Firmante = new Firmante
            {
                Nombre = "Ana Torres",
                Empresa = "Acme S.A.",
                Email = "ana@acme.com"
            }
        };

        Console.WriteLine("  📄 DOCUMENTO ORIGINAL (prototipo):")
        ;
        docBase.Mostrar();
        Console.WriteLine();

        // ── Clon superficial: la lista se COMPARTE ──
        Console.WriteLine("  ── CLON SUPERFICIAL (shallow copy) ──");
        var docClon = (Documento)docBase.Clone();
        docClon.Titulo = "Contrato de confidencialidad (copia)";
        docClon.Clausulas.Add("No divulgación"); // ⚠️ afecta al original
        Console.WriteLine("  (le agregamos una cláusula a la copia...)");
        Console.WriteLine();

        Console.WriteLine("  📄 DOCUMENTO ORIGINAL después de modificar la copia:");
        docBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ⚠️  ¡La 'No divulgación' apareció en el original!");
        Console.WriteLine("     El clon superficial comparte la lista.\n");

        // ── Clon profundo: todo se duplica ──
        Console.WriteLine("  ── CLON PROFUNDO (deep copy) ──");
        var docDeep = docBase.DeepClone();
        docDeep.Titulo = "Contrato de arrendamiento (deep)";
        docDeep.Clausulas.Add("Caución"); // solo en el deep clone
        Console.WriteLine("  (le agregamos 'Caución' a la copia profunda...)");
        Console.WriteLine();

        Console.WriteLine("  📄 DOCUMENTO ORIGINAL (sin cambios):")
        ;
        docBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  📄 Copia profunda (modificada):")
        ;
        docDeep.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ✅ Con deep copy, las modificaciones no afectan al original.");
        Console.WriteLine("     Útil cuando necesitas variantes de algo costoso de crear.");
    }
}
