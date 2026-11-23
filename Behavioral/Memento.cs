namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN MEMENTO — Guardar y restaurar estado
/// ============================================================================
/// Intención: capturar y externalizar el estado interno de un objeto SIN
/// violar su encapsulamiento, de modo que el objeto pueda ser restaurado a
/// ese estado más tarde.
///
/// Analogía: el Pensadero de Harry Potter: Dumbledore extrae un recuerdo, lo
/// guarda en un frasco y puede volver a sumergirse en él exactamente como era.
/// El Pensadero (Caretaker) guarda recuerdos sin alterar la mente del mago.
///
/// Cuándo usarlo: checkpoints, undo de estado completo, snapshots.
///
/// Cuándo NO usarlo: si el objeto es pequeño, guardar una copia simple basta;
/// el patrón brilla con estado grande o privado.
///
/// USO REAL: Guardado de juegos (checkpoints), undo en editores de gráficos,
/// snapshots de máquinas virtuales, historial de configuraciones.
/// ============================================================================

// --- Memento: almacena un recuerdo (inmutable desde fuera) ---
public record RecuerdoMemento(string Titulo, string Descripcion, int Emocion, DateTimeOffset GuardadoEn);

// --- Originator: el mago cuya "memoria" se guarda ---
public class Mago
{
    public string Nombre { get; set; } = "Harry";
    public List<string> Recuerdos { get; set; } = [];
    public int Emocion { get; set; } = 50;

    public RecuerdoMemento ExtraerRecuerdo()
    {
        var descripcion = string.Join("; ", Recuerdos);
        Console.WriteLine("  🧠 Extrayendo un recuerdo hacia el Pensadero...");
        return new RecuerdoMemento(Nombre, descripcion, Emocion, DateTimeOffset.UtcNow);
    }

    public void Restaurar(RecuerdoMemento estado)
    {
        Console.WriteLine($"  ⏪ Reviviendo el recuerdo de {estado.Titulo} desde {estado.GuardadoEn:HH:mm:ss}Z...");
        Nombre = estado.Titulo;
        Recuerdos = estado.Descripcion.Length > 0
            ? estado.Descripcion.Split("; ").ToList()
            : [];
        Emocion = estado.Emocion;
    }

    public void Mostrar()
    {
        var lista = Recuerdos.Count > 0 ? string.Join(", ", Recuerdos) : "(sin recuerdos)";
        var preview = lista.Length > 50 ? lista[..50] + "..." : lista;
        Console.WriteLine($"    🧙 [{Nombre}] Recuerdos: {preview} · Emoción: {Emocion}");
    }

    public void AgregarRecuerdo(string recuerdo)
    {
        Recuerdos.Add(recuerdo);
        Emocion = Math.Min(100, Emocion + 10);
        Console.WriteLine($"    ➕ Recuerdo añadido: \"{recuerdo}\"");
    }

    public void SufrirDesmemorizacion()
    {
        Recuerdos = [];
        Emocion = 0;
        Console.WriteLine("    🗑️ ¡Obliviate! Los recuerdos se han borrado.");
    }
}

// --- Caretaker: administra los mementos (el Pensadero) ---
public class Pensadero
{
    private readonly Stack<RecuerdoMemento> _recuerdos = new();

    public void Guardar(RecuerdoMemento estado)
    {
        _recuerdos.Push(estado);
        Console.WriteLine($"    🏺 Recuerdos en el Pensadero: {_recuerdos.Count}");
    }

    public RecuerdoMemento? Restaurar()
    {
        if (_recuerdos.Count == 0)
        {
            Console.WriteLine("    ⚠️  El Pensadero está vacío");
            return null;
        }
        return _recuerdos.Pop();
    }
}

public static class MementoDemo
{
    public static void Run()
    {
        Console.WriteLine("  💾 MEMENTO — Guardar y restaurar estado (el Pensadero de Dumbledore)\n");
        Console.WriteLine("  Escenario: Un mago guarda recuerdos por si un Obliviate le borra la memoria\n");

        var mago = new Mago();
        var pensadero = new Pensadero();

        // Inicio
        Console.WriteLine("  ── Primer recuerdo ──");
        mago.AgregarRecuerdo("Recibí mi carta de Hogwarts");
        mago.Mostrar();
        pensadero.Guardar(mago.ExtraerRecuerdo());
        Console.WriteLine();

        // Más recuerdos
        Console.WriteLine("  ── La ceremonia del Sombrero Seleccionador ──");
        mago.AgregarRecuerdo("Gryffindor!");
        mago.AgregarRecuerdo("Mi primera clase de Defensas");
        mago.Mostrar();
        pensadero.Guardar(mago.ExtraerRecuerdo());
        Console.WriteLine();

        // Desastre
        Console.WriteLine("  ── ¡Desastre! Un Death Eater lanza Obliviate ──");
        mago.SufrirDesmemorizacion();
        mago.Mostrar();
        Console.WriteLine();

        // Restaurar el último recuerdo
        Console.WriteLine("  ── Sumergirse en el último recuerdo del Pensadero ──");
        var checkpoint = pensadero.Restaurar();
        if (checkpoint is not null)
        {
            mago.Restaurar(checkpoint);
            mago.Mostrar();
        }
        Console.WriteLine();

        // Restaurar al primer recuerdo
        Console.WriteLine("  ── Sumergirse en el primer recuerdo ──");
        var inicio = pensadero.Restaurar();
        if (inicio is not null)
        {
            mago.Restaurar(inicio);
            mago.Mostrar();
        }
        Console.WriteLine();

        Console.WriteLine("  ✅ El memento encapsula el estado sin exponer los detalles internos.");
        Console.WriteLine("     El Pensadero (Caretaker) nunca modifica los recuerdos directamente.");
    }
}
