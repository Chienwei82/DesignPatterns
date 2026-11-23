namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN MEDIATOR — Comunicación centralizada
/// ============================================================================
/// Intención: definir un objeto que encapsula cómo interactúan un conjunto de
/// objetos. Promueve el acoplamiento débil evitando que los objetos se
/// referencien explícitamente entre sí.
///
/// Analogía: la party de un MMO (World of Warcraft): el tanque, el sanador y
/// el DPS no se hablan entre sí directamente; todo se coordina por el chat del
/// grupo (el mediador).
///
/// Cuándo usarlo: salas de chat, paneles UI que se afectan entre sí,
/// coordinación de formularios.
///
/// Cuándo NO usarlo: si solo dos objetos se comunican, el mediador es un
/// intermediario innecesario.
///
/// USO REAL: Salas de chat, control de tráfico aéreo, coordinadores de
/// workflows, mediadores en frameworks UI (MVVM).
/// ============================================================================

// --- Mediator ---
public interface IChatParty
{
    void Registrar(MiembroParty miembro);
    void Enviar(string mensaje, MiembroParty remitente);
}

// --- Colega ---
public abstract class MiembroParty
{
    public string Nombre { get; }
    public string Rol { get; }
    private IChatParty? _party;

    protected MiembroParty(string nombre, string rol)
    {
        Nombre = nombre;
        Rol = rol;
    }

    public void UnirseAParty(IChatParty party)
    {
        _party = party;
        _party.Registrar(this);
    }

    public void Enviar(string mensaje)
    {
        Console.WriteLine($"  [{Nombre} ({Rol})] >> {mensaje}");
        _party?.Enviar(mensaje, this);
    }

    public abstract void Recibir(string mensaje, MiembroParty remitente);
}

// --- Mediador concreto: la party (chat del grupo) ---
public class PartyChat : IChatParty
{
    private readonly List<MiembroParty> _miembros = [];

    public void Registrar(MiembroParty miembro)
    {
        _miembros.Add(miembro);
        Console.WriteLine($"  [Party] {miembro.Nombre} ({miembro.Rol}) se unió. Total: {_miembros.Count} miembros");
    }

    public void Enviar(string mensaje, MiembroParty remitente)
    {
        foreach (var miembro in _miembros)
        {
            if (miembro != remitente)
                miembro.Recibir(mensaje, remitente);
        }
    }
}

// --- Colegas concretos ---
public class Tanque : MiembroParty
{
    public Tanque(string nombre) : base(nombre, "Tanque") { }
    public override void Recibir(string mensaje, MiembroParty remitente) =>
        Console.WriteLine($"    🛡️ [{Nombre}] Mensaje de {remitente.Nombre}: \"{mensaje}\"");
}

public class Sanador : MiembroParty
{
    public Sanador(string nombre) : base(nombre, "Sanador") { }
    public override void Recibir(string mensaje, MiembroParty remitente) =>
        Console.WriteLine($"    💚 [{Nombre}] Mensaje de {remitente.Nombre}: \"{mensaje}\"");
}

public class DPS : MiembroParty
{
    public DPS(string nombre) : base(nombre, "DPS") { }
    public override void Recibir(string mensaje, MiembroParty remitente) =>
        Console.WriteLine($"    ⚔️ [{Nombre}] Mensaje de {remitente.Nombre}: \"{mensaje}\"");
}

public static class MediatorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🗣️  MEDIATOR — Comunicación centralizada (party de raid MMO)\n");
        Console.WriteLine("  Escenario: Una party de World of Warcraft se coordina por el chat del grupo\n");

        var party = new PartyChat();

        var tank = new Tanque("Thrall");
        var heals = new Sanador("Malfurion");
        var dps = new DPS("Illidan");

        tank.UnirseAParty(party);
        heals.UnirseAParty(party);
        dps.UnirseAParty(party);
        Console.WriteLine();

        tank.Enviar("¡Pulling the boss en 3... 2... 1!");
        Console.WriteLine();

        heals.Enviar("¡Cúbranse, viene AoE!");
        Console.WriteLine();

        dps.Enviar("Burn phase, all cooldowns!");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los miembros NO se conocen entre sí directamente.");
        Console.WriteLine("     Toda la coordinación pasa por la party (mediator).");
        Console.WriteLine("     Agregar/quitar jugadores no rompe el código de nadie.");
    }
}
