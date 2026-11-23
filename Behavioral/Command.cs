namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN COMMAND — Comandos como objetos (con UNDO/REDO)
/// ============================================================================
/// Intención: encapsular una solicitud como un objeto, permitiendo parametrizar
/// clientes con diferentes solicitudes, encolarlas o registrarlas, y soportar
/// operaciones reversibles (undo/redo).
///
/// Analogía: pedir en un restaurante: el mesero escribe el pedido (comando),
/// el cocinero lo ejecuta; el pedido se puede anular. En este proyecto: un
/// editor de texto donde cada inserción/borrado es un comando con undo/redo.
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

// --- Receiver: el editor de texto real ---
public class EditorTexto
{
    private readonly List<string> _lineas = [];

    public void AgregarLinea(string texto)
    {
        _lineas.Add(texto);
        Console.WriteLine($"    ➕ Línea agregada: \"{texto}\"");
    }

    public string QuitarLinea()
    {
        if (_lineas.Count == 0)
        {
            Console.WriteLine("    ⚠️  El editor está vacío");
            return "";
        }

        var ultima = _lineas[^1];
        _lineas.RemoveAt(_lineas.Count - 1);
        Console.WriteLine($"    ➖ Línea eliminada: \"{ultima}\"");
        return ultima;
    }

    public void Mostrar()
    {
        var texto = _lineas.Count > 0 ? string.Join(" | ", _lineas) : "(vacío)";
        Console.WriteLine($"    ── Editor actual: {texto}");
    }
}

// --- Commands concretos ---
public class InsertarLinea : IComando
{
    private readonly EditorTexto _editor;
    private readonly string _texto;
    public string Nombre => $"Insertar \"{_texto}\"";

    public InsertarLinea(EditorTexto editor, string texto)
    {
        _editor = editor;
        _texto = texto;
    }

    public void Ejecutar() => _editor.AgregarLinea(_texto);
    public void Deshacer() => _editor.QuitarLinea();
}

public class BorrarLinea : IComando
{
    private readonly EditorTexto _editor;
    private string _lineaBorrada = "";
    public string Nombre => "Borrar última línea";

    public BorrarLinea(EditorTexto editor) => _editor = editor;

    public void Ejecutar() => _lineaBorrada = _editor.QuitarLinea();

    public void Deshacer()
    {
        if (!string.IsNullOrEmpty(_lineaBorrada))
            _editor.AgregarLinea(_lineaBorrada);
    }
}

// --- Invoker: maneja el historial y la ejecución ---
public class EditorInvoker
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
        Console.WriteLine("  🎮 COMMAND — Comandos como objetos (con UNDO/REDO)\n");
        Console.WriteLine("  Escenario: Un editor de texto donde el usuario inserta y borra líneas\n");

        var editor = new EditorTexto();
        var invoker = new EditorInvoker();

        invoker.Ejecutar(new InsertarLinea(editor, "línea 1: Hola mundo"));
        editor.Mostrar();
        Console.WriteLine();

        invoker.Ejecutar(new InsertarLinea(editor, "línea 2: Este es un ejemplo"));
        invoker.Ejecutar(new InsertarLinea(editor, "línea 3: de patrón Command"));
        editor.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ── Deshacer última línea ──");
        invoker.Deshacer();
        editor.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ── Deshacer otra vez ──");
        invoker.Deshacer();
        editor.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ── Rehacer ──");
        invoker.Rehacer();
        editor.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Cada comando es un objeto que sabe ejecutarse y deshacerse.");
        Console.WriteLine("     Se pueden encolar, registrar en bitácora, o serializar.");
    }
}
