namespace DesignPatterns.Behavioral;

/// PATRÓN MEMENTO
/// ─────────────
/// Permite capturar y externalizar el estado interno de un objeto
/// SIN violar su encapsulamiento, de modo que el objeto pueda ser
/// restaurado a ese estado más tarde.
///
/// USO REAL: Guardado de juegos (checkpoints), undo en editores de
///           gráficos, snapshots de máquinas virtuales, historial
///           de configuraciones.

// --- Memento: almacena el estado (inmutable desde fuera) ---
public class EstadoJuego
{
    public string Nivel { get; }
    public int Vidas { get; }
    public int Puntaje { get; }
    public decimal PosicionX { get; }
    public decimal PosicionY { get; }
    public DateTime GuardadoEn { get; }

    public EstadoJuego(string nivel, int vidas, int puntaje, decimal x, decimal y)
    {
        Nivel = nivel;
        Vidas = vidas;
        Puntaje = puntaje;
        PosicionX = x;
        PosicionY = y;
        GuardadoEn = DateTime.Now;
    }

    public void Mostrar()
    {
        Console.WriteLine($"    🎮 {Nivel} | Vidas: {Vidas} | Puntaje: {Puntaje} | Pos: [{PosicionX:F1}, {PosicionY:F1}] | Guardado: {GuardadoEn:HH:mm:ss}");
    }
}

// --- Originator: el objeto cuyo estado se guarda ---
public class Partida
{
    public string Nivel { get; set; } = "Nivel 1";
    public int Vidas { get; set; } = 3;
    public int Puntaje { get; set; } = 0;
    public decimal PosicionX { get; set; } = 0;
    public decimal PosicionY { get; set; } = 0;

    public EstadoJuego Guardar()
    {
        Console.WriteLine($"  💾 Guardando partida en {Nivel}...");
        return new EstadoJuego(Nivel, Vidas, Puntaje, PosicionX, PosicionY);
    }

    public void Restaurar(EstadoJuego estado)
    {
        Console.WriteLine($"  ⏪ Restaurando partida desde {estado.GuardadoEn:HH:mm:ss}...");
        Nivel = estado.Nivel;
        Vidas = estado.Vidas;
        Puntaje = estado.Puntaje;
        PosicionX = estado.PosicionX;
        PosicionY = estado.PosicionY;
    }

    public void Mostrar()
    {
        Console.WriteLine($"    🎮 {Nivel} | Vidas: {Vidas} | Puntaje: {Puntaje} | Pos: [{PosicionX:F1}, {PosicionY:F1}]");
    }

    public void Avanzar(decimal x, decimal y, int puntos)
    {
        PosicionX += x;
        PosicionY += y;
        Puntaje += puntos;
        Console.WriteLine($"    🏃 Avanzando a [{PosicionX:F1}, {PosicionY:F1}] (+{puntos} pts)");
    }

    public void RecibirDaño()
    {
        Vidas--;
        Console.WriteLine($"    💥 ¡Daño recibido! Vidas restantes: {Vidas}");
    }
}

// --- Caretaker: administra los mementos (historial de guardados) ---
public class GestorGuardados
{
    private readonly Stack<EstadoJuego> _checkpoints = new();

    public void Guardar(EstadoJuego estado)
    {
        _checkpoints.Push(estado);
        Console.WriteLine($"    📚 Checkpoints guardados: {_checkpoints.Count}");
    }

    public EstadoJuego? Deshacer()
    {
        if (_checkpoints.Count == 0)
        {
            Console.WriteLine("    ⚠️  No hay checkpoints guardados");
            return null;
        }
        return _checkpoints.Pop();
    }
}

public static class MementoDemo
{
    public static void Run()
    {
        Console.WriteLine("  💾 MEMENTO — Guardar y restaurar estado sin romper encapsulamiento\n");
        Console.WriteLine("  Escenario: Checkpoints en un videojuego\n");

        var partida = new Partida();
        var gestor = new GestorGuardados();

        // Inicio
        Console.WriteLine("  ── Inicio de partida ──");
        partida.Mostrar();
        gestor.Guardar(partida.Guardar());
        Console.WriteLine();

        // Avanzamos
        Console.WriteLine("  ── Jugando... ──");
        partida.Avanzar(10, 5, 100);
        partida.Avanzar(20, 10, 250);
        partida.Mostrar();
        gestor.Guardar(partida.Guardar());
        Console.WriteLine();

        // Más avance y daño
        Console.WriteLine("  ── Avanzando y recibiendo daño... ──");
        partida.Avanzar(5, 30, 50);
        partida.RecibirDaño();
        partida.Mostrar();
        Console.WriteLine();

        // Restaurar al último checkpoint
        Console.WriteLine("  ── ¡Oh no! Restaurando último checkpoint ──");
        var checkpoint = gestor.Deshacer();
        if (checkpoint != null)
        {
            partida.Restaurar(checkpoint);
            partida.Mostrar();
        }
        Console.WriteLine();

        // Restaurar al inicio
        Console.WriteLine("  ── Restaurando al inicio ──");
        var inicio = gestor.Deshacer();
        if (inicio != null)
        {
            partida.Restaurar(inicio);
            partida.Mostrar();
        }
        Console.WriteLine();

        Console.WriteLine("  ✅ El memento encapsula el estado sin exponer la Partida.");
        Console.WriteLine("     El Gestor (Caretaker) nunca modifica el estado directamente.");
    }
}
