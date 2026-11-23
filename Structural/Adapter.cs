namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN ADAPTER — Interfaces incompatibles → compatibles
/// ============================================================================
/// Intención: permitir que clases con interfaces incompatibles trabajen
/// juntas, traduciendo llamadas de una interfaz a otra.
///
/// Analogía: un emulador que deja jugar cartuchos de consolas clásicas (NES,
/// arcade) en una sola máquina: cada Adapter traduce la API antigua a la
/// interfaz común del emulador.
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

// --- Target: la interfaz que espera nuestra app (el emulador) ---
public interface IConsolaRetro
{
    bool Jugar(string rom);
}

// --- Adaptee 1: consola NES clásica ---
public class ConsolaNES
{
    public bool Encender()
    {
        Console.WriteLine("  [NES] Consola encendida. Soplando el cartucho...");
        return true;
    }

    public bool InsertarCartucho(string cartucho)
    {
        Console.WriteLine($"  [NES] Cartucho '{cartucho}' insertado en el puerto 60 pines");
        return true;
    }
}

// --- Adapter 1: traduce IConsolaRetro -> ConsolaNES ---
public class ConsolaNesAdapter : IConsolaRetro
{
    private readonly ConsolaNES _nes;

    public ConsolaNesAdapter(ConsolaNES nes) => _nes = nes;

    public bool Jugar(string rom)
    {
        _nes.Encender();
        var ok = _nes.InsertarCartucho(rom);
        Console.WriteLine("  [Adapter NES] ▶️  ¡Jugando 'Super Mario Bros'!");
        return ok;
    }
}

// --- Adaptee 2: máquina arcade original ---
public class ArcadeOriginal
{
    public string InsertarMonedas(int creditos) => $"ARCADE-CREDITO-{creditos}";
    public void EsperarAttractMode() => Console.WriteLine("  [Arcade] Pantalla de 'INSERT COIN' parpadeando...");
}

// --- Adapter 2: traduce IConsolaRetro -> ArcadeOriginal ---
public class ArcadeAdapter : IConsolaRetro
{
    private readonly ArcadeOriginal _arcade;

    public ArcadeAdapter(ArcadeOriginal arcade) => _arcade = arcade;

    public bool Jugar(string rom)
    {
        _arcade.EsperarAttractMode();
        var id = _arcade.InsertarMonedas(3);
        Console.WriteLine($"  [Adapter Arcade] ▶️  ¡Partida de '{rom}' iniciada ({id})!");
        return true;
    }
}

public static class AdapterDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔌 ADAPTER — Interfaces incompatibles → compatibles (retro gaming)\n");
        Console.WriteLine("  Escenario: Un emulador juega ROMs de consolas con APIs distintas\n");

        // La app solo conoce IConsolaRetro
        var consolas = new List<(string nombre, IConsolaRetro consola)>
        {
            ("NES (Adapter)", new ConsolaNesAdapter(new ConsolaNES())),
            ("Arcade (Adapter)", new ArcadeAdapter(new ArcadeOriginal()))
        };

        foreach (var (nombre, consola) in consolas)
        {
            Console.WriteLine($"  ── Consola: {nombre} ──");
            var ok = consola.Jugar("Contra");
            Console.WriteLine($"  Resultado: {(ok ? "✅ Partida en marcha" : "❌ Falló")}");
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El Adapter permite usar cualquier consola");
        Console.WriteLine("     sin modificar el emulador. Solo un Adapter por sistema.");
    }
}
