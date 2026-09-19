namespace DesignPatterns.Behavioral;

/// PATRÓN STATE
/// ────────────
/// Permite que un objeto ALTERE su comportamiento cuando su ESTADO
/// interno cambia. Parecerá que el objeto cambia de clase.
/// Es como una máquina de estados finitos modelada con OOP.
///
/// USO REAL: Máquinas de estados en videojuegos (idle, running,
///           jumping), flujos de workflow (aprobación, rechazo),
///           proceso de pedidos, conexiones TCP.

// --- State ---
public interface IEstadoAnimo
{
    string Nombre { get; }
    void RecibirPedido(Chef chef);
    void Cocinar(Chef chef);
    void Descansar(Chef chef);
}

// --- Context ---
public class Chef
{
    public string Nombre { get; }
    public IEstadoAnimo Estado { get; private set; }

    public Chef(string nombre)
    {
        Nombre = nombre;
        Estado = ChefFeliz.Instancia; // Estado inicial (singleton)
    }

    // Solo el propio Chef (o clases del mismo assembly) pueden cambiar de humor
    internal void CambiarEstado(IEstadoAnimo nuevoEstado) => Estado = nuevoEstado;

    // Los métodos delegan al estado actual
    public void RecibirPedido() => Estado.RecibirPedido(this);
    public void Cocinar() => Estado.Cocinar(this);
    public void Descansar() => Estado.Descansar(this);
}

// --- Estados concretos ---

public class ChefFeliz : IEstadoAnimo
{
    public static readonly ChefFeliz Instancia = new();
    public string Nombre => "😄 Feliz";

    public void RecibirPedido(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} sonríe: \"¡Con gusto, un pedido más!\"");
    }

    public void Cocinar(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} cocina con pasión... pero la presión empieza a sentirse");
        chef.CambiarEstado(ChefEstresado.Instancia);
    }

    public void Descansar(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} se toma un café tranquilo");
    }
}

public class ChefEstresado : IEstadoAnimo
{
    public static readonly ChefEstresado Instancia = new();
    public string Nombre => "😰 Estresado";

    public void RecibirPedido(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} aprieta los dientes: \"¡¿OTRO pedido?!\"");
        chef.CambiarEstado(ChefEnojado.Instancia);
    }

    public void Cocinar(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} cocina rapidísimo, sin perder la técnica");
    }

    public void Descansar(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} respira hondo y recupera la calma");
        chef.CambiarEstado(ChefFeliz.Instancia);
    }
}

public class ChefEnojado : IEstadoAnimo
{
    public static readonly ChefEnojado Instancia = new();
    public string Nombre => "😡 Enojado";

    public void RecibirPedido(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} golpea la mesa: \"¡Ni un pedido más!\"");
    }

    public void Cocinar(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} cocina quemando el aceite de puro coraje");
    }

    public void Descansar(Chef chef)
    {
        Console.WriteLine($"  {chef.Nombre} se toma cinco minutos y vuelve a sonreír");
        chef.CambiarEstado(ChefFeliz.Instancia);
    }
}

public static class StateDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔄 STATE — Comportamiento que cambia según el estado\n");
        Console.WriteLine("  Escenario: El humor del chef cambia con la presión del servicio\n");

        var chef = new Chef("Chef Ramírez");

        Console.WriteLine($"  🏁 Estado inicial: {chef.Estado.Nombre}");
        Console.WriteLine();

        // El servicio avanza y el humor cambia solo
        Console.WriteLine("  ── Llega un pedido ──");
        chef.RecibirPedido();
        Console.WriteLine($"     Humor ahora: {chef.Estado.Nombre}\n");

        Console.WriteLine("  ── A cocinar ──");
        chef.Cocinar();
        Console.WriteLine($"     Humor ahora: {chef.Estado.Nombre}\n");

        Console.WriteLine("  ── Llega otro pedido con el chef estresado ──");
        chef.RecibirPedido();
        Console.WriteLine($"     Humor ahora: {chef.Estado.Nombre}\n");

        Console.WriteLine("  ── Cocinar enojado ──");
        chef.Cocinar();
        Console.WriteLine($"     Humor ahora: {chef.Estado.Nombre}\n");

        Console.WriteLine("  ── Un merecido descanso ──");
        chef.Descansar();
        Console.WriteLine($"     Humor ahora: {chef.Estado.Nombre}\n");

        Console.WriteLine("  ✅ Cada humor define CÓMO reacciona el chef.");
        Console.WriteLine("     Sin State: if/else gigante con flags y validaciones.");
        Console.WriteLine("     Con State: cada humor es una clase independiente.");
    }
}
