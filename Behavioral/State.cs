namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN STATE — Comportamiento según el estado
/// ============================================================================
/// Intención: permitir que un objeto altere su comportamiento cuando su estado
/// interno cambia. Parecerá que el objeto cambia de clase.
///
/// Analogía: el mismo botón de teléfono hace cosas distintas según si suena,
/// estás hablando o en espera.
///
/// Cuándo usarlo: flujos de pedidos, máquinas de estados de videojuegos,
/// workflows de aprobación.
///
/// Cuándo NO usarlo: con 2 estados triviales, un `enum` + `switch` es más
/// simple y suficiente.
///
/// USO REAL: Máquinas de estados en videojuegos (idle, running, jumping),
/// flujos de workflow (aprobación, rechazo), proceso de pedidos, conexiones TCP.
/// ============================================================================

// --- State ---
public interface IEstadoPedido
{
    string Nombre { get; }
    void Recibir(Pedido pedido);
    void Preparar(Pedido pedido);
    void Enviar(Pedido pedido);
    void Entregar(Pedido pedido);
}

// --- Context ---
public class Pedido
{
    public string Numero { get; }
    public IEstadoPedido Estado { get; private set; }

    public Pedido(string numero)
    {
        Numero = numero;
        Estado = PedidoRecibido.Instancia; // Estado inicial (singleton)
    }

    // Solo el propio Pedido puede cambiar de estado
    internal void CambiarEstado(IEstadoPedido nuevoEstado) => Estado = nuevoEstado;

    // Los métodos delegan al estado actual
    public void Recibir() => Estado.Recibir(this);
    public void Preparar() => Estado.Preparar(this);
    public void Enviar() => Estado.Enviar(this);
    public void Entregar() => Estado.Entregar(this);
}

// --- Estados concretos ---
public class PedidoRecibido : IEstadoPedido
{
    public static readonly PedidoRecibido Instancia = new();
    public string Nombre => "📥 Recibido";

    public void Recibir(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] Pedido recibido. Esperando confirmación...");

    public void Preparar(Pedido pedido)
    {
        Console.WriteLine($"  [{pedido.Numero}] Empezando a preparar el pedido...");
        pedido.CambiarEstado(PedidoEnPreparacion.Instancia);
    }

    public void Enviar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ No se puede enviar: el pedido aún no está preparado");

    public void Entregar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ No se puede entregar: el pedido aún no está enviado");
}

public class PedidoEnPreparacion : IEstadoPedido
{
    public static readonly PedidoEnPreparacion Instancia = new();
    public string Nombre => "👨‍🍳 En preparación";

    public void Recibir(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ Ya está en preparación, no se puede recibir");

    public void Preparar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] Ya se está preparando el pedido...");

    public void Enviar(Pedido pedido)
    {
        Console.WriteLine($"  [{pedido.Numero}] Empaquetando y enviando...");
        pedido.CambiarEstado(PedidoEnviado.Instancia);
    }

    public void Entregar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ No se puede entregar: el pedido aún no está enviado");
}

public class PedidoEnviado : IEstadoPedido
{
    public static readonly PedidoEnviado Instancia = new();
    public string Nombre => "🚚 Enviado";

    public void Recibir(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ Ya fue enviado, no se puede recibir");

    public void Preparar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ Ya fue preparado, no se puede preparar");

    public void Enviar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ Ya fue enviado, no se puede enviar");

    public void Entregar(Pedido pedido)
    {
        Console.WriteLine($"  [{pedido.Numero}] Entregando al cliente...");
        pedido.CambiarEstado(PedidoEntregado.Instancia);
    }
}

public class PedidoEntregado : IEstadoPedido
{
    public static readonly PedidoEntregado Instancia = new();
    public string Nombre => "✅ Entregado";

    public void Recibir(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ El pedido ya fue entregado");

    public void Preparar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ El pedido ya fue entregado");

    public void Enviar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ⚠️ El pedido ya fue entregado");

    public void Entregar(Pedido pedido) =>
        Console.WriteLine($"  [{pedido.Numero}] ✅ El pedido ya fue entregado");
}

public static class StateDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔄 STATE — Comportamiento que cambia según el estado\n");
        Console.WriteLine("  Escenario: Ciclo de vida de un pedido de e-commerce\n");

        var pedido = new Pedido("ORD-2024-001");

        Console.WriteLine($"  🏁 Estado inicial: {pedido.Estado.Nombre}");
        Console.WriteLine();

        Console.WriteLine("  ── Recibir pedido ──");
        pedido.Recibir();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        Console.WriteLine("  ── Preparar pedido ──");
        pedido.Preparar();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        Console.WriteLine("  ── Enviar pedido ──");
        pedido.Enviar();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        Console.WriteLine("  ── Entregar pedido ──");
        pedido.Entregar();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        Console.WriteLine("  ✅ Cada estado define CÓMO reacciona el pedido.");
        Console.WriteLine("     Sin State: if/else gigante con flags y validaciones.");
        Console.WriteLine("     Con State: cada estado es una clase independiente.");
    }
}
