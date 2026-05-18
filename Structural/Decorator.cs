namespace DesignPatterns.Structural;

/// PATRÓN DECORATOR
/// ────────────────
/// Permite AÑADIR comportamientos a objetos de forma DINÁMICA,
/// envolviéndolos en objetos "decorador". Es una alternativa flexible
/// a la herencia para extender funcionalidad.
///
/// USO REAL: Middleware en ASP.NET Core (logging, auth, caching),
///           Streams con buffering/compresión en .NET, agregar
///           funcionalidades a objetos GUI, notificaciones con
///           canales adicionales.

// --- Componente abstracto ---
public interface ICafe
{
    string GetDescripcion();
    decimal GetCosto();
}

// --- Componente concreto ---
public class CafeSimple : ICafe
{
    public string GetDescripcion() => "Café simple";
    public decimal GetCosto() => 1500m; // ¢1,500
}

// --- Decorador base ---
public abstract class CafeDecorator : ICafe
{
    protected ICafe _cafe;

    public CafeDecorator(ICafe cafe)
    {
        _cafe = cafe;
    }

    public virtual string GetDescripcion() => _cafe.GetDescripcion();
    public virtual decimal GetCosto() => _cafe.GetCosto();
}

// --- Decoradores concretos ---
public class ConLeche : CafeDecorator
{
    public ConLeche(ICafe cafe) : base(cafe) { }

    public override string GetDescripcion() =>
        $"{_cafe.GetDescripcion()} + Leche";

    public override decimal GetCosto() =>
        _cafe.GetCosto() + 500m;
}

public class ConCrema : CafeDecorator
{
    public ConCrema(ICafe cafe) : base(cafe) { }

    public override string GetDescripcion() =>
        $"{_cafe.GetDescripcion()} + Crema batida";

    public override decimal GetCosto() =>
        _cafe.GetCosto() + 800m;
}

public class ConCaramelo : CafeDecorator
{
    public ConCaramelo(ICafe cafe) : base(cafe) { }

    public override string GetDescripcion() =>
        $"{_cafe.GetDescripcion()} + Shot de caramelo";

    public override decimal GetCosto() =>
        _cafe.GetCosto() + 400m;
}

public class ConCanela : CafeDecorator
{
    public ConCanela(ICafe cafe) : base(cafe) { }

    public override string GetDescripcion() =>
        $"{_cafe.GetDescripcion()} + Canela espolvoreada";

    public override decimal GetCosto() =>
        _cafe.GetCosto() + 200m;
}

public static class DecoratorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎄 DECORATOR — Añadir funcionalidad dinámicamente\n");
        Console.WriteLine("  Escenario: Cafetería — arma tu café con extras\n");

        // Café básico
        ICafe cafe = new CafeSimple();
        Console.WriteLine($"  Base: {cafe.GetDescripcion(),-35} ¢{cafe.GetCosto(),6:N0}");

        // Vamos decorando paso a paso
        cafe = new ConLeche(cafe);
        Console.WriteLine($"  +    {cafe.GetDescripcion(),-35} ¢{cafe.GetCosto(),6:N0}");

        cafe = new ConCrema(cafe);
        Console.WriteLine($"  +    {cafe.GetDescripcion(),-35} ¢{cafe.GetCosto(),6:N0}");

        cafe = new ConCaramelo(cafe);
        Console.WriteLine($"  +    {cafe.GetDescripcion(),-35} ¢{cafe.GetCosto(),6:N0}");

        Console.WriteLine();
        Console.WriteLine($"  🧾 TOTAL: {cafe.GetDescripcion(),-35} ¢{cafe.GetCosto(),6:N0}");
        Console.WriteLine();

        // Otra combinación
        Console.WriteLine("  ── Otra combinación: Café + Crema + Canela ──");
        ICafe otro = new CafeSimple();
        otro = new ConCrema(otro);
        otro = new ConCanela(otro);
        Console.WriteLine($"  {otro.GetDescripcion(),-40} ¢{otro.GetCosto(),6:N0}");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los decoradores se pueden combinar en cualquier orden.");
        Console.WriteLine("     No necesitas crear N clases por cada combinación posible.");
    }
}
