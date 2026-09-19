namespace DesignPatterns.Creational;

/// PATRÓN FACTORY METHOD
/// ─────────────────────
/// Define una interfaz para crear objetos, pero DELEGA a las subclases
/// qué clase concreta instanciar. Permite que una clase difiera la
/// creación de sus objetos a las subclases.
///
/// USO REAL: Frameworks ORM (DbConnection según la BD), generación de
///           documentos (PDF, Excel, Word), factories de controles UI.

// --- Producto abstracto ---
public interface IPlatillo
{
    void Preparar();
}

// --- Productos concretos: cada estación crea el suyo ---
public class CostillaBBQ : IPlatillo
{
    public void Preparar()
    {
        Console.WriteLine("  🍖 [Parrilla]    Costillas BBQ con salsa ahumada");
    }
}

public class Pastel : IPlatillo
{
    public void Preparar()
    {
        Console.WriteLine("  🎂 [Pastelería]  Pastel de chocolate recién horneado");
    }
}

public class Limonada : IPlatillo
{
    public void Preparar()
    {
        Console.WriteLine("  🍋 [Bar]         Limonada fría con hierbabuena");
    }
}

// --- Creator abstracto (Factory Method) ---
public abstract class EstacionCocina
{
    // Factory Method — cada estación decide qué platillo crear
    public abstract IPlatillo CrearPlatillo();

    // Método que usa el producto creado
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
        Console.WriteLine("  Escenario: Cocina de restaurante con estaciones especializadas\n");

        // El cliente no sabe qué clase concreta se crea
        var estaciones = new (string nombre, EstacionCocina estacion)[]
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
        Console.WriteLine("     no de las clases concretas. Agregar una estación nueva");
        Console.WriteLine("     solo requiere crear otra subclase.");
    }
}
