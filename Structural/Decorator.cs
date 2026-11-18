namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN DECORATOR — Añadir responsabilidades dinámicamente
/// ============================================================================
/// Intención: envolver un objeto con otros que le agregan comportamiento, sin
/// herencia ni modificar la clase original. Los decoradores se apilan.
///
/// Analogía: vestirse en capas: camiseta, suéter, abrigo — la persona es la
/// misma, se agregan capas.
///
/// Cuándo usarlo: cuando tendrías una explosión de subclases (pastel con
/// glaseado+chispas+velitas...). Es la base de streams y middleware en .NET.
///
/// Cuándo NO usarlo: si solo hay una variante posible, la herencia simple es
/// más directa.
///
/// USO REAL: Middleware en ASP.NET Core (logging, auth, caching), Streams con
/// buffering/compresión en .NET, agregar funcionalidades a objetos GUI.
/// ============================================================================

// --- Componente abstracto ---
public interface IPastel
{
    string GetDescripcion();
    decimal GetPrecio();
}

// --- Componente concreto ---
public class PastelVainilla : IPastel
{
    public string GetDescripcion() => "Pastel de vainilla";
    public decimal GetPrecio() => 8000m;
}

// --- Decorador base ---
public abstract class PastelDecorator : IPastel
{
    protected readonly IPastel _pastel;

    protected PastelDecorator(IPastel pastel) => _pastel = pastel;

    public virtual string GetDescripcion() => _pastel.GetDescripcion();
    public virtual decimal GetPrecio() => _pastel.GetPrecio();
}

// --- Decoradores concretos ---
public class ConGlaseado : PastelDecorator
{
    public ConGlaseado(IPastel pastel) : base(pastel) { }
    public override string GetDescripcion() => $"{_pastel.GetDescripcion()} + glaseado de fresa";
    public override decimal GetPrecio() => _pastel.GetPrecio() + 1500m;
}

public class ConChispas : PastelDecorator
{
    public ConChispas(IPastel pastel) : base(pastel) { }
    public override string GetDescripcion() => $"{_pastel.GetDescripcion()} + chispas de chocolate";
    public override decimal GetPrecio() => _pastel.GetPrecio() + 800m;
}

public class ConVelitas : PastelDecorator
{
    public ConVelitas(IPastel pastel) : base(pastel) { }
    public override string GetDescripcion() => $"{_pastel.GetDescripcion()} + velitas de cumpleaños";
    public override decimal GetPrecio() => _pastel.GetPrecio() + 500m;
}

public static class DecoratorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎄 DECORATOR — Añadir funcionalidad dinámicamente\n");
        Console.WriteLine("  Escenario: Pastelería — decora el pastel con extras\n");

        // Pastel básico
        IPastel pastel = new PastelVainilla();
        Mostrar(pastel, "Base");

        // Vamos decorando paso a paso
        pastel = new ConGlaseado(pastel);
        Mostrar(pastel, "+ glaseado");

        pastel = new ConChispas(pastel);
        Mostrar(pastel, "+ chispas");

        pastel = new ConVelitas(pastel);
        Mostrar(pastel, "+ velitas");

        Console.WriteLine();
        Console.WriteLine($"  🧾 TOTAL: {pastel.GetDescripcion(),-45} ¢{pastel.GetPrecio(),6:N0}");
        Console.WriteLine();

        // Otra combinación (chispas + velitas, sin glaseado)
        Console.WriteLine("  ── Otra combinación: Pastel + chispas + velitas ──");
        IPastel otro = new ConVelitas(new ConChispas(new PastelVainilla()));
        Console.WriteLine($"  {otro.GetDescripcion(),-45} ¢{otro.GetPrecio(),6:N0}");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los decoradores se pueden combinar en cualquier orden.");
        Console.WriteLine("     No necesitas crear N clases por cada combinación posible.");
    }

    static void Mostrar(IPastel p, string etiqueta) =>
        Console.WriteLine($"  {etiqueta,-12} {p.GetDescripcion(),-45} ¢{p.GetPrecio(),6:N0}");
}
