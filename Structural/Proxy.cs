namespace DesignPatterns.Structural;

/// PATRÓN PROXY
/// ────────────
/// Proporciona un SUSTITUTO o REPRESENTANTE de otro objeto para
/// controlar el acceso a él. Es como un intermediario que puede
/// agregar lógica antes/después de la llamada al objeto real.
///
/// USO REAL: Lazy loading (Entity Framework), control de acceso,
///           logging, caché, llamadas remotas (gRPC), virtual proxy.

// --- Sujeto real ---
public interface IServicioDatos
{
    string ObtenerDatos(int id);
}

// --- Sujeto concreto (costoso) ---
public class ServicioDatosRemoto : IServicioDatos
{
    public string ObtenerDatos(int id)
    {
        // Simula llamada a BD o API externa (lenta)
        Thread.Sleep(1500); // 1.5 segundos de latencia
        return $"Datos del registro #{id} — obtenidos del servidor remoto";
    }
}

// --- Proxy de Caché ---
public class ProxyCache : IServicioDatos
{
    private readonly ServicioDatosRemoto _real = new();
    private readonly Dictionary<int, (string datos, DateTimeOffset timestamp)> _cache = new();
    private readonly TimeSpan _tiempoExpiracion = TimeSpan.FromSeconds(30);

    public string ObtenerDatos(int id)
    {
        // Verificar caché
        if (_cache.TryGetValue(id, out var entry))
        {
            if (DateTimeOffset.UtcNow - entry.timestamp < _tiempoExpiracion)
            {
                Console.WriteLine($"  [Proxy-Cache] 🟢 Cache HIT para ID={id}");
                return $" [CACHE] {entry.datos}";
            }
            else
            {
                Console.WriteLine($"  [Proxy-Cache] 🟡 Cache EXPIRADO para ID={id}");
                _cache.Remove(id);
            }
        }
        else
        {
            Console.WriteLine($"  [Proxy-Cache] 🔴 Cache MISS para ID={id}");
        }

        // Llamar al servicio real
        var resultado = _real.ObtenerDatos(id);

        // Guardar en caché
        _cache[id] = (resultado, DateTimeOffset.UtcNow);
        return resultado;
    }
}

// --- Proxy de Seguridad / Control de Acceso ---
public class ProxySeguridad : IServicioDatos
{
    private readonly ServicioDatosRemoto _real = new();
    private readonly string _rolPermitido;
    private readonly string _rolUsuario;

    public ProxySeguridad(string rolPermitido, string rolUsuario = "Usuario")
    {
        _rolPermitido = rolPermitido;
        _rolUsuario = rolUsuario;
    }

    public string ObtenerDatos(int id)
    {
        if (_rolUsuario != _rolPermitido)
        {
            Console.WriteLine($"  [Proxy-Seguridad] 🚫 Acceso DENEGADO: rol '{_rolUsuario}' no autorizado. Se requiere '{_rolPermitido}'");
            return $" [DENEGADO] No tienes permisos para acceder al registro #{id}";
        }

        Console.WriteLine($"  [Proxy-Seguridad] ✅ Acceso PERMITIDO: rol '{_rolUsuario}'");
        return _real.ObtenerDatos(id);
    }
}

public static class ProxyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎭 PROXY — Control de acceso y optimización\n");
        Console.WriteLine("  Escenario: Sistema con datos remotos + control de acceso\n");

        // ── Proxy de Caché ──
        Console.WriteLine("  ── Proxy de Caché ──");
        var proxyCache = new ProxyCache();

        var sw = System.Diagnostics.Stopwatch.StartNew();
        var r1 = proxyCache.ObtenerDatos(42);
        sw.Stop();
        Console.WriteLine($"  Resultado: {r1}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms\n");

        // Segunda llamada al mismo ID — debe ser cache hit
        sw.Restart();
        var r2 = proxyCache.ObtenerDatos(42);
        sw.Stop();
        Console.WriteLine($"  Resultado: {r2}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms (mucho más rápido 🚀)\n");

        // ── Proxy de Seguridad ──
        Console.WriteLine("  ── Proxy de Seguridad ──");

        // Usuario normal intentando acceder a datos de administrador
        var proxySegDenegado = new ProxySeguridad("Administrador", rolUsuario: "Usuario");
        var resultadoDenegado = proxySegDenegado.ObtenerDatos(7);
        Console.WriteLine($"  Resultado: {resultadoDenegado}\n");

        // Administrador con acceso permitido
        var proxySegPermitido = new ProxySeguridad("Administrador", rolUsuario: "Administrador");
        var resultadoPermitido = proxySegPermitido.ObtenerDatos(8);
        Console.WriteLine($"  Resultado: {resultadoPermitido}");
        Console.WriteLine();

        Console.WriteLine("  ✅ El Proxy evita llamar al objeto real si no es necesario.");
        Console.WriteLine("     Útil para: caché, permisos, logging, lazy loading.");
    }
}
