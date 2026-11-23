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
public interface ITransporte
{
    void Entregar(string paquete);
}

// --- Productos concretos ---
public class Moto : ITransporte
{
    public void Entregar(string paquete) =>
        Console.WriteLine("  🏍️ [Moto] Entregando paquete por la ciudad en 20 min");
}

public class Bicicleta : ITransporte
{
    public void Entregar(string paquete) =>
        Console.WriteLine("  🚲 [Bici] Entregando paquete por ciclovía en 35 min");
}

public class Auto : ITransporte
{
    public void Entregar(string paquete) =>
        Console.WriteLine("  🚗 [Auto] Entregando paquete en furgón en 25 min");
}

// --- Creator abstracto ---
public abstract class RepartidorApp
{
    // Factory Method — cada repartidor decide qué transporte usar
    public abstract ITransporte CrearTransporte();

    public void AsignarPedido(string cliente, string paquete)
    {
        Console.WriteLine($"  Pedido de {cliente}: {paquete}");
        var transporte = CrearTransporte();
        transporte.Entregar(paquete);
    }
}

// --- Creators concretos ---
public class RepartidorExpress : RepartidorApp
{
    public override ITransporte CrearTransporte() => new Moto();
}

public class RepartidorEcologico : RepartidorApp
{
    public override ITransporte CrearTransporte() => new Bicicleta();
}

public class RepartidorCarga : RepartidorApp
{
    public override ITransporte CrearTransporte() => new Auto();
}

public static class FactoryMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭 FACTORY METHOD — Cada repartidor decide qué transporte usar\n");
        Console.WriteLine("  Escenario: App de delivery con repartidores especializados\n");

        var repartos = new (string cliente, RepartidorApp repartidor)[]
        {
            ("Sofía", new RepartidorExpress()),
            ("Diego", new RepartidorEcologico()),
            ("Valeria", new RepartidorCarga())
        };

        foreach (var (cliente, repartidor) in repartos)
        {
            repartidor.AsignarPedido(cliente, "Paquete estándar");
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El cliente depende de la abstracción (RepartidorApp),");
        Console.WriteLine("     no de las clases concretas. Para agregar un transporte,");
        Console.WriteLine("     solo se crea otra subclase.");
    }
}
