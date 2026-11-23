namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN OBSERVER — Suscripción uno-a-muchos
/// ============================================================================
/// Intención: definir una dependencia uno-a-muchos entre objetos, de modo que
/// cuando un objeto cambia su estado, todos sus dependientes son notificados
/// automáticamente.
///
/// Analogía: la Batiseñal de Gotham: cuando Gordon la enciende, todos los
/// aliados de Batman (Robin, Alfred, la Liga) reaccionan sin que Batman tenga
/// que llamarlos uno por uno.
///
/// Cuándo usarlo: eventos de UI, notificaciones push, sincronizar vistas.
///
/// Cuándo NO usarlo: si un solo receptor escucha siempre, un callback directo
/// es más simple. Cuidado con el orden de desuscripción (memory leaks).
///
/// USO REAL: Eventos en UI (click, keypress), cotizaciones de bolsa en tiempo
/// real, notificaciones push, actualización de vistas en MVVM, events/delegates
/// de C#.
/// ============================================================================

// --- Sujeto (observable) ---
public interface IBatSenal
{
    void Suscribir(IAliado observador);
    void Desuscribir(IAliado observador);
    void EmitirSenal(string alerta, string amenaza);
}

// --- Observador ---
public interface IAliado
{
    string Nombre { get; }
    void Recibir(string alerta, string amenaza);
}

// --- Sujeto concreto ---
public class BatSenal : IBatSenal
{
    private readonly List<IAliado> _aliados = [];

    public void Suscribir(IAliado observador)
    {
        _aliados.Add(observador);
        Console.WriteLine($"  🦇 [Batiseñal] {observador.Nombre} se unió a la red de alertas");
    }

    public void Desuscribir(IAliado observador)
    {
        _aliados.Remove(observador);
        Console.WriteLine($"  🦇 [Batiseñal] {observador.Nombre} se dio de baja");
    }

    public void EmitirSenal(string alerta, string amenaza)
    {
        Console.WriteLine($"    Iluminando el cielo de Gotham a { _aliados.Count} aliado(s)...");

        foreach (var aliado in _aliados)
            aliado.Recibir(alerta, amenaza);

        Console.WriteLine();
    }

    // Gordon enciende la Batiseñal
    public void EncenderSenal(string alerta, string amenaza)
    {
        Console.WriteLine($"\n  🚨 BATISEÑAL: {alerta} — {amenaza}");
        EmitirSenal(alerta, amenaza);
    }
}

// --- Observadores concretos ---
public class Robin : IAliado
{
    public string Nombre => "Robin";
    public void Recibir(string alerta, string amenaza) =>
        Console.WriteLine($"    🎯 [Robin] '{alerta}': voy en camino a enfrentar a {amenaza}");
}

public class Alfred : IAliado
{
    public string Nombre => "Alfred";
    public void Recibir(string alerta, string amenaza) =>
        Console.WriteLine($"    🇬🇧 [Alfred] Preparo el Batmóvil y el café, señor. ({alerta})");
}

public class ComisionadoGordon : IAliado
{
    public string Nombre => "Comisionado Gordon";
    public void Recibir(string alerta, string amenaza) =>
        Console.WriteLine($"    👮 [Gordon] Registro el incidente '{alerta}' en la central");
}

// --- Observador con filtro (solo le interesan las amenazas del Joker) ---
public class VigilanteDelJoker : IAliado
{
    public string Nombre => "Detective Bullock";

    public void Recibir(string alerta, string amenaza)
    {
        var esJoker = amenaza.Contains("Joker", StringComparison.OrdinalIgnoreCase);

        if (!esJoker)
        {
            Console.WriteLine($"    🔇 [Bullock] Ignoro '{alerta}' (no es el Joker)");
            return;
        }
        Console.WriteLine($"    🃏 [Bullock] ¡El Joker ataca de nuevo: {amenaza}!");
    }
}

public static class ObserverDemo
{
    public static void Run()
    {
        Console.WriteLine("  👁️  OBSERVER — Suscripción y notificación en tiempo real (Batiseñal)\n");
        Console.WriteLine("  Escenario: Gordon enciende la Batiseñal y todos los aliados reaccionan\n");

        var senal = new BatSenal();

        var robin = new Robin();
        var alfred = new Alfred();
        var gordon = new ComisionadoGordon();
        var bullock = new VigilanteDelJoker();

        senal.Suscribir(robin);
        senal.Suscribir(alfred);
        senal.Suscribir(gordon);
        senal.Suscribir(bullock);
        Console.WriteLine();

        senal.EncenderSenal("RESCATE", "Dos Caras en el puerto");
        senal.EncenderSenal("EMERGENCIA", "El Joker en el Gotham Trust");

        senal.Desuscribir(gordon);
        Console.WriteLine();

        senal.EncenderSenal("ALERTA", "Enigma en el museo");

        Console.WriteLine("  ✅ Observer desacopla al emisor (Batiseñal) de los aliados.");
        Console.WriteLine("     Nuevos aliados se agregan sin modificar la Batiseñal.");
    }
}
