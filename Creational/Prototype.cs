namespace DesignPatterns.Creational;

/// PATRÓN PROTOTYPE
/// ────────────────
/// Permite copiar objetos existentes sin que el código dependa de
/// sus clases concretas. La copia puede ser superficial (shallow)
/// o profunda (deep). Clave: el objeto original sirve como "prototipo"
/// del cual se clonan nuevas instancias.
///
/// USO REAL: Clonación de configuraciones, duplicación de documentos,
///           creación de enemigos en videojuegos, evitar costos de
///           inicialización pesada.

// ICloneable es la interfaz nativa de .NET para clonación,
// aunque devuelve object (no es genérica) y hoy se prefiere
// un método propio como DeepClone().

/// --- Prototipo concreto: Receta ---
public class Receta : ICloneable
{
    public string Nombre { get; set; }
    public int Porciones { get; set; }
    public List<string> Ingredientes { get; set; } = [];
    public Chef Chef { get; set; }

    public Receta(string nombre, int porciones)
    {
        Nombre = nombre;
        Porciones = porciones;
        Chef = new Chef();
    }

    // Clone() crea una copia superficial (shallow copy)
    // Los objetos de referencia (List, Chef) se COMPARTEN
    public object Clone()
    {
        return MemberwiseClone();
    }

    // DeepClone() crea una copia profunda — TODO es nuevo
    public Receta DeepClone()
    {
        var copia = (Receta)MemberwiseClone();
        copia.Ingredientes = new List<string>(Ingredientes);
        copia.Chef = Chef with { }; // los records son inmutables: with crea uno nuevo
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
        Console.WriteLine("  Escenario: El chef clona su receta maestra de salsa\n" +
                         "  para crear variantes sin empezar de cero\n");

        // ── Crear receta original (prototipo base) ──
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

        // ── Clon superficial ──
        Console.WriteLine("  ── CLON SUPERFICIAL (shallow copy) ──");
        var recetaClon = (Receta)recetaBase.Clone();
        recetaClon.Nombre = "Salsa picante (copia)";
        recetaClon.Porciones = 8;
        recetaClon.Ingredientes.Add("chile habanero"); // ⚠️ ¡Agrega a la LISTA COMPARTIDA!
        Console.WriteLine("  (le agregamos chile a la copia...)");
        Console.WriteLine();

        Console.WriteLine("  📄 RECETA ORIGINAL después de modificar la copia:");
        recetaBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ⚠️  ¡El 'chile habanero' apareció en la receta original!");
        Console.WriteLine("     Porque el clon superficial comparte la lista.\n");

        // ── Clon profundo ──
        Console.WriteLine("  ── CLON PROFUNDO (deep copy) ──");
        var recetaDeep = recetaBase.DeepClone();
        recetaDeep.Nombre = "Salsa suave (deep)";
        recetaDeep.Porciones = 6;
        recetaDeep.Ingredientes.Add("crema"); // solo en el deep clone
        Console.WriteLine("  (le agregamos crema a la copia profunda...)");
        Console.WriteLine();

        Console.WriteLine("  📄 RECETA ORIGINAL (sin cambios esta vez):");
        recetaBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  📄 Copia profunda (modificada):");
        recetaDeep.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ✅ Con deep copy, las modificaciones no afectan al original.");
        Console.WriteLine("     Útil cuando necesitas variantes de algo costoso de crear.");
    }
}
