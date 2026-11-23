namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN ABSTRACT FACTORY — Familias de objetos compatibles
/// ============================================================================
/// Intención: crear FAMILIAS de objetos relacionados sin acoplarse a sus
/// clases concretas.
///
/// Analogía: en Hogwarts cada casa tiene un conjunto a juego (estandarte,
/// mascota, sala común, colores). La fábrica de Gryffindor produce el set rojo
/// y dorado; la de Slytherin, el verde y plateado. Nunca mezclas piezas.
///
/// Cuándo usarlo: familias de objetos que deben usarse juntos (temas de UI,
/// conectores a distintas bases de datos, kits multiplataforma).
///
/// Cuándo NO usarlo: si solo hay un producto por familia (para eso es
/// Factory Method, más simple).
///
/// USO REAL: Interfaces UI multiplataforma, conectores a distintos motores
/// de BD, temas visuales completos.
/// ============================================================================

// --- Productos abstractos ---
public interface IEstandarte { void Mostrar(); }
public interface IMascota { void Mostrar(); }
public interface ISalaComun { void Mostrar(); }

// --- Familia: Gryffindor ---
public class EstandarteGryffindor : IEstandarte
{
    public void Mostrar() => Console.WriteLine("  [🦁 Estandarte] rojo y dorado con un león");
}

public class MascotaGryffindor : IMascota
{
    public void Mostrar() => Console.WriteLine("  [🦁 Mascota] el León, símbolo de valor");
}

public class SalaGryffindor : ISalaComun
{
    public void Mostrar() => Console.WriteLine("  [🏰 Sala común] Torre de Gryffindor, chimenea acogedora");
}

// --- Familia: Slytherin ---
public class EstandarteSlytherin : IEstandarte
{
    public void Mostrar() => Console.WriteLine("  [🐍 Estandarte] verde y plateado con una serpiente");
}

public class MascotaSlytherin : IMascota
{
    public void Mostrar() => Console.WriteLine("  [🐍 Mascota] la Serpiente, símbolo de astucia");
}

public class SalaSlytherin : ISalaComun
{
    public void Mostrar() => Console.WriteLine("  [🌊 Sala común] Mazmorra bajo el lago, luz verdosa");
}

// --- Abstract Factory ---
public interface ICasaHogwartsFactory
{
    IEstandarte CrearEstandarte();
    IMascota CrearMascota();
    ISalaComun CrearSalaComun();
}

// --- Factories concretas ---
public class GryffindorFactory : ICasaHogwartsFactory
{
    public IEstandarte CrearEstandarte() => new EstandarteGryffindor();
    public IMascota CrearMascota() => new MascotaGryffindor();
    public ISalaComun CrearSalaComun() => new SalaGryffindor();
}

public class SlytherinFactory : ICasaHogwartsFactory
{
    public IEstandarte CrearEstandarte() => new EstandarteSlytherin();
    public IMascota CrearMascota() => new MascotaSlytherin();
    public ISalaComun CrearSalaComun() => new SalaSlytherin();
}

public static class AbstractFactoryDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏰 ABSTRACT FACTORY — Familias de piezas que combinan (casas de Hogwarts)\n");
        Console.WriteLine("  Escenario: El Sombrero Seleccionador arma el set completo de cada casa\n");

        var casas = new (string nombre, ICasaHogwartsFactory factory)[]
        {
            ("Gryffindor", new GryffindorFactory()),
            ("Slytherin", new SlytherinFactory())
        };

        foreach (var (nombre, factory) in casas)
        {
            Console.WriteLine($"  ── CASA {nombre.ToUpper()} ──");

            // El cliente usa la factory sin saber qué clases concretas se crean
            factory.CrearEstandarte().Mostrar();
            factory.CrearMascota().Mostrar();
            factory.CrearSalaComun().Mostrar();
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Las piezas de cada familia combinan entre sí.");
        Console.WriteLine("  ✅ Cambiar de casa = cambiar de factory. El cliente no cambia.");
    }
}
