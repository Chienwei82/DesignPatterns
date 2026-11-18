namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN FACTORY METHOD — Creación delegada a subclases
/// ============================================================================
/// Intención: definir una interfaz para crear objetos, pero dejar que las
/// subclases decidan qué clase concreta instanciar.
///
/// Analogía: franquicia de muebles: el proceso es estándar, pero cada taller
/// decide si usa roble o pino.
///
/// Cuándo usarlo: no sabes de antemano los tipos exactos que crearás; quieres
/// que otros extiendan tu librería sin modificarla.
///
/// Cuándo NO usarlo: si solo hay un tipo de producto y no habrá variaciones,
/// un simple `new` es más simple.
///
/// USO REAL: Frameworks ORM (DbConnection según la BD), generación de
/// documentos (PDF, Excel, Word), factories de controles UI.
/// ============================================================================

// --- Producto abstracto ---
public interface IPlatillo
{
    void Preparar();
}

// --- Productos concretos ---
public class CostillaBBQ : IPlatillo
{
    public void Preparar()
    {
        Console.WriteLine("  🍖 [Parrilla] Costillas BBQ con salsa ahumada");
    }
}

public class Pastel : IPlatillo
{
    public void Preparar()
    {
        Console.WriteLine("  🎂 [Pastelería] Pastel de chocolate recién horneado");
    }
}

public class Limonada : IPlatillo
{
    public void Preparar()
    {
        Console.WriteLine("  🍋 [Bar] Limonada fría con hierbabuena");
    }
}

// --- Creator abstracto ---
public abstract class EstacionCocina
{
    // Factory Method — cada estación decide qué crear
    public abstract IPlatillo CrearPlatillo();

    public void ServirPedido(string cliente)
    {
        Console.WriteLine($"  Pedido de {cliente}:");
        var platillo = CrearPlatillo();
        platillo.Preparar();
    }
}

// --- Creators concretos ---
public class EstacionParrilla : EstacionCocina
{
    public override IPlatillo CrearPlatillo() => new CostillaBBQ();
}

public class EstacionPasteleria : EstacionCocina
{
    public override IPlatillo CrearPlatillo() => new Pastel();
}

public class EstacionBar : EstacionCocina
{
    public override IPlatillo CrearPlatillo() => new Limonada();
}

public static class FactoryMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭 FACTORY METHOD — Cada estación decide qué cocinar\n");
        Console.WriteLine("  Escenario: Cocina con estaciones especializadas\n");

        var estaciones = new (string cliente, EstacionCocina estacion)[]
        {
            ("Mesa 1", new EstacionParrilla()),
            ("Mesa 2", new EstacionPasteleria()),
            ("Mesa 3", new EstacionBar())
        };

        foreach (var (cliente, estacion) in estaciones)
        {
            estacion.ServirPedido(cliente);
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El cliente depende de la abstracción (EstacionCocina),");
        Console.WriteLine("     no de las clases concretas. Para agregar una estación,");
        Console.WriteLine("     solo se crea otra subclase.");
    }
}
