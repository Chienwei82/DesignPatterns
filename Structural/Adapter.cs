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

// --- Target: la interfaz que espera nuestra cocina ---
public interface IProveedorIngredientes
{
    bool Entregar(string ingrediente, decimal kilos);
}

// --- Adaptee: proveedor moderno con otra interfaz ---
public class ProveedorInternacional
{
    public string Deliver(string item, double weightKg, string unit)
    {
        var codigo = Guid.NewGuid().ToString("N").ToUpper();
        return $"INTL-{codigo[..9]}";
    }
}

// --- Adapter: traduce IProveedorIngredientes -> ProveedorInternacional ---
public class ProveedorInternacionalAdapter : IProveedorIngredientes
{
    private readonly ProveedorInternacional _proveedor;

    public ProveedorInternacionalAdapter(ProveedorInternacional proveedor) => _proveedor = proveedor;

    public bool Entregar(string ingrediente, decimal kilos)
    {
        Console.WriteLine("  [Adapter] Traduciendo el pedido al formato internacional...");

        var guia = _proveedor.Deliver(ingrediente.ToLower(), (double)kilos, "kg");

        Console.WriteLine($"  [Adapter] Entrega confirmada con guía {guia}");
        return !string.IsNullOrEmpty(guia);
    }
}

// --- Adaptee: distribuidor legacy (sistema antiguo) ---
public class ProveedorLegacy
{
    public string EnviarMercancia(string ingredienteEncriptado, string kilosTexto)
    {
        return $"LEGACY-{DateTime.Now.Ticks}";
    }
}

// --- Adapter: traduce IProveedorIngredientes -> ProveedorLegacy ---
public class ProveedorLegacyAdapter : IProveedorIngredientes
{
    private readonly ProveedorLegacy _legacy;

    public ProveedorLegacyAdapter(ProveedorLegacy legacy) => _legacy = legacy;

    public bool Entregar(string ingrediente, decimal kilos)
    {
        var ingredienteEncriptado = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(ingrediente));
        var kilosTexto = kilos.ToString("F2");
        var guia = _legacy.EnviarMercancia(ingredienteEncriptado, kilosTexto);
        Console.WriteLine($"  [LegacyAdapter] Entrega legacy confirmada: {guia}");
        return true;
    }
}

public static class AdapterDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔌 ADAPTER — Interfaces incompatibles → compatibles\n");
        Console.WriteLine("  Escenario: La cocina pide ingredientes a proveedores distintos\n");

        // La cocina solo conoce IProveedorIngredientes
        var proveedores = new List<(string nombre, IProveedorIngredientes proveedor)>
        {
            ("Internacional (Adapter)", new ProveedorInternacionalAdapter(new ProveedorInternacional())),
            ("Legacy (Adapter)", new ProveedorLegacyAdapter(new ProveedorLegacy()))
        };

        foreach (var (nombre, proveedor) in proveedores)
        {
            Console.WriteLine($"  ── Proveedor: {nombre} ──");
            var resultado = proveedor.Entregar("Tomate", 12.5m);
            Console.WriteLine($"  Resultado: {(resultado ? "✅ Entregado" : "❌ Falló")}");
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El Adapter permite que la cocina use cualquier");
        Console.WriteLine("     proveedor sin modificar su código.");
        Console.WriteLine("     Solo se necesita un nuevo Adapter por cada proveedor.");
    }
}
