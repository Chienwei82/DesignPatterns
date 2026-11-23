namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN STRATEGY — Algoritmos intercambiables
/// ============================================================================
/// Intención: definir una familia de algoritmos, encapsularlos y hacerlos
/// intercambiables. Permite que el algoritmo varíe independientemente del
/// cliente que lo usa.
///
/// Analogía: elegir cómo ir al trabajo: auto, bici o bus. El destino es el
/// mismo; el medio cambia.
///
/// Cuándo usarlo: varios "motores" intercambiables (impuestos por país, métodos
/// de pago, compresión).
///
/// Cuándo NO usarlo: si el algoritmo nunca cambia, inyectarlo es complejidad
/// gratis.
///
/// USO REAL: Estrategias de ordenamiento, métodos de pago en e-commerce,
/// compresión de archivos, cálculo de descuentos.
/// ============================================================================

// --- Strategy ---
public interface IEnvioStrategy
{
    string Nombre { get; }
    decimal Calcular(decimal subtotal, decimal pesoKg);
}

// --- Estrategias concretas ---
public class EnvioEstandar : IEnvioStrategy
{
    public string Nombre => "Estándar (5-7 días)";
    public decimal Calcular(decimal subtotal, decimal pesoKg) => subtotal * 0.05m + pesoKg * 0.5m;
}

public class EnvioExpress : IEnvioStrategy
{
    public string Nombre => "Express (1-2 días)";
    public decimal Calcular(decimal subtotal, decimal pesoKg) => subtotal * 0.15m + pesoKg * 1.0m;
}

public class RecogerEnTienda : IEnvioStrategy
{
    public string Nombre => "Recoger en tienda (gratis)";
    public decimal Calcular(decimal subtotal, decimal pesoKg) => 0m;
}

// --- Context ---
public class CalculadoraEnvio
{
    private IEnvioStrategy _estrategia;

    public CalculadoraEnvio(IEnvioStrategy estrategia) => _estrategia = estrategia;

    // Clave del patrón: cambiar la estrategia en tiempo de ejecución
    public void CambiarEstrategia(IEnvioStrategy nuevaEstrategia)
    {
        Console.WriteLine($"  ↪ Cambiando estrategia: {_estrategia.Nombre} → {nuevaEstrategia.Nombre}");
        _estrategia = nuevaEstrategia;
    }

    public void Cotizar(string producto, decimal subtotal, decimal pesoKg)
    {
        var costo = _estrategia.Calcular(subtotal, pesoKg);

        Console.WriteLine($"  Producto: {producto}");
        Console.WriteLine($"  Subtotal:   ¢{subtotal,10:N2}");
        Console.WriteLine($"  Envío ({_estrategia.Nombre}): ¢{costo,10:N2}");
        Console.WriteLine("  ─────────────────────────");
        Console.WriteLine($"  TOTAL:      ¢{subtotal + costo,10:N2}");
    }
}

public static class StrategyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧠 STRATEGY — Algoritmos intercambiables\n");
        Console.WriteLine("  Escenario: La app calcula el costo de envío según el método elegido\n");

        var calc = new CalculadoraEnvio(new EnvioEstandar());

        Console.WriteLine("  ── Envío #1: método estándar ──");
        calc.Cotizar("Teclado mecánico", 5000m, 1.5m);
        Console.WriteLine();

        calc.CambiarEstrategia(new EnvioExpress());
        Console.WriteLine("  ── Envío #2: el cliente paga por rapidez ──");
        calc.Cotizar("Monitor 4K", 15000m, 5.0m);
        Console.WriteLine();

        calc.CambiarEstrategia(new RecogerEnTienda());
        Console.WriteLine("  ── Envío #3: recoger en tienda ──");
        calc.Cotizar("Laptop gaming", 45000m, 2.5m);
        Console.WriteLine();

        Console.WriteLine("  ✅ Sin Strategy: if/else por cada tipo de envío.");
        Console.WriteLine("     Con Strategy: cada método de envío es una clase separada.");
        Console.WriteLine("     Fácil de extender — solo agregas una clase más.");
    }
}
