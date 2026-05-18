namespace DesignPatterns.Structural;

/// PATRÓN ADAPTER
/// ──────────────
/// Permite que clases con interfaces INCOMPATIBLES trabajen juntas.
/// Actúa como un "adaptador" o "traductor" entre dos mundos.
/// Es como un adaptador de corriente — convierte un enchufe europeo
/// a uno americano.
///
/// USO REAL: Integración con APIs de terceros, wrappers de bibliotecas
///           legacy, normalización de datos de distintos proveedores.

// --- Target: la interfaz que espera nuestro sistema ---
public interface IPagoProcesador
{
    bool Cobrar(string numeroTarjeta, decimal monto, string moneda);
}

// --- Adaptee: un servicio externo con interfaz diferente ---
// Simula un gateway de pago internacional (ej. Stripe, PayPal)
public class GatewayPagoInternacional
{
    public string Charge(string token, double amount, string currency)
    {
        // Simula procesamiento
        Thread.Sleep(50);
        return $"TX-{Guid.NewGuid():N}".ToUpper()[..12];
    }
}

// --- Adapter: convierte IPagoProcesador -> GatewayPagoInternacional ---
public class PagoAdapter : IPagoProcesador
{
    private readonly GatewayPagoInternacional _gateway;

    public PagoAdapter(GatewayPagoInternacional gateway)
    {
        _gateway = gateway;
    }

    public bool Cobrar(string numeroTarjeta, decimal monto, string moneda)
    {
        Console.WriteLine($"  [Adapter] Conviniendo solicitud...");

        // 1. Normalizar datos
        var token = $"tok_{numeroTarjeta[^4..]}"; // simula tokenización
        var amount = (double)monto;
        var currency = moneda.ToUpper();

        // 2. Llamar al gateway con su interfaz nativa
        Console.WriteLine($"  [Adapter] Llamando Gateway.Charge(token={token}, amount={amount}, currency={currency})");
        var txId = _gateway.Charge(token, amount, currency);

        // 3. Traducir la respuesta
        Console.WriteLine($"  [Adapter] Transacción completada: {txId}");
        return !string.IsNullOrEmpty(txId);
    }
}

// --- Otro Adaptee: pasarela legacy (sistema antiguo) ---
public class PasarelaPagoLegacy
{
    public string EnviarPago(string tarjetaEncriptada, string montoStr)
    {
        Thread.Sleep(30);
        return $"LEGACY-{DateTime.Now.Ticks}";
    }
}

// --- Otro Adapter ---
public class LegacyAdapter : IPagoProcesador
{
    private readonly PasarelaPagoLegacy _legacy;

    public LegacyAdapter(PasarelaPagoLegacy legacy)
    {
        _legacy = legacy;
    }

    public bool Cobrar(string numeroTarjeta, decimal monto, string moneda)
    {
        var tarjetaEncriptada = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(numeroTarjeta));
        var montoStr = $"{monto:F2}";
        var id = _legacy.EnviarPago(tarjetaEncriptada, montoStr);
        Console.WriteLine($"  [LegacyAdapter] Pago legacy procesado: {id}");
        return true;
    }
}

public static class AdapterDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔌 ADAPTER — Interfaces incompatibles → compatibles\n");
        Console.WriteLine("  Escenario: Sistema de e-commerce con múltiples pasarelas de pago\n");

        // El sistema solo conoce IPagoProcesador
        var procesadores = new List<(string nombre, IPagoProcesador proc)>
        {
            ("Stripe (Adapter)", new PagoAdapter(new GatewayPagoInternacional())),
            ("Legacy (Adapter)", new LegacyAdapter(new PasarelaPagoLegacy()))
        };

        foreach (var (nombre, proc) in procesadores)
        {
            Console.WriteLine($"  ── {nombre} ──");
            var resultado = proc.Cobrar("4000-1234-5678-9010", 49990.00m, "CRC");
            Console.WriteLine($"  Resultado: {(resultado ? "✅ Aprobado" : "❌ Rechazado")}");
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El Adapter permite que el sistema central use");
        Console.WriteLine("     cualquier pasarela sin modificar su código.");
        Console.WriteLine("     Solo se necesita un nuevo Adapter por cada integración.");
    }
}
