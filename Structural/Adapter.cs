namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN ADAPTER — Interfaces incompatibles → compatibles
/// ============================================================================
/// Intención: permitir que clases con interfaces incompatibles trabajen
/// juntas, traduciendo llamadas de una interfaz a otra.
///
/// Analogía: adaptador de corriente entre un enchufe europeo y uno americano.
///
/// Cuándo usarlo: integrar librerías de terceros o sistemas legacy cuya
/// interfaz no puedes cambiar.
///
/// Cuándo NO usarlo: si controlas ambas interfaces y puedes cambiarlas,
/// mejor ajustarlas directamente.
///
/// USO REAL: Integración con APIs de terceros, wrappers de bibliotecas
/// legacy, normalización de datos de distintos proveedores.
/// ============================================================================

// --- Target: la interfaz que espera nuestra app ---
public interface IPasarelaPago
{
    bool Cobrar(decimal monto, string moneda);
}

// --- Adaptee: pasarela moderna con otra interfaz ---
public class StripeLike
{
    public string Charge(double amountCents, string currencyCode)
    {
        var id = Guid.NewGuid().ToString("N").ToUpper();
        return $"stripe-{id[..12]}";
    }
}

// --- Adapter: traduce IPasarelaPago -> StripeLike ---
public class StripeAdapter : IPasarelaPago
{
    private readonly StripeLike _stripe;

    public StripeAdapter(StripeLike stripe) => _stripe = stripe;

    public bool Cobrar(decimal monto, string moneda)
    {
        Console.WriteLine("  [StripeAdapter] Convirtiendo monto a centavos...");

        var id = _stripe.Charge((double)(monto * 100), moneda.ToLower());

        Console.WriteLine($"  [StripeAdapter] Cobro confirmado con id {id}");
        return !string.IsNullOrEmpty(id);
    }
}

// --- Adaptee: pasarela legacy (sistema antiguo) ---
public class PaypalLegacy
{
    public string SendPayment(string amountTexto, string currency)
    {
        return $"pp-legacy-{DateTime.Now.Ticks}";
    }
}

// --- Adapter: traduce IPasarelaPago -> PaypalLegacy ---
public class PaypalAdapter : IPasarelaPago
{
    private readonly PaypalLegacy _legacy;

    public PaypalAdapter(PaypalLegacy legacy) => _legacy = legacy;

    public bool Cobrar(decimal monto, string moneda)
    {
        var montoTexto = monto.ToString("F2");
        var id = _legacy.SendPayment(montoTexto, moneda);
        Console.WriteLine($"  [PaypalAdapter] Cobro legacy confirmado: {id}");
        return true;
    }
}

public static class AdapterDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔌 ADAPTER — Interfaces incompatibles → compatibles\n");
        Console.WriteLine("  Escenario: La app cobra a través de distintas pasarelas de pago\n");

        // La app solo conoce IPasarelaPago
        var pasarelas = new List<(string nombre, IPasarelaPago pasarela)>
        {
            ("Stripe (Adapter)", new StripeAdapter(new StripeLike())),
            ("PayPal Legacy (Adapter)", new PaypalAdapter(new PaypalLegacy()))
        };

        foreach (var (nombre, pasarela) in pasarelas)
        {
            Console.WriteLine($"  ── Pasarela: {nombre} ──");
            var resultado = pasarela.Cobrar(49.99m, "USD");
            Console.WriteLine($"  Resultado: {(resultado ? "✅ Cobrado" : "❌ Falló")}");
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El Adapter permite que la app use cualquier");
        Console.WriteLine("     pasarela sin modificar su código.");
        Console.WriteLine("     Solo se necesita un nuevo Adapter por cada proveedor.");
    }
}
