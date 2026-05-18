namespace DesignPatterns.Behavioral;

/// PATRÓN COMMAND
/// ──────────────
/// Encapsula una SOLICITUD como un OBJETO, permitiendo parametrizar
/// clientes con diferentes solicitudes, encolar o registrar solicitudes,
/// y soportar operaciones reversibles (UNDO).
///
/// USO REAL: Botones de toolbar en editores, operaciones CRUD en
///           sistemas transaccionales, colas de trabajos (jobs),
///           macros de teclado, historial de Ctrl+Z.

// --- Command ---
public interface IComando
{
    string Nombre { get; }
    void Ejecutar();
    void Deshacer();
}

// --- Receiver: la lógica de negocio real ---
public class EditorTexto
{
    private string _contenido = "";
    private string _portapapeles = "";

    public void Insertar(string texto)
    {
        _contenido += texto;
        Console.WriteLine($"    📝 Insertado: \"{texto}\"");
    }

    public void InsertarEn(int posicion, string texto)
    {
        if (posicion >= 0 && posicion <= _contenido.Length)
        {
            _contenido = _contenido.Insert(posicion, texto);
            Console.WriteLine($"    📝 Insertado en pos {posicion}: \"{texto}\"");
        }
    }

    public void Borrar(int cantidad)
    {
        if (cantidad <= _contenido.Length)
        {
            var borrado = _contenido[^cantidad..];
            _contenido = _contenido[..^cantidad];
            Console.WriteLine($"    🗑️  Borrado: \"{borrado}\"");
        }
    }

    public void Cortar(int desde, int hasta)
    {
        if (desde >= 0 && hasta <= _contenido.Length && desde < hasta)
        {
            _portapapeles = _contenido[desde..hasta];
            _contenido = _contenido[..desde] + _contenido[hasta..];
            Console.WriteLine($"    ✂️  Cortado: \"{_portapapeles}\"");
        }
    }

    public void Pegar()
    {
        if (!string.IsNullOrEmpty(_portapapeles))
        {
            _contenido += _portapapeles;
            Console.WriteLine($"    📋 Pegado: \"{_portapapeles}\"");
        }
    }

    public string Contenido => _contenido;

    public void Mostrar()
    {
        Console.WriteLine($"    ── Contenido actual: \"{_contenido}\"");
    }
}

// --- Commands concretos ---
public class ComandoInsertar : IComando
{
    private readonly EditorTexto _editor;
    private readonly string _texto;
    public string Nombre => $"Insertar \"{_texto}\"";

    public ComandoInsertar(EditorTexto editor, string texto)
    {
        _editor = editor;
        _texto = texto;
    }

    public void Ejecutar() => _editor.Insertar(_texto);

    public void Deshacer() => _editor.Borrar(_texto.Length);
}

public class ComandoCortar : IComando
{
    private readonly EditorTexto _editor;
    private readonly int _desde;
    private readonly int _hasta;
    private string _textoCortado = "";
    public string Nombre => $"Cortar [{_desde}..{_hasta}]";

    public ComandoCortar(EditorTexto editor, int desde, int hasta)
    {
        _editor = editor;
        _desde = desde;
        _hasta = hasta;
    }

    public void Ejecutar()
    {
        // Guardar el texto antes de cortar para poder deshacer
        var contenidoPrevio = _editor.Contenido;
        _editor.Cortar(_desde, _hasta);
        // El texto cortado queda en el portapapeles del editor
        // Calculamos qué se cortó comparando antes y después
        if (_hasta <= contenidoPrevio.Length && _desde < _hasta)
            _textoCortado = contenidoPrevio[_desde.._hasta];
    }

    public void Deshacer()
    {
        Console.WriteLine($"    ↩️ [Undo] Restaurando texto cortado...");
        _editor.InsertarEn(_desde, _textoCortado);
    }
}

public class ComandoPegar : IComando
{
    private readonly EditorTexto _editor;
    private int _longitudPegada = 0;
    public string Nombre => "Pegar";

    public ComandoPegar(EditorTexto editor) => _editor = editor;

    public void Ejecutar()
    {
        var textoPrevio = _editor.Contenido;
        _editor.Pegar();
        _longitudPegada = _editor.Contenido.Length - textoPrevio.Length;
    }

    public void Deshacer()
    {
        Console.WriteLine($"    ↩️ [Undo] Despegando {_longitudPegada} caracteres...");
        _editor.Borrar(_longitudPegada);
    }
}

// --- Invoker: maneja el historial y la ejecución ---
public class HistorialComandos
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
        Console.WriteLine("  Escenario: Editor de texto con Ctrl+Z / Ctrl+Y\n");

        var editor = new EditorTexto();
        var historial = new HistorialComandos();

        // Secuencia de edición
        historial.Ejecutar(new ComandoInsertar(editor, "Hola mundo cruel"));
        editor.Mostrar();
        Console.WriteLine();

        // Cortar "mundo " (posiciones 5 a 11)
        historial.Ejecutar(new ComandoCortar(editor, 5, 11));
        editor.Mostrar();
        Console.WriteLine();

        // Pegar al final
        historial.Ejecutar(new ComandoPegar(editor));
        editor.Mostrar();
        Console.WriteLine();

        // Undo pegar
        Console.WriteLine("  ── Deshacer Pegar (Ctrl+Z) ──");
        historial.Deshacer();
        editor.Mostrar();
        Console.WriteLine();

        // Undo cortar
        Console.WriteLine("  ── Deshacer Cortar (Ctrl+Z) ──");
        historial.Deshacer();
        editor.Mostrar();
        Console.WriteLine();

        // Redo cortar
        Console.WriteLine("  ── Rehacer Cortar (Ctrl+Y) ──");
        historial.Rehacer();
        editor.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Cada comando es un objeto que sabe ejecutarse y deshacerse.");
        Console.WriteLine("     Se pueden encolar, registrar en bitácora, o serializar.");
    }
}
