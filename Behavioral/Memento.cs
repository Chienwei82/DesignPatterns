namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN MEMENTO — Guardar y restaurar estado
/// ============================================================================
/// Intención: capturar y externalizar el estado interno de un objeto SIN
/// violar su encapsulamiento, de modo que el objeto pueda ser restaurado a
/// ese estado más tarde.
///
/// Analogía: guardar partida en un videojuego y volver al checkpoint.
///
/// Cuándo usarlo: checkpoints, undo de estado completo, snapshots.
///
/// Cuándo NO usarlo: si el objeto es pequeño, guardar una copia simple basta;
/// el patrón brilla con estado grande o privado.
///
/// USO REAL: Guardado de juegos (checkpoints), undo en editores de gráficos,
/// snapshots de máquinas virtuales, historial de configuraciones.
/// ============================================================================

// --- Memento: almacena el estado (inmutable desde fuera) ---
public record DocumentoMemento(string Titulo, string Contenido, DateTimeOffset GuardadoEn);

// --- Originator: el objeto cuyo estado se guarda ---
public class DocumentoEditor
{
    public string Titulo { get; set; } = "Sin título";
    public string Contenido { get; set; } = "";

    public DocumentoMemento Guardar()
    {
        Console.WriteLine("  💾 Guardando el estado del documento...");
        return new DocumentoMemento(Titulo, Contenido, DateTimeOffset.UtcNow);
    }

    public void Restaurar(DocumentoMemento estado)
    {
        Console.WriteLine($"  ⏪ Restaurando documento desde {estado.GuardadoEn:HH:mm:ss}Z...");
        Titulo = estado.Titulo;
        Contenido = estado.Contenido;
    }

    public void Mostrar()
    {
        var preview = Contenido.Length > 50 ? Contenido[..50] + "..." : Contenido;
        Console.WriteLine($"    📄 [{Titulo}] {preview}");
    }

    public void Escribir(string texto)
    {
        Contenido += texto;
        Console.WriteLine($"    ➕ Agregado: \"{texto}\"");
    }

    public void BorrarTodo()
    {
        Contenido = "";
        Console.WriteLine("    🗑️ Contenido borrado");
    }
}

// --- Caretaker: administra los mementos (historial de guardados) ---
public class HistorialVersiones
{
    private readonly Stack<DocumentoMemento> _versiones = new();

    public void Guardar(DocumentoMemento estado)
    {
        _versiones.Push(estado);
        Console.WriteLine($"    📚 Versiones guardadas: {_versiones.Count}");
    }

    public DocumentoMemento? Restaurar()
    {
        if (_versiones.Count == 0)
        {
            Console.WriteLine("    ⚠️  No hay versiones guardadas");
            return null;
        }
        return _versiones.Pop();
    }
}

public static class MementoDemo
{
    public static void Run()
    {
        Console.WriteLine("  💾 MEMENTO — Guardar y restaurar estado sin romper encapsulamiento\n");
        Console.WriteLine("  Escenario: Un editor de documentos donde guardas versiones por si arruinas el contenido\n");

        var doc = new DocumentoEditor();
        var historial = new HistorialVersiones();

        // Inicio
        Console.WriteLine("  ── Empezamos el documento ──");
        doc.Titulo = "Informe mensual";
        doc.Contenido = "Introducción...";
        doc.Mostrar();
        historial.Guardar(doc.Guardar());
        Console.WriteLine();

        // Agregamos contenido
        Console.WriteLine("  ── Agregando secciones ──");
        doc.Escribir("Sección 1: Resultados");
        doc.Escribir("Sección 2: Análisis");
        doc.Mostrar();
        historial.Guardar(doc.Guardar());
        Console.WriteLine();

        // Se arruina
        Console.WriteLine("  ── ¡Desastre! Borraste todo por accidente ──");
        doc.BorrarTodo();
        doc.Mostrar();
        Console.WriteLine();

        // Restaurar al último checkpoint
        Console.WriteLine("  ── Restaurando el último checkpoint ──");
        var checkpoint = historial.Restaurar();
        if (checkpoint is not null)
        {
            doc.Restaurar(checkpoint);
            doc.Mostrar();
        }
        Console.WriteLine();

        // Restaurar al inicio
        Console.WriteLine("  ── Restaurando al inicio ──");
        var inicio = historial.Restaurar();
        if (inicio is not null)
        {
            doc.Restaurar(inicio);
            doc.Mostrar();
        }
        Console.WriteLine();

        Console.WriteLine("  ✅ El memento encapsula el estado sin exponer el Documento.");
        Console.WriteLine("     El Historial (Caretaker) nunca modifica el estado directamente.");
    }
}
