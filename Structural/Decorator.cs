namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN DECORATOR — Añadir responsabilidades dinámicamente
/// ============================================================================
/// Intención: envolver un objeto con otros que le agregan comportamiento, sin
/// herencia ni modificar la clase original. Los decoradores se apilan.
///
/// Analogía: encantar un arma de un RPG (una espada base) con runas de fuego,
/// hielo y rayo: la espada es la misma, pero cada encantamiento añade poder.
///
/// Cuándo usarlo: cuando tendrías una explosión de subclases (espada con
/// fuego+hielo+rayo...). Es la base de streams y middleware en .NET.
///
/// Cuándo NO usarlo: si solo hay una variante posible, la herencia simple es
/// más directa.
///
/// USO REAL: Middleware en ASP.NET Core (logging, auth, caching), Streams con
/// buffering/compresión en .NET, agregar funcionalidades a objetos GUI.
/// ============================================================================

// --- Componente abstracto ---
public interface IArma
{
    string Nombre { get; }
    int Danio { get; }
    void Atacar();
}

// --- Componente concreto ---
public class EspadaBasica : IArma
{
    public string Nombre => "Espada";
    public int Danio => 10;
    public void Atacar() => Console.WriteLine($"  ⚔️ {Nombre}: golpe básico ({Danio} de daño)");
}

// --- Decorador base ---
public abstract class ArmaDecorator : IArma
{
    protected readonly IArma _arma;

    protected ArmaDecorator(IArma arma) => _arma = arma;

    public abstract string Nombre { get; }
    public abstract int Danio { get; }

    public virtual void Atacar()
    {
        _arma.Atacar();
        Console.WriteLine($"     ✨ {Nombre} añade {Danio - _arma.Danio} de daño → total {Danio}");
    }
}

// --- Decoradores concretos (encantamientos) ---
public class EncantamientoFuego : ArmaDecorator
{
    public EncantamientoFuego(IArma arma) : base(arma) { }
    public override string Nombre => "Encantamiento de Fuego";
    public override int Danio => _arma.Danio + 8;
    public override void Atacar()
    {
        base.Atacar();
        Console.WriteLine("     🔥 ¡La hoja arde en llamas!");
    }
}

public class EncantamientoHielo : ArmaDecorator
{
    public EncantamientoHielo(IArma arma) : base(arma) { }
    public override string Nombre => "Encantamiento de Hielo";
    public override int Danio => _arma.Danio + 5;
    public override void Atacar()
    {
        base.Atacar();
        Console.WriteLine("     ❄️ El enemigo queda ralentizado");
    }
}

public class EncantamientoRayo : ArmaDecorator
{
    public EncantamientoRayo(IArma arma) : base(arma) { }
    public override string Nombre => "Encantamiento de Rayo";
    public override int Danio => _arma.Danio + 12;
    public override void Atacar()
    {
        base.Atacar();
        Console.WriteLine("     ⚡ ¡Descarga en cadena a enemigos cercanos!");
    }
}

public static class DecoratorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎄 DECORATOR — Encantar un arma dinámicamente (RPG)\n");
        Console.WriteLine("  Escenario: Una espada base recibe encantamientos apilados\n");

        IArma arma = new EspadaBasica();
        Mostrar(arma, "Base");

        arma = new EncantamientoFuego(arma);
        Mostrar(arma, "+ fuego");

        arma = new EncantamientoHielo(arma);
        Mostrar(arma, "+ hielo");

        arma = new EncantamientoRayo(arma);
        Mostrar(arma, "+ rayo");

        Console.WriteLine();
        Console.WriteLine($"  🧾 Arma final: {arma.GetType().Name} (daño {arma.Danio})");
        Console.WriteLine();

        // Otra combinación (rayo + fuego)
        Console.WriteLine("  ── Otra combinación: Espada + rayo + fuego ──");
        IArma otra = new EncantamientoFuego(new EncantamientoRayo(new EspadaBasica()));
        Mostrar(otra, "Otra");

        Console.WriteLine("  ✅ Los encantamientos se combinan en cualquier orden.");
        Console.WriteLine("     No necesitas una clase por cada combinación posible.");
    }

    static void Mostrar(IArma arma, string etiqueta)
    {
        Console.WriteLine($"  [{etiqueta}]");
        arma.Atacar();
        Console.WriteLine();
    }
}
