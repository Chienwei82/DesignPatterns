namespace DesignPatterns.Creational;

/// PATRÓN ABSTRACT FACTORY
/// ────────────────────────
/// Proporciona una interfaz para crear FAMILIAS de objetos relacionados
/// sin especificar sus clases concretas. Es como un Factory Method
/// pero para crear múltiples productos que funcionan juntos.
///
/// USO REAL: Interfaces UI multiplataforma, conectores a distintos
///           motores de BD, menús completos de distintas cocinas.

// --- Productos abstractos ---
public interface IEntrada
{
    void Servir();
}

public interface IPlatoFuerte
{
    void Servir();
}

// --- Familia: Menú Italiano ---
public class Bruschetta : IEntrada
{
    public void Servir()
    {
        Console.WriteLine("  [🇮🇹 Entrada] Bruschetta con tomate y albahaca");
    }
}

public class Pizza : IPlatoFuerte
{
    public void Servir()
    {
        Console.WriteLine("  [🇮🇹 Fuerte]  Pizza margarita recién salida del horno");
    }
}

// --- Familia: Menú Mexicano ---
public class Nachos : IEntrada
{
    public void Servir()
    {
        Console.WriteLine("  [🇲🇽 Entrada] Nachos con guacamole y jalapeños");
    }
}

public class Tacos : IPlatoFuerte
{
    public void Servir()
    {
        Console.WriteLine("  [🇲🇽 Fuerte]  Tacos al pastor con piña");
    }
}

// --- Abstract Factory ---
public interface IMenuFactory
{
    IEntrada CrearEntrada();
    IPlatoFuerte CrearPlatoFuerte();
}

// --- Factories concretas ---
public class MenuItalianoFactory : IMenuFactory
{
    public IEntrada CrearEntrada() => new Bruschetta();
    public IPlatoFuerte CrearPlatoFuerte() => new Pizza();
}

public class MenuMexicanoFactory : IMenuFactory
{
    public IEntrada CrearEntrada() => new Nachos();
    public IPlatoFuerte CrearPlatoFuerte() => new Tacos();
}

// --- Cliente ---
public static class AbstractFactoryDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭🏭 ABSTRACT FACTORY — Familias de platillos que combinan\n");
        Console.WriteLine("  Escenario: El restaurante arma menús completos por cocina\n");

        var menus = new (string nombre, IMenuFactory factory)[]
        {
            ("Italiano", new MenuItalianoFactory()),
            ("Mexicano", new MenuMexicanoFactory())
        };

        foreach (var (nombre, factory) in menus)
        {
            Console.WriteLine($"  ── MENÚ {nombre.ToUpper()} ──");

            // El cliente usa la factory sin saber qué clases concretas se crean
            var entrada = factory.CrearEntrada();
            var fuerte = factory.CrearPlatoFuerte();

            entrada.Servir();
            fuerte.Servir();
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Los platillos de cada familia son compatibles entre sí.");
        Console.WriteLine("  ✅ Cambiar de cocina = cambiar de factory. El cliente no cambia.");
    }
}
