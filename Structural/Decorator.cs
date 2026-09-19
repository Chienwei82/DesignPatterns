namespace DesignPatterns.Structural;

/// PATRÓN DECORATOR
/// ────────────────
/// Permite AÑADIR comportamientos a objetos de forma DINÁMICA,
/// envolviéndolos en objetos "decorador". Es una alternativa flexible
/// a la herencia para extender funcionalidad.
///
/// USO REAL: Middleware en ASP.NET Core (logging, auth, caching),
///           Streams con buffering/compresión en .NET, agregar
///           funcionalidades a objetos GUI.

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

    protected PastelDecorator(IPastel pastel)
    {
        _pastel = pastel;
    }

    public virtual string GetDescripcion() => _pastel.GetDescripcion();
    public virtual decimal GetPrecio() => _pastel.GetPrecio();
}

// --- Decoradores concretos ---
public class ConGlaseado : PastelDecorator
{
    public ConGlaseado(IPastel pastel) : base(pastel) { }

    public override string GetDescripcion() =>
        $"{_pastel.GetDescripcion()} + glaseado de fresa";

    public override decimal GetPrecio() =>
        _pastel.GetPrecio() + 1500m;
}

public class ConChispas : PastelDecorator
{
    public ConChispas(IPastel pastel) : base(pastel) { }

    public override string GetDescripcion() =>
        $"{_pastel.GetDescripcion()} + chispas de chocolate";

    public override decimal GetPrecio() =>
        _pastel.GetPrecio() + 800m;
}

public class ConVelitas : PastelDecorator
{
    public ConVelitas(IPastel pastel) : base(pastel) { }

    public override string GetDescripcion() =>
        $"{_pastel.GetDescripcion()} + velitas de cumpleaños";

    public override decimal GetPrecio() =>
        _pastel.GetPrecio() + 500m;
}

public static class DecoratorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎄 DECORATOR — Añadir funcionalidad dinámicamente\n");
        Console.WriteLine("  Escenario: Pastelería — decora el pastel con extras\n");

        // Pastel básico
        IPastel pastel = new PastelVainilla();
        Console.WriteLine($"  Base: {pastel.GetDescripcion(),-45} ¢{pastel.GetPrecio(),6:N0}");

        // Vamos decorando paso a paso
        pastel = new ConGlaseado(pastel);
        Console.WriteLine($"  +    {pastel.GetDescripcion(),-45} ¢{pastel.GetPrecio(),6:N0}");

        pastel = new ConChispas(pastel);
        Console.WriteLine($"  +    {pastel.GetDescripcion(),-45} ¢{pastel.GetPrecio(),6:N0}");

        pastel = new ConVelitas(pastel);
        Console.WriteLine($"  +    {pastel.GetDescripcion(),-45} ¢{pastel.GetPrecio(),6:N0}");

        Console.WriteLine();
        Console.WriteLine($"  🧾 TOTAL: {pastel.GetDescripcion(),-45} ¢{pastel.GetPrecio(),6:N0}");
        Console.WriteLine();

        // Otra combinación
        Console.WriteLine("  ── Otra combinación: Pastel + chispas + velitas ──");
        IPastel otro = new PastelVainilla();
        otro = new ConChispas(otro);
        otro = new ConVelitas(otro);
        Console.WriteLine($"  {otro.GetDescripcion(),-45} ¢{otro.GetPrecio(),6:N0}");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los decoradores se pueden combinar en cualquier orden.");
        Console.WriteLine("     No necesitas crear N clases por cada combinación posible.");
    }
}
