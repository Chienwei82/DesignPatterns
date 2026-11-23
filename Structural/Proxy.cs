namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN PROXY — Control de acceso y optimización
/// ============================================================================
/// Intención: proporcionar un sustituto o representante de otro objeto para
/// controlar el acceso a él, agregando lógica antes/después de la llamada.
///
/// Analogía: los archivos clasificados de S.H.I.E.L.D.: nunca accedes directo al
/// bóveda. Un Proxy de seguridad exige el nivel de credencial correcto y un
/// Proxy de caché recuerda los expedientes ya consultados.
///
/// Cuándo usarlo: cachear llamadas costosas, verificar permisos, lazy loading.
///
/// Cuándo NO usarlo: si el objeto es barato y no hay reglas de acceso.
///
/// USO REAL: Lazy loading (Entity Framework), control de acceso, logging,
/// caché, llamadas remotas (gRPC).
/// ============================================================================

using System.Diagnostics;

// --- Sujeto (la interfaz común) ---
public interface IRepositorioClasificado
{
    string ConsultarExpediente(string id);
}

// --- Sujeto real (costoso: consulta a la bóveda de S.H.I.E.L.D.) ---
public class ArchivoReal : IRepositorioClasificado
{
    public string ConsultarExpediente(string id)
    {
        Thread.Sleep(200); // simula abrir una bóveda física de alta seguridad
        return $"  🗂️ [BÓVEDA] Expediente {id.ToUpper()} — AGENTE: Phil Coulson";
    }
}

// --- Proxy de Caché: recuerda expedientes ya consultados ---
public class ExpedienteCacheProxy : IRepositorioClasificado
{
    private readonly ArchivoReal _real = new();
    private readonly Dictionary<string, string> _cache = new();

    public string ConsultarExpediente(string id)
    {
        if (_cache.TryGetValue(id, out var guardado))
        {
            Console.WriteLine($"  [Proxy-Cache] 🟢 Cache hit: {id} (desde memoria)");
            return guardado;
        }

        Console.WriteLine($"  [Proxy-Cache] 🔴 Cache miss: {id} (abriendo la bóveda)");
        var resultado = _real.ConsultarExpediente(id);
        _cache[id] = resultado;
        return resultado;
    }
}

// --- Proxy de Seguridad: solo credenciales autorizadas acceden ---
public class ExpedienteSeguridadProxy : IRepositorioClasificado
{
    private readonly ArchivoReal _real = new();
    private readonly string _credencial;

    public ExpedienteSeguridadProxy(string credencial) => _credencial = credencial;

    public string ConsultarExpediente(string id)
    {
        // Simulación: solo credencial 'director' ve expedientes 'nivel-10'
        if (id.StartsWith("nivel-10") && _credencial != "director")
        {
            Console.WriteLine($"  [Proxy-Seguridad] 🚫 Credencial '{_credencial}' sin acceso a '{id}'");
            return "  ❌ ACCESO DENEGADO — se requiere Nivel 10";
        }

        Console.WriteLine($"  [Proxy-Seguridad] ✅ Acceso concedido a '{_credencial}'");
        return _real.ConsultarExpediente(id);
    }
}

public static class ProxyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎭 PROXY — Control de acceso y optimización (archivos de S.H.I.E.L.D.)\n");
        Console.WriteLine("  Escenario: Expedientes clasificados con caché y nivel de credencial\n");

        // ── Proxy de Caché ──
        Console.WriteLine("  ── Proxy de Caché ──");
        var proxyCache = new ExpedienteCacheProxy();

        var sw = Stopwatch.StartNew();
        var r1 = proxyCache.ConsultarExpediente("agent-coulson");
        sw.Stop();
        Console.WriteLine($"  Resultado:{r1}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms\n");

        sw.Restart();
        var r2 = proxyCache.ConsultarExpediente("agent-coulson");
        sw.Stop();
        Console.WriteLine($"  Resultado:{r2}");
        Console.WriteLine($"  Tiempo: {sw.ElapsedMilliseconds}ms (mucho más rápido 🚀)\n");

        // ── Proxy de Seguridad ──
        Console.WriteLine("  ── Proxy de Seguridad ──");

        var agente = new ExpedienteSeguridadProxy(credencial: "agente");
        var denegado = agente.ConsultarExpediente("nivel-10-ikari");
        Console.WriteLine($"  Resultado:{denegado}\n");

        var director = new ExpedienteSeguridadProxy(credencial: "director");
        var permitido = director.ConsultarExpediente("nivel-10-ikari");
        Console.WriteLine($"  Resultado:{permitido}");
        Console.WriteLine();

        Console.WriteLine("  ✅ El Proxy evita tocar el objeto real si no hace falta.");
        Console.WriteLine("     Útil para: caché, permisos, logging, lazy loading.");
    }
}
