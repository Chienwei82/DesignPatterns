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

// ICloneable es la interfaz nativa de .NET para clonación

/// --- Prototipo concreto: Factura ---
public class Factura : ICloneable
{
    public string NumeroFactura { get; set; }
    public string Cliente { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public List<string> Lineas { get; set; } = [];
    public DatosFiscales DatosFiscales { get; set; }

    public Factura(string numero, string cliente)
    {
        NumeroFactura = numero;
        Cliente = cliente;
        Fecha = DateTime.Now;
        DatosFiscales = new DatosFiscales();
    }

    // Clone() crea una copia superficial (shallow copy)
    // Los objetos de referencia (List, DatosFiscales) se COMPARTEN
    public object Clone()
    {
        return MemberwiseClone();
    }

    // DeepClone() crea una copia profunda — TODO es nuevo
    public Factura DeepClone()
    {
        var copia = (Factura)MemberwiseClone();
        copia.Lineas = new List<string>(Lineas);
        copia.DatosFiscales = new DatosFiscales
        {
            Cedula = DatosFiscales.Cedula,
            Nombre = DatosFiscales.Nombre,
            Telefono = DatosFiscales.Telefono
        };
        return copia;
    }

    public void Mostrar()
    {
        Console.WriteLine($"  Factura #{NumeroFactura}");
        Console.WriteLine($"  Cliente: {Cliente}");
        Console.WriteLine($"  Fecha:   {Fecha:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"  Total:   ¢{Total:N2}");
        Console.WriteLine($"  Líneas:  {string.Join(", ", Lineas)}");
        Console.WriteLine($"  Cédula:  {DatosFiscales.Cedula}");
    }
}

public record DatosFiscales(string Cedula = "", string Nombre = "", string Telefono = "");

public static class PrototypeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧬 PROTOTYPE — Clonación de objetos\n");
        Console.WriteLine("  Escenario: Sistema de facturación — crear facturas\n" +
                         "  similares a partir de una plantilla\n");

        // ── Crear factura original (prototipo base) ──
        var facturaBase = new Factura("F001-00001", "Comercial XYZ")
        {
            Total = 125000.00m,
            Lineas = ["Laptop Dell XPS 13", "Mouse Bluetooth"],
            DatosFiscales = new DatosFiscales
            {
                Cedula = "3-101-234567",
                Nombre = "Comercial XYZ S.A.",
                Telefono = "2256-7890"
            }
        };

        Console.WriteLine("  📄 FACTURA ORIGINAL (prototipo):");
        facturaBase.Mostrar();
        Console.WriteLine();

        // ── Clon superficial ──
        Console.WriteLine("  ── CLON SUPERFICIAL (shallow copy) ──");
        var facturaClon = (Factura)facturaBase.Clone();
        facturaClon.NumeroFactura = "F001-00002";
        facturaClon.Cliente = "Tienda ABC (copia)";
        facturaClon.Total = 85000.00m;
        facturaClon.Lineas.Add("Monitor 24\""); // ⚠️ ¡Agrega a la LISTA COMPARTIDA!
        Console.WriteLine("  (modificamos líneas en el clon...)");
        Console.WriteLine();

        Console.WriteLine("  📄 Factura ORIGINAL después de modificar el clon:");
        facturaBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ⚠️  ¡La línea 'Monitor 24\"' apareció en la original!");
        Console.WriteLine("     Porque el clon superficial comparte la lista.\n");

        // ── Clon profundo ──
        Console.WriteLine("  ── CLON PROFUNDO (deep copy) ──");
        var facturaDeep = facturaBase.DeepClone();
        facturaDeep.NumeroFactura = "F001-00003";
        facturaDeep.Cliente = "Nuevo Cliente (deep)";
        facturaDeep.Total = 95000.00m;
        facturaDeep.Lineas.Add("Teclado Mecánico"); // solo en el deep clone
        Console.WriteLine("  (modificamos líneas en el deep clone...)");
        Console.WriteLine();

        Console.WriteLine("  📄 Factura ORIGINAL (sin cambios esta vez):");
        facturaBase.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  📄 Deep Clone (modificado):");
        facturaDeep.Mostrar();
        Console.WriteLine();
        Console.WriteLine("  ✅ Con deep copy, las modificaciones no afectan al original.");
        Console.WriteLine("     Útil cuando necesitas variantes de un objeto costoso de crear.");
    }
}
