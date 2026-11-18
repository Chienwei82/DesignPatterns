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

// --- Prototipo concreto: Receta ---
public class Receta : ICloneable
{
    public string Nombre { get; set; } = "";
    public int Porciones { get; set; }
    public List<string> Ingredientes { get; set; } = [];
    public Chef Chef { get; set; } = new();

    public Receta(string nombre, int porciones)
    {
        Nombre = nombre;
        Porciones = porciones;
        Chef = new Chef();
    }

    // Clone(): copia superficial (shallow) — listas y objetos se COMPARTEN
    public object Clone() => MemberwiseClone();

    // DeepClone(): copia profunda — TODO se duplica
    public Receta DeepClone()
    {
        var copia = (Receta)MemberwiseClone();
        copia.Ingredientes = new List<string>(Ingredientes);
        copia.Chef = Chef with { }; // record inmutable: `with` crea uno nuevo
        return copia;
    }

    public void Mostrar()
    {
        Console.WriteLine($"  Receta:      {Nombre}");
        Console.WriteLine($"  Porciones:   {Porciones}");
        Console.WriteLine($"  Ingredientes: {string.Join(", ", Ingredientes)}");
        Console.WriteLine($"  Chef:        {Chef.Nombre} ({Chef.Restaurante})");
    }
}

public record Chef(string Nombre = "", string Restaurante = "", string Telefono = "");

public static class PrototypeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧬 PROTOTYPE — Clonar recetas\n");
        Console.WriteLine("  Escenario: El chef clona su receta maestra para crear variantes\n" +
                         "  sin empezar de cero\n");

        // ── Receta original (prototipo base) ──
        var recetaBase = new Receta("Salsa de la casa", 4)
        {
            Ingredientes = ["tomate", "cebolla", "culantro"],
            Chef = new Chef
            {
                Nombre = "Chef Ramírez",
                Restaurante = "La Cocina Caótica",
                Telefono = "2256-7890"
            }
        };

        Console.WriteLine("  📄 RECETA ORIGINAL (prototipo):");
        recetaBase.Mostrar();
        Console.WriteLine();

        // ── Clon superficial: la lista se COMPARTE ──
        Console.WriteLine("  ── CLON SUPERFICIAL (shallow copy) ──");
        var recetaClon = (Receta)recetaBase.Clone();
        recetaClon.Nombre = "Salsa picante (copia)";
        recetaClon.Porciones = 8;
        recetaClon.Ingredientes.Add("chile habanero"); // ⚠️ afecta al original
        Console.WriteLine("  (le agregamos chile a la copia...)");
        Console.WriteLine();

        Console.WriteLine("  📄 RECETA ORIGINAL después de modificar la copia:");
        recetaBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ⚠️  ¡El 'chile habanero' apareció en la original!");
        Console.WriteLine("     El clon superficial comparte la lista.\n");

        // ── Clon profundo: todo se duplica ──
        Console.WriteLine("  ── CLON PROFUNDO (deep copy) ──");
        var recetaDeep = recetaBase.DeepClone();
        recetaDeep.Nombre = "Salsa suave (deep)";
        recetaDeep.Porciones = 6;
        recetaDeep.Ingredientes.Add("crema"); // solo en el deep clone
        Console.WriteLine("  (le agregamos crema a la copia profunda...)");
        Console.WriteLine();

        Console.WriteLine("  📄 RECETA ORIGINAL (sin cambios):");
        recetaBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  📄 Copia profunda (modificada):");
        recetaDeep.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ✅ Con deep copy, las modificaciones no afectan al original.");
        Console.WriteLine("     Útil cuando necesitas variantes de algo costoso de crear.");
    }
}
