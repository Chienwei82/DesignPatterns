namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN STATE — Comportamiento según el estado
/// ============================================================================
/// Intención: permitir que un objeto altere su comportamiento cuando su estado
/// interno cambia. Parecerá que el objeto cambia de clase.
///
/// Analogía: la evolución de un Pokémon (Charmander → Charmeleon → Charizard):
/// el mismo "ser" cambia de comportamiento y ataques según su estado
/// evolutivo.
///
/// Cuándo usarlo: flujos de pedidos, máquinas de estados de videojuegos,
/// workflows de aprobación.
///
/// Cuándo NO usarlo: con 2 estados triviales, un `enum` + `switch` es más
/// simple y suficiente.
///
/// USO REAL: Máquinas de estados en videojuegos (idle, running, jumping),
/// flujos de workflow (aprobación, rechazo), proceso de pedidos, conexiones TCP.
/// ============================================================================

// --- State ---
public interface IEstadoPokemon
{
    string Nombre { get; }
    void UsarMovimiento(Pokemon pokemon);
    void Evolucionar(Pokemon pokemon);
}

// --- Context ---
public class Pokemon
{
    public string Nombre { get; }
    public IEstadoPokemon Estado { get; private set; }

    public Pokemon(string nombre)
    {
        Nombre = nombre;
        Estado = CharmanderState.Instancia; // Estado inicial (singleton)
    }

    // Solo el propio Pokemon puede cambiar de estado
    internal void CambiarEstado(IEstadoPokemon nuevoEstado) => Estado = nuevoEstado;

    // Los métodos delegan al estado actual
    public void UsarMovimiento() => Estado.UsarMovimiento(this);
    public void Evolucionar() => Estado.Evolucionar(this);
}

// --- Estados concretos ---
public class CharmanderState : IEstadoPokemon
{
    public static readonly CharmanderState Instancia = new();
    public string Nombre => "🔥 Charmander";

    public void UsarMovimiento(Pokemon pokemon) =>
        Console.WriteLine($"  [{pokemon.Nombre}] Usa Ascuas (movimiento débil de fuego)");

    public void Evolucionar(Pokemon pokemon)
    {
        Console.WriteLine($"  [{pokemon.Nombre}] ¡Está evolucionando...! ✨");
        pokemon.CambiarEstado(CharmeleonState.Instancia);
    }
}

public class CharmeleonState : IEstadoPokemon
{
    public static readonly CharmeleonState Instancia = new();
    public string Nombre => "🔥 Charmeleon";

    public void UsarMovimiento(Pokemon pokemon) =>
        Console.WriteLine($"  [{pokemon.Nombre}] Usa Lanzallamas (poderoso ataque de fuego)");

    public void Evolucionar(Pokemon pokemon)
    {
        Console.WriteLine($"  [{pokemon.Nombre}] ¡Está evolucionando...! ✨");
        pokemon.CambiarEstado(CharizardState.Instancia);
    }
}

public class CharizardState : IEstadoPokemon
{
    public static readonly CharizardState Instancia = new();
    public string Nombre => "🐉 Charizard (Fuego/Volador)";

    public void UsarMovimiento(Pokemon pokemon) =>
        Console.WriteLine($"  [{pokemon.Nombre}] Usa Hiperrayo y despega a volar");

    public void Evolucionar(Pokemon pokemon) =>
        Console.WriteLine($"  [{pokemon.Nombre}] ⭐ Es su evolución final: no evoluciona más");
}

public static class StateDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔄 STATE — Comportamiento que cambia según el estado (evolución Pokémon)\n");
        Console.WriteLine("  Escenario: Charmander evoluciona y cambia su comportamiento\n");

        var ash = new Pokemon("Ash's Charmander");

        Console.WriteLine($"  🏁 Estado inicial: {ash.Estado.Nombre}");
        Console.WriteLine();

        Console.WriteLine("  ── Charmander usa un movimiento ──");
        ash.UsarMovimiento();
        Console.WriteLine();

        Console.WriteLine("  ── Charmander evoluciona ──");
        ash.Evolucionar();
        Console.WriteLine($"     Estado ahora: {ash.Estado.Nombre}\n");

        Console.WriteLine("  ── Charmeleon usa un movimiento (¡más fuerte!) ──");
        ash.UsarMovimiento();
        Console.WriteLine();

        Console.WriteLine("  ── Charmeleon evoluciona otra vez ──");
        ash.Evolucionar();
        Console.WriteLine($"     Estado ahora: {ash.Estado.Nombre}\n");

        Console.WriteLine("  ── Charizard usa su movimiento definitivo ──");
        ash.UsarMovimiento();
        Console.WriteLine();

        Console.WriteLine("  ── Charizard intenta evolucionar ──");
        ash.Evolucionar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Cada estado define CÓMO reacciona el Pokémon.");
        Console.WriteLine("     Sin State: if/else gigante con flags y validaciones.");
        Console.WriteLine("     Con State: cada etapa evolutiva es una clase independiente.");
    }
}
