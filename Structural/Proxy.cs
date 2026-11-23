namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN PROXY — Control de acceso y optimización
/// ============================================================================
/// Intención: proporcionar un sustituto o representante de otro objeto para
/// controlar el acceso a él, agregando lógica antes/después de la llamada.
///
/// Analogía: el asistente personal que filtra llamadas antes de pasarlas al jefe.
///
/// Cuándo usarlo: cachear llamadas costosas, verificar permisos, lazy loading
/// (Entity Framework lo usa).
///
/// Cuándo NO usarlo: si el objeto es barato y no hay reglas de acceso; el proxy
/// solo agrega complejidad.
///
/// USO REAL: Lazy loading (Entity Framework), control de acceso, logging,
/// caché, llamadas remotas (gRPC).
/// ============================================================================

using System.Diagnostics;

// --- Sujeto (la interfaz común) ---
public interface IRepositorio
{
    string Obtener(string id);
}

// --- Sujeto real (costoso y lento: consulta a BD) ---
public class RepositorioReal : IRepositorio
{
    public string Obtener(string id)
    {
        Thread.Sleep(200); // simula una consulta lenta a base de datos
        return $"  📦 [BD] Registro {id} - Nombre de ejemplo";
    }
}

// --- Proxy de Caché: recuerda registros ya consultados ---
public class RepositorioCacheProxy : IRepositorio
{
    private readonly RepositorioReal _real = new();
    private readonly Dictionary<string, string> _cache = new();

    public string Obtener(string id)
    {
        if (_cache.TryGetValue(id, out var guardado))
        {
            Console.WriteLine($"  [Proxy-Cache] 🟢 Cache hit: {id} (desde memoria)");
            return guardado;
        }

        Console.WriteLine($"  [Proxy-Cache] 🔴 Cache miss: {id} (consultando BD)");
        var resultado = _real.Obtener(id);
        _cache[id] = resultado;
        return resultado;
    }
}

// --- Proxy de Seguridad: solo roles autorizados pueden acceder ---
public class RepositorioSeguridadProxy : IRepositorio
{
    private readonly RepositorioReal _real = new();
    private readonly string _rol;

    public RepositorioSeguridadProxy(string rol) => _rol = rol;

    public string Obtener(string id)
    {
        // Simulación: solo 'admin' puede ver registros con id 'secret-*'
        if (id.StartsWith("secret-") && _rol != "admin")
        {
            Console.WriteLine($"  [Proxy-Seguridad] 🚫 Rol '{_rol}' sin acceso a '{id}'");
            return "  ❌ Acceso denegado";
        }

        Console.WriteLine($"  [Proxy-Seguridad] ✅ Acceso permitido para '{_rol}'");
        return _real.Obtener(id);
    }
}

public static class ProxyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎭 PROXY — Control de acceso y optimización\n");
        Console.WriteLine("  Escenario: Repositorio de datos con caché y permisos\n");

        // ── Proxy de Caché ──
        Console.WriteLine("  ── Proxy de Caché ──");
        var proxyCache = new RepositorioCacheProxy();

        var sw = Stopwatch.StartNew();
        var r1 = proxyCache.Obtener("user-123");
        sw.Stop();
        Console.WriteLine($"  Resultado:{r1}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms\n");

        // Segunda vez el mismo registro — cache hit (más rápido)
        sw.Restart();
        var r2 = proxyCache.Obtener("user-123");
        sw.Stop();
        Console.WriteLine($"  Resultado:{r2}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms (mucho más rápido 🚀)\n");

        // ── Proxy de Seguridad ──
        Console.WriteLine("  ── Proxy de Seguridad ──");

        // Usuario normal intentando acceder a recurso restringido
        var proxyUser = new RepositorioSeguridadProxy(rol: "user");
        var denegado = proxyUser.Obtener("secret-keys");
        Console.WriteLine($"  Resultado:{denegado}\n");

        // Admin con acceso permitido
        var proxyAdmin = new RepositorioSeguridadProxy(rol: "admin");
        var permitido = proxyAdmin.Obtener("secret-keys");
        Console.WriteLine($"  Resultado:{permitido}");
        Console.WriteLine();

        Console.WriteLine("  ✅ El Proxy evita llamar al objeto real si no es necesario.");
        Console.WriteLine("     Útil para: caché, permisos, logging, lazy loading.");
    }
}
