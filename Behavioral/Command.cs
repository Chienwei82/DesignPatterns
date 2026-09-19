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
public interface IComandoComanda
{
    string Nombre { get; }
    void Ejecutar();
    void Deshacer();
}

// --- Receiver: la comanda real del restaurante ---
public class Comanda
{
    private readonly List<string> _platillos = [];

    public void Agregar(string platillo)
    {
        _platillos.Add(platillo);
        Console.WriteLine($"    ➕ Anotado: \"{platillo}\"");
    }

    // Quita el último platillo anotado y lo devuelve (o "" si está vacía)
    public string Quitar()
    {
        if (_platillos.Count == 0)
        {
            Console.WriteLine("    ⚠️  La comanda está vacía");
            return "";
        }

        var ultimo = _platillos[^1];
        _platillos.RemoveAt(_platillos.Count - 1);
        Console.WriteLine($"    ➖ Tachado: \"{ultimo}\"");
        return ultimo;
    }

    public void Mostrar()
    {
        var texto = _platillos.Count > 0 ? string.Join(", ", _platillos) : "(vacía)";
        Console.WriteLine($"    ── Comanda actual: {texto}");
    }
}

// --- Commands concretos ---
public class ComandoAgregar : IComandoComanda
{
    private readonly Comanda _comanda;
    private readonly string _platillo;
    public string Nombre => $"Agregar \"{_platillo}\"";

    public ComandoAgregar(Comanda comanda, string platillo)
    {
        _comanda = comanda;
        _platillo = platillo;
    }

    public void Ejecutar() => _comanda.Agregar(_platillo);

    public void Deshacer() => _comanda.Quitar();
}

public class ComandoQuitar : IComandoComanda
{
    private readonly Comanda _comanda;
    private string _platilloQuitado = "";
    public string Nombre => "Quitar último platillo";

    public ComandoQuitar(Comanda comanda) => _comanda = comanda;

    public void Ejecutar() => _platilloQuitado = _comanda.Quitar();

    public void Deshacer()
    {
        if (!string.IsNullOrEmpty(_platilloQuitado))
            _comanda.Agregar(_platilloQuitado);
    }
}

// --- Invoker: maneja el historial y la ejecución ---
public class MeseroComandas
{
    private readonly Stack<IComandoComanda> _historial = new();
    private readonly Stack<IComandoComanda> _rehacer = new();

    public void Ejecutar(IComandoComanda comando)
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
        Console.WriteLine("  Escenario: El mesero anota y corrige la comanda\n");

        var comanda = new Comanda();
        var mesero = new MeseroComandas();

        // Secuencia de anotaciones
        mesero.Ejecutar(new ComandoAgregar(comanda, "Pizza margarita"));
        comanda.Mostrar();
        Console.WriteLine();

        mesero.Ejecutar(new ComandoAgregar(comanda, "Ensalada césar"));
        mesero.Ejecutar(new ComandoAgregar(comanda, "Jugo natural"));
        comanda.Mostrar();
        Console.WriteLine();

        // El cliente se arrepiente del jugo
        Console.WriteLine("  ── Deshacer último platillo ──");
        mesero.Deshacer();
        comanda.Mostrar();
        Console.WriteLine();

        // Y también de la ensalada
        Console.WriteLine("  ── Deshacer otra vez ──");
        mesero.Deshacer();
        comanda.Mostrar();
        Console.WriteLine();

        // Se arrepiente de haberse arrepentido
        Console.WriteLine("  ── Rehacer ──");
        mesero.Rehacer();
        comanda.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Cada comando es un objeto que sabe ejecutarse y deshacerse.");
        Console.WriteLine("     Se pueden encolar, registrar en bitácora, o serializar.");
    }
}
