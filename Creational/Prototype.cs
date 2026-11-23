namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN PROTOTYPE — Clonar objetos existentes
/// ============================================================================
/// Intención: crear nuevos objetos copiando un prototipo existente, en vez de
/// construirlos desde cero. La copia puede ser superficial (comparte
/// referencias) o profunda (todo nuevo).
///
/// Analogía: los soldados clon de Star Wars se producen copiando un "prototipo"
/// (el clon original). Un clon superficial comparte equipo; uno profundo es
/// totalmente independiente.
///
/// Cuándo usarlo: duplicar documentos plantilla, crear muchos enemigos
/// similares en un videojuego.
///
/// Cuándo NO usarlo: cuando el objeto es simple y barato de crear con `new`.
///
/// Cuidado: diferencia copia superficial (shallow: comparte listas) de copia
/// profunda (deep: duplica todo).
///
/// USO REAL: Clonación de configuraciones, duplicación de documentos,
/// creación de enemigos en videojuegos, evitar costos de inicialización.
/// ============================================================================

// --- Prototipo concreto: Soldado Clon ---
public class SoldadoClon : ICloneable
{
    public string Nombre { get; set; } = "";
    public string Designacion { get; set; } = "";
    public List<string> Equipo { get; set; } = [];
    public RegistroAD Registro { get; set; } = new();

    public SoldadoClon(string designacion)
    {
        Designacion = designacion;
        Registro = new RegistroAD();
    }

    // Clone(): copia superficial (shallow) — listas y objetos se COMPARTEN
    public object Clone() => MemberwiseClone();

    // DeepClone(): copia profunda — TODO se duplica
    public SoldadoClon DeepClone()
    {
        var copia = (SoldadoClon)MemberwiseClone();
        copia.Equipo = new List<string>(Equipo);
        copia.Registro = Registro with { }; // record inmutable: `with` crea uno nuevo
        return copia;
    }

    public void Mostrar()
    {
        Console.WriteLine($"  Clon:       {Nombre}");
        Console.WriteLine($"  Designación:{Designacion}");
        Console.WriteLine($"  Equipo:     {(Equipo.Count > 0 ? string.Join(", ", Equipo) : "(ninguno)")}");
        Console.WriteLine($"  AD:         {Registro.Codigo} · origen {Registro.PlanetaOrigen}");
    }
}

public record RegistroAD(string Codigo = "", string PlanetaOrigen = "");

public static class PrototypeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧬 PROTOTYPE — Clonar soldados plantilla (Star Wars)\n");
        Console.WriteLine("  Escenario: Los kaminoanos clonan al soldado máster original para formar un ejército\n");

        // ── Soldado original (prototipo base) ──
        var original = new SoldadoClon("CT-original")
        {
            Nombre = "CT-7567 'Rex'",
            Equipo = ["Blaster DC-15", "Armadura Phase I", "Comlink"],
            Registro = new RegistroAD { Codigo = "JADE-SHAWEPI", PlanetaOrigen = "Kamino" }
        };

        Console.WriteLine("  🧬 SOLDADO ORIGINAL (prototipo):");
        original.Mostrar();
        Console.WriteLine();

        // ── Clon superficial: el equipo se COMPARTE ──
        Console.WriteLine("  ── CLON SUPERFICIAL (shallow copy) ──");
        var clon = (SoldadoClon)original.Clone();
        clon.Nombre = "CT-0000 'Echo'";
        clon.Equipo.Add("Casco extra"); // ⚠️ afecta al original
        Console.WriteLine("  (le agregamos 'Casco extra' al clon...)");
        Console.WriteLine();

        Console.WriteLine("  🧬 SOLDADO ORIGINAL después de modificar el clon:");
        original.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ⚠️  ¡El 'Casco extra' apareció en el original!");
        Console.WriteLine("     El clon superficial comparte la lista de equipo.\n");

        // ── Clon profundo: todo se duplica ──
        Console.WriteLine("  ── CLON PROFUNDO (deep copy) ──");
        var clonDeep = original.DeepClone();
        clonDeep.Nombre = "CT-5597 'Fives'";
        clonDeep.Equipo.Add("Medpack"); // solo en el deep clone
        clonDeep.Registro = clonDeep.Registro with { Codigo = "CTI-5597" }; // AD independiente
        Console.WriteLine("  (le agregamos 'Medpack' y AD propio al clon profundo...)");
        Console.WriteLine();

        Console.WriteLine("  🧬 SOLDADO ORIGINAL (sin cambios):")
        ;
        original.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  🧬 Clon profundo (independiente):")
        ;
        clonDeep.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ✅ Con deep copy, las modificaciones no afectan al original.");
        Console.WriteLine("     Útil cuando necesitas variantes de algo costoso de crear.");
    }
}
