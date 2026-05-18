namespace DesignPatterns.Behavioral;

/// PATRÓN STRATEGY
/// ───────────────
/// Define una familia de algoritmos, los encapsula y los hace
/// INTERCAMBIABLES. Permite que el algoritmo varíe independientemente
/// del cliente que lo usa. Es como tener varios "motores" que se
/// pueden enchufar según la necesidad.
///
/// USO REAL: Estrategias de ordenamiento, métodos de pago en
///           e-commerce, compresión de archivos, validación de datos.

// --- Strategy ---
public interface ICalculadorImpuesto
{
    string Nombre { get; }
    decimal Calcular(decimal monto);
}

// --- Concrete Strategies ---
public class ImpuestoCostaRica : ICalculadorImpuesto
{
    public string Nombre => "IVA Costa Rica (13%)";

    public decimal Calcular(decimal monto)
    {
        return monto * 0.13m;
    }
}

public class ImpuestoPanama : ICalculadorImpuesto
{
    public string Nombre => "ITBMS Panamá (7%)";

    public decimal Calcular(decimal monto)
    {
        return monto * 0.07m;
    }
}

public class ImpuestoMexico : ICalculadorImpuesto
{
    public string Nombre => "IVA México (16%)";

    public decimal Calcular(decimal monto)
    {
        return monto * 0.16m;
    }
}

public class ImpuestoSinImpuesto : ICalculadorImpuesto
{
    public string Nombre => "Zona Franca (0%)";

    public decimal Calcular(decimal monto) => 0;
}

// --- Context ---
public class Facturador
{
    private ICalculadorImpuesto _estrategia;

    public Facturador(ICalculadorImpuesto estrategia)
    {
        _estrategia = estrategia;
    }

    // ¡Clave del patrón! Podemos cambiar la estrategia en tiempo de ejecución
    public void CambiarEstrategia(ICalculadorImpuesto nuevaEstrategia)
    {
        Console.WriteLine($"  ↪ Cambiando estrategia: {_estrategia.Nombre} → {nuevaEstrategia.Nombre}");
        _estrategia = nuevaEstrategia;
    }

    public void Facturar(string producto, decimal precio)
    {
        var impuesto = _estrategia.Calcular(precio);
        var total = precio + impuesto;

        Console.WriteLine($"  Producto: {producto}");
        Console.WriteLine($"  Precio base:  ¢{precio,10:N2}");
        Console.WriteLine($"  Impuesto ({_estrategia.Nombre}): ¢{impuesto,10:N2}");
        Console.WriteLine($"  ─────────────────────────");
        Console.WriteLine($"  TOTAL:        ¢{total,10:N2}");
    }
}

public static class StrategyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧠 STRATEGY — Algoritmos intercambiables\n");
        Console.WriteLine("  Escenario: Facturación con impuestos según el país\n");

        // Creamos el contexto con una estrategia inicial
        var facturador = new Facturador(new ImpuestoCostaRica());

        // Primera factura — Costa Rica
        Console.WriteLine("  ── Factura #1: 🇨🇷 Cliente en Costa Rica ──");
        facturador.Facturar("Teclado Mecánico", 45_000m);
        Console.WriteLine();

        // Cambiamos la estrategia en tiempo real
        facturador.CambiarEstrategia(new ImpuestoMexico());
        Console.WriteLine("  ── Factura #2: 🇲🇽 Cliente en México ──");
        facturador.Facturar("Monitor 27\"", 95_000m);
        Console.WriteLine();

        // Otra estrategia
        facturador.CambiarEstrategia(new ImpuestoPanama());
        Console.WriteLine("  ── Factura #3: 🇵🇦 Cliente en Panamá ──");
        facturador.Facturar("Laptop", 650_000m);
        Console.WriteLine();

        Console.WriteLine("  ✅ Sin Strategy: if/else por cada país.");
        Console.WriteLine("     Con Strategy: cada país es una clase separada.");
        Console.WriteLine("     Fácil de extender — solo agregas una clase más.");
    }
}
