namespace DesignPatterns.Behavioral;

/// PATRÓN MEDIATOR
/// ───────────────
/// Define un objeto que encapsula CÓMO interactúan un conjunto de objetos.
/// Promueve el acoplamiento débil evitando que los objetos se referencien
/// explícitamente entre sí. Es como un "switchboard" o una sala de chat.
///
/// USO REAL: Salas de chat, control de tráfico aéreo, coordinadores
///           de workflows, mediadores en frameworks UI (MVVM).

// --- Mediator ---
public interface IPaseCocina
{
    void Registrar(TrabajadorCocina trabajador);
    void Enviar(string mensaje, TrabajadorCocina remitente);
}

// --- Colega ---
public abstract class TrabajadorCocina
{
    public string Nombre { get; }
    private IPaseCocina? _pase;

    protected TrabajadorCocina(string nombre)
    {
        Nombre = nombre;
    }

    public void UnirseAlPase(IPaseCocina pase)
    {
        _pase = pase;
        _pase.Registrar(this);
    }

    public void Enviar(string mensaje)
    {
        Console.WriteLine($"  [{Nombre}] >> {mensaje}");
        _pase?.Enviar(mensaje, this);
    }

    // Por defecto, cada uno reacciona a su manera
    public abstract void Recibir(string mensaje, string remitente);
}

// --- Mediador concreto: el pase de cocina ---
public class PaseCocina : IPaseCocina
{
    private readonly List<TrabajadorCocina> _trabajadores = [];
    private readonly string _nombre;

    public PaseCocina(string nombre)
    {
        _nombre = nombre;
    }

    public void Registrar(TrabajadorCocina trabajador)
    {
        _trabajadores.Add(trabajador);
        Console.WriteLine($"  [Pase '{_nombre}'] {trabajador.Nombre} se sumó. Total: {_trabajadores.Count}");
    }

    public void Enviar(string mensaje, TrabajadorCocina remitente)
    {
        foreach (var trabajador in _trabajadores)
        {
            if (trabajador != remitente)
                trabajador.Recibir(mensaje, remitente.Nombre);
        }
    }
}

// --- Colegas concretos ---
public class Mesero : TrabajadorCocina
{
    public Mesero(string nombre) : base(nombre) { }

    public override void Recibir(string mensaje, string remitente)
    {
        Console.WriteLine($"    🧾 [{Nombre}] Anota lo de {remitente}: \"{mensaje}\"");
    }
}

public class Cocinero : TrabajadorCocina
{
    public Cocinero(string nombre) : base(nombre) { }

    public override void Recibir(string mensaje, string remitente)
    {
        Console.WriteLine($"    👨🍳 [{Nombre}] Prende la estufa por {remitente}: \"{mensaje}\"");
    }
}

public class Repartidor : TrabajadorCocina
{
    public Repartidor(string nombre) : base(nombre) { }

    public override void Recibir(string mensaje, string remitente)
    {
        Console.WriteLine($"    🛵 [{Nombre}] Arranca la moto por {remitente}: \"{mensaje}\"");
    }
}

public static class MediatorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🗣️  MEDIATOR — Comunicación centralizada\n");
        Console.WriteLine("  Escenario: El pase de cocina coordina a todo el equipo\n");

        var pase = new PaseCocina("Servicio de la noche");

        var luis = new Mesero("Luis");
        var ana = new Cocinero("Ana");
        var pedro = new Repartidor("Pedro");

        luis.UnirseAlPase(pase);
        ana.UnirseAlPase(pase);
        pedro.UnirseAlPase(pase);
        Console.WriteLine();

        luis.Enviar("Mesa 4 pidió pizza, ¡ya sale!");
        Console.WriteLine();

        ana.Enviar("Pizza lista en el pase.");
        Console.WriteLine();

        pedro.Enviar("Voy con el pedido de la mesa 4.");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los trabajadores NO se conocen entre sí.");
        Console.WriteLine("     Todo pasa por el pase (mediator).");
        Console.WriteLine("     Agregar/eliminar gente no rompe el código de nadie.");
    }
}
