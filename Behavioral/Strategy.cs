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
public interface IEstrategiaDescuento
{
    string Nombre { get; }
    decimal Calcular(decimal subtotal);
}

// --- Estrategias concretas ---
public class SinDescuento : IEstrategiaDescuento
{
    public string Nombre => "Sin descuento";
    public decimal Calcular(decimal subtotal) => 0;
}

public class HappyHour : IEstrategiaDescuento
{
    public string Nombre => "Happy Hour (20%)";
    public decimal Calcular(decimal subtotal) => subtotal * 0.20m;
}

public class ClienteVip : IEstrategiaDescuento
{
    public string Nombre => "Cliente VIP (15%)";
    public decimal Calcular(decimal subtotal) => subtotal * 0.15m;
}

public class MenuDelDia : IEstrategiaDescuento
{
    public string Nombre => "Menú del día (10%)";
    public decimal Calcular(decimal subtotal) => subtotal * 0.10m;
}

// --- Context ---
public class CajaRegistradora
{
    private IEstrategiaDescuento _estrategia;

    public CajaRegistradora(IEstrategiaDescuento estrategia) => _estrategia = estrategia;

    // Clave del patrón: cambiar la estrategia en tiempo de ejecución
    public void CambiarEstrategia(IEstrategiaDescuento nuevaEstrategia)
    {
        Console.WriteLine($"  ↪ Cambiando estrategia: {_estrategia.Nombre} → {nuevaEstrategia.Nombre}");
        _estrategia = nuevaEstrategia;
    }

    public void Cobrar(string platillo, decimal precio)
    {
        var descuento = _estrategia.Calcular(precio);
        var total = precio - descuento;

        Console.WriteLine($"  Platillo: {platillo}");
        Console.WriteLine($"  Precio base:  ¢{precio,10:N2}");
        Console.WriteLine($"  Descuento ({_estrategia.Nombre}): ¢{descuento,10:N2}");
        Console.WriteLine("  ─────────────────────────");
        Console.WriteLine($"  TOTAL:        ¢{total,10:N2}");
    }
}

public static class StrategyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧠 STRATEGY — Algoritmos intercambiables\n");
        Console.WriteLine("  Escenario: La caja aplica descuentos según el cliente\n");

        var caja = new CajaRegistradora(new SinDescuento());

        Console.WriteLine("  ── Cuenta #1: cliente cualquiera ──");
        caja.Cobrar("Pizza margarita", 9_500m);
        Console.WriteLine();

        caja.CambiarEstrategia(new HappyHour());
        Console.WriteLine("  ── Cuenta #2: son las 5 p.m. ──");
        caja.Cobrar("Hamburguesa monstruosa", 12_000m);
        Console.WriteLine();

        caja.CambiarEstrategia(new ClienteVip());
        Console.WriteLine("  ── Cuenta #3: cliente VIP ──");
        caja.Cobrar("Corte de carne", 28_000m);
        Console.WriteLine();

        caja.CambiarEstrategia(new MenuDelDia());
        Console.WriteLine("  ── Cuenta #4: menú del día ──");
        caja.Cobrar("Casado del día", 6_500m);
        Console.WriteLine();

        Console.WriteLine("  ✅ Sin Strategy: if/else por cada tipo de descuento.");
        Console.WriteLine("     Con Strategy: cada descuento es una clase separada.");
        Console.WriteLine("     Fácil de extender — solo agregas una clase más.");
    }
}
