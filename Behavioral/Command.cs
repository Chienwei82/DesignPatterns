namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN COMMAND — Comandos como objetos (con UNDO/REDO)
/// ============================================================================
/// Intención: encapsular una solicitud como un objeto, permitiendo parametrizar
/// clientes con diferentes solicitudes, encolarlas o registrarlas, y soportar
/// operaciones reversibles (undo/redo).
///
/// Analogía: un videojuego tipo "rewind" (como Prince of Persia o Braid): cada
/// acción del personaje (saltar, atacar) es un comando que puede ejecutarse y
/// deshacerse (rebobinar el tiempo).
///
/// Cuándo usarlo: undo/redo (Ctrl+Z), colas de trabajos, macros, logging de
/// acciones.
///
/// Cuándo NO usarlo: si la operación nunca se deshace, encola ni parametriza,
/// una llamada directa es más simple.
///
/// USO REAL: Botones de toolbar en editores, operaciones CRUD en sistemas
/// transaccionales, colas de trabajos (jobs), macros de teclado, historial
/// de Ctrl+Z.
/// ============================================================================

// --- Command ---
public interface IComando
{
    string Nombre { get; }
    void Ejecutar();
    void Deshacer();
}

// --- Receiver: el personaje del videojuego ---
public class Personaje
{
    private int _posX;
    private int _posY;

    public Personaje(int x, int y)
    {
        _posX = x;
        _posY = y;
    }

    public void Mover(int dx, int dy)
    {
        _posX += dx;
        _posY += dy;
        Console.WriteLine($"    🚶 Se mueve a la posición ({_posX}, {_posY})");
    }

    public void Saltar()
    {
        Console.WriteLine($"    🦘 Salta desde ({_posX}, {_posY}) — altura +3");
    }

    public void Mostrar()
    {
        Console.WriteLine($"    ── Posición actual: ({_posX}, {_posY})");
    }
}

// --- Commands concretos ---
public class MoverComando : IComando
{
    private readonly Personaje _personaje;
    private readonly int _dx;
    private readonly int _dy;
    public string Nombre => $"Mover ({_dx}, {_dy})";

    public MoverComando(Personaje personaje, int dx, int dy)
    {
        _personaje = personaje;
        _dx = dx;
        _dy = dy;
    }

    public void Ejecutar() => _personaje.Mover(_dx, _dy);
    public void Deshacer() => _personaje.Mover(-_dx, -_dy);
}

public class SaltarComando : IComando
{
    private readonly Personaje _personaje;
    public string Nombre => "Saltar";

    public SaltarComando(Personaje personaje) => _personaje = personaje;

    public void Ejecutar() => _personaje.Saltar();

    // Saltar es instantáneo: deshacer lo deja justo donde estaba (aunque el salto
    // ya ocurrió). En un juego real, deshacer rebobinaría el estado completo.
    public void Deshacer() =>
        Console.WriteLine("    ⏪ Rebobinando el salto... (el personaje vuelve a su pose previa)");
}

// --- Invoker: maneja el historial y la ejecución ---
public class ControlDeJuego
{
    private readonly Stack<IComando> _historial = new();
    private readonly Stack<IComando> _rehacer = new();

    public void Ejecutar(IComando comando)
    {
        Console.WriteLine($"  ▶️ Ejecutando: {comando.Nombre}");
        comando.Ejecutar();
        _historial.Push(comando);
        _rehacer.Clear(); // al ejecutar algo nuevo, se limpia el redo
    }

    public void Deshacer()
    {
        if (_historial.Count == 0)
        {
            Console.WriteLine("  ⚠️  No hay comandos para deshacer");
            return;
        }

        var comando = _historial.Pop();
        Console.WriteLine($"  ↩️ Deshaciendo: {comando.Nombre}");
        comando.Deshacer();
        _rehacer.Push(comando);
    }

    public void Rehacer()
    {
        if (_rehacer.Count == 0)
        {
            Console.WriteLine("  ⚠️  No hay comandos para rehacer");
            return;
        }

        var comando = _rehacer.Pop();
        Console.WriteLine($"  ↪️ Rehaciendo: {comando.Nombre}");
        comando.Ejecutar();
        _historial.Push(comando);
    }
}

public static class CommandDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎮 COMMAND — Comandos como objetos con UNDO/REDO (rewind de videojuego)\n");
        Console.WriteLine("  Escenario: Un juego donde puedes rebobinar cada acción del personaje\n");

        var heroe = new Personaje(0, 0);
        var control = new ControlDeJuego();

        control.Ejecutar(new MoverComando(heroe, 5, 0));
        heroe.Mostrar();
        Console.WriteLine();

        control.Ejecutar(new SaltarComando(heroe));
        control.Ejecutar(new MoverComando(heroe, 0, -3));
        heroe.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ── Rebobinar el último movimiento ──");
        control.Deshacer();
        heroe.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ── Rebobinar también el salto ──");
        control.Deshacer();
        heroe.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ── Rehacer ──");
        control.Rehacer();
        Console.WriteLine();

        Console.WriteLine("  ✅ Cada comando sabe ejecutarse y deshacerse.");
        Console.WriteLine("     Se pueden encolar, registrar en una bitácora o serializar.");
    }
}
