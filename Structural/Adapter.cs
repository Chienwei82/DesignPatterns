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
        Thread.Sleep(40); // simula procesamiento
        var codigo = Guid.NewGuid().ToString("N").ToUpper();
        return $"INTL-{codigo[..9]}";
    }
}

// --- Adapter: adapta ProveedorInternacional -> IProveedorIngredientes ---
public class ProveedorInternacionalAdapter : IProveedorIngredientes
{
    private readonly ProveedorInternacional _proveedor;

    public ProveedorInternacionalAdapter(ProveedorInternacional proveedor)
    {
        _proveedor = proveedor;
    }

    public bool Entregar(string ingrediente, decimal kilos)
    {
        Console.WriteLine("  [Adapter] Traduciendo el pedido al formato internacional...");

        // 1. Normalizar datos
        var item = ingrediente.ToLower();
        var weight = (double)kilos;
        var unit = "kg";

        // 2. Llamar al proveedor con su interfaz nativa
        var guia = _proveedor.Deliver(item, weight, unit);

        // 3. Traducir la respuesta
        Console.WriteLine($"  [Adapter] Entrega confirmada con guía {guia}");
        return !string.IsNullOrEmpty(guia);
    }
}

// --- Otro Adaptee: distribuidor legacy (sistema antiguo) ---
public class ProveedorLegacy
{
    public string EnviarMercancia(string ingredienteEncriptado, string kilosTexto)
    {
        Thread.Sleep(30);
        return $"LEGACY-{DateTime.Now.Ticks}";
    }
}

// --- Otro Adapter ---
public class ProveedorLegacyAdapter : IProveedorIngredientes
{
    private readonly ProveedorLegacy _legacy;

    public ProveedorLegacyAdapter(ProveedorLegacy legacy)
    {
        _legacy = legacy;
    }

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
