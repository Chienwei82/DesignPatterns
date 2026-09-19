using System.Diagnostics;

namespace DesignPatterns.Structural;

/// PATRÓN PROXY
/// ────────────
/// Proporciona un SUSTITUTO o REPRESENTANTE de otro objeto para
/// controlar el acceso a él. Es como un intermediario que puede
/// agregar lógica antes/después de la llamada al objeto real.
///
/// USO REAL: Lazy loading (Entity Framework), control de acceso,
///           logging, caché, llamadas remotas (gRPC), virtual proxy.

// --- Sujeto (la interfaz común) ---
public interface IChef
{
    string Cocinar(string plato);
}

// --- Sujeto real (costoso y lento) ---
public class ChefReal : IChef
{
    public string Cocinar(string plato)
    {
        Thread.Sleep(300); // el chef estrella se toma su tiempo
        return $"  👨🍳 {plato} preparado por el Chef Real";
    }
}

// --- Proxy de Caché: recuerda platos ya preparados ---
public class ProxyChefCache : IChef
{
    private readonly ChefReal _real = new();
    private readonly Dictionary<string, string> _cache = new();

    public string Cocinar(string plato)
    {
        if (_cache.TryGetValue(plato, out var guardado))
        {
            Console.WriteLine($"  [Proxy-Cache] 🟢 Ya lo teníamos: {plato}");
            return guardado;
        }

        Console.WriteLine($"  [Proxy-Cache] 🔴 No estaba en caché: {plato}");
        var resultado = _real.Cocinar(plato);
        _cache[plato] = resultado;
        return resultado;
    }
}

// --- Proxy de Seguridad: solo el cliente VIP puede pedir el plato estrella ---
public class ProxyChefSeguridad : IChef
{
    private readonly ChefReal _real = new();
    private readonly string _rolUsuario;

    public ProxyChefSeguridad(string rolUsuario)
    {
        _rolUsuario = rolUsuario;
    }

    public string Cocinar(string plato)
    {
        if (plato == "Plato estrella" && _rolUsuario != "VIP")
        {
            Console.WriteLine($"  [Proxy-Seguridad] 🚫 '{_rolUsuario}' no puede pedir el {plato}");
            return $"  ❌ El {plato} es solo para clientes VIP";
        }

        Console.WriteLine($"  [Proxy-Seguridad] ✅ Acceso permitido para '{_rolUsuario}'");
        return _real.Cocinar(plato);
    }
}

public static class ProxyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎭 PROXY — Control de acceso y optimización\n");
        Console.WriteLine("  Escenario: El chef estrella es lento y exclusivo\n");

        // ── Proxy de Caché ──
        Console.WriteLine("  ── Proxy de Caché ──");
        var proxyCache = new ProxyChefCache();

        var sw = Stopwatch.StartNew();
        var r1 = proxyCache.Cocinar("Pizza");
        sw.Stop();
        Console.WriteLine($"  Resultado:{r1}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms\n");

        // Segunda vez el mismo plato — debe ser cache hit
        sw.Restart();
        var r2 = proxyCache.Cocinar("Pizza");
        sw.Stop();
        Console.WriteLine($"  Resultado:{r2}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms (mucho más rápido 🚀)\n");

        // ── Proxy de Seguridad ──
        Console.WriteLine("  ── Proxy de Seguridad ──");

        // Cliente normal intentando pedir el plato estrella
        var proxyCliente = new ProxyChefSeguridad(rolUsuario: "Cliente");
        var denegado = proxyCliente.Cocinar("Plato estrella");
        Console.WriteLine($"  Resultado:{denegado}\n");

        // Cliente VIP con acceso permitido
        var proxyVip = new ProxyChefSeguridad(rolUsuario: "VIP");
        var permitido = proxyVip.Cocinar("Plato estrella");
        Console.WriteLine($"  Resultado:{permitido}");
        Console.WriteLine();

        Console.WriteLine("  ✅ El Proxy evita llamar al objeto real si no es necesario.");
        Console.WriteLine("     Útil para: caché, permisos, logging, lazy loading.");
    }
}
