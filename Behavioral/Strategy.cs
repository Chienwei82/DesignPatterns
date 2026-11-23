namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN STRATEGY — Algoritmos intercambiables
/// ============================================================================
/// Intención: definir una familia de algoritmos, encapsularlos y hacerlos
/// intercambiables. Permite que el algoritmo varíe independientemente del
/// cliente que lo usa.
///
/// Analogía: en un juego de pelea (o RPG) eliges el estilo de combate de tu
/// luchador: agresivo, defensivo o equilibrado. El personaje es el mismo; solo
/// cambia la estrategia con la que pelea.
///
/// Cuándo usarlo: varios "motores" intercambiables (impuestos por país, métodos
/// de pago, compresión).
///
/// Cuándo NO usarlo: si el algoritmo nunca cambia, inyectarlo es complejidad
/// gratis.
///
/// USO REAL: Estrategias de ordenamiento, métodos de pago en e-commerce,
/// compresión de archivos, cálculo de descuentos.
/// ============================================================================

// --- Strategy ---
public interface IEstiloCombate
{
    string Nombre { get; }
    string DescribirAtaque(string rival);
}

// --- Estrategias concretas ---
public class EstiloAgresivo : IEstiloCombate
{
    public string Nombre => "Agresivo (todo al ataque)";
    public string DescribirAtaque(string rival) =>
        $"💥 Combo veloz sobre {rival}: mucho daño, defensa baja";
}

public class EstiloDefensivo : IEstiloCombate
{
    public string Nombre => "Defensivo (bloqueo y contraataque)";
    public string DescribirAtaque(string rival) =>
        $"🛡️ Bloqueas a {rival} y contraatacas con precisión";
}

public class EstiloEquilibrado : IEstiloCombate
{
    public string Nombre => "Equilibrado";
    public string DescribirAtaque(string rival) =>
        $"⚖️ Alternas ataque y defensa contra {rival}";
}

// --- Context ---
public class Luchador
{
    public string Nombre { get; }
    private IEstiloCombate _estilo;

    public Luchador(string nombre, IEstiloCombate estiloInicial)
    {
        Nombre = nombre;
        _estilo = estiloInicial;
    }

    // Clave del patrón: cambiar la estrategia en tiempo de ejecución
    public void CambiarEstilo(IEstiloCombate nuevo)
    {
        Console.WriteLine($"  ↪ {Nombre} cambia de estilo: {_estilo.Nombre} → {nuevo.Nombre}");
        _estilo = nuevo;
    }

    public void Pelear(string rival)
    {
        Console.WriteLine($"  {Nombre} [{_estilo.Nombre}] vs {rival}");
        Console.WriteLine($"     {_estilo.DescribirAtaque(rival)}");
    }
}

public static class StrategyDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧠 STRATEGY — Algoritmos intercambiables (estilos de combate)\n");
        Console.WriteLine("  Escenario: Ryu cambia su estilo de pelea según el rival\n");

        var ryu = new Luchador("Ryu", new EstiloEquilibrado());

        Console.WriteLine("  ── Combate #1: estilo equilibrado ──");
        ryu.Pelear("Ken");
        Console.WriteLine();

        ryu.CambiarEstilo(new EstiloAgresivo());
        Console.WriteLine("  ── Combate #2: contra un rival resistente, ataca a fondo ──");
        ryu.Pelear("M. Bison");
        Console.WriteLine();

        ryu.CambiarEstilo(new EstiloDefensivo());
        Console.WriteLine("  ── Combate #3: contra un rival veloz, defiende y contraataca ──");
        ryu.Pelear("Cammy");
        Console.WriteLine();

        Console.WriteLine("  ✅ Sin Strategy: un if/else gigante por cada estilo.");
        Console.WriteLine("     Con Strategy: cada estilo es una clase intercambiable.");
        Console.WriteLine("     Fácil de extender — solo agregas otra clase de estilo.");
    }
}
