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
public interface IEstadoPedido
{
    string Nombre { get; }
    void Pagar(Pedido pedido);
    void Enviar(Pedido pedido);
    void Entregar(Pedido pedido);
    void Cancelar(Pedido pedido);
}

// --- Context ---
public class Pedido
{
    public int Id { get; }
    public string Producto { get; }
    public IEstadoPedido Estado { get; set; }

    public Pedido(int id, string producto)
    {
        Id = id;
        Producto = producto;
        Estado = EstadoNuevo.Instancia; // Estado inicial (singleton)
    }

    // Los métodos delegan al estado actual
    public void Pagar() => Estado.Pagar(this);
    public void Enviar() => Estado.Enviar(this);
    public void Entregar() => Estado.Entregar(this);
    public void Cancelar() => Estado.Cancelar(this);
}

// --- Estados concretos ---

public class EstadoNuevo : IEstadoPedido
{
    public static readonly EstadoNuevo Instancia = new();
    public string Nombre => "🆕 Nuevo";

    public void Pagar(Pedido pedido)
    {
        Console.WriteLine($"  💳 Pedido #{pedido.Id}: Pago confirmado");
        pedido.Estado = EstadoPagado.Instancia;
    }

    public void Enviar(Pedido pedido)
    {
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: No se puede enviar — aún no se ha pagado");
    }

    public void Entregar(Pedido pedido)
    {
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: No se puede entregar — aún no se ha pagado");
    }

    public void Cancelar(Pedido pedido)
    {
        Console.WriteLine($"  ❌ Pedido #{pedido.Id}: Cancelado por el cliente");
        pedido.Estado = EstadoCancelado.Instancia;
    }
}

public class EstadoPagado : IEstadoPedido
{
    public static readonly EstadoPagado Instancia = new();
    public string Nombre => "💰 Pagado";

    public void Pagar(Pedido pedido)
    {
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Ya está pagado");
    }

    public void Enviar(Pedido pedido)
    {
        Console.WriteLine($"  📦 Pedido #{pedido.Id}: Enviado al domicilio");
        pedido.Estado = EstadoEnviado.Instancia;
    }

    public void Entregar(Pedido pedido)
    {
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: No se puede entregar — no se ha enviado aún");
    }

    public void Cancelar(Pedido pedido)
    {
        Console.WriteLine($"  ↩️  Pedido #{pedido.Id}: Cancelado — reembolso procesado");
        pedido.Estado = EstadoCancelado.Instancia;
    }
}

public class EstadoEnviado : IEstadoPedido
{
    public static readonly EstadoEnviado Instancia = new();
    public string Nombre => "🚚 Enviado";

    public void Pagar(Pedido pedido)
    {
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Ya está pagado y enviado");
    }

    public void Enviar(Pedido pedido)
    {
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Ya fue enviado");
    }

    public void Entregar(Pedido pedido)
    {
        Console.WriteLine($"  ✅ Pedido #{pedido.Id}: ENTREGADO — ¡Gracias por su compra!");
        pedido.Estado = EstadoEntregado.Instancia;
    }

    public void Cancelar(Pedido pedido)
    {
        Console.WriteLine($"  ↩️  Pedido #{pedido.Id}: Cancelado durante envío (devolución en tránsito)");
        pedido.Estado = EstadoCancelado.Instancia;
    }
}

public class EstadoEntregado : IEstadoPedido
{
    public static readonly EstadoEntregado Instancia = new();
    public string Nombre => "✅ Entregado";

    public void Pagar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Pedido completado");

    public void Enviar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Pedido completado");

    public void Entregar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Ya fue entregado");

    public void Cancelar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: No se puede cancelar — ya entregado");
}

public class EstadoCancelado : IEstadoPedido
{
    public static readonly EstadoCancelado Instancia = new();
    public string Nombre => "❌ Cancelado";

    public void Pagar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Fue cancelado. No se puede pagar");

    public void Enviar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Fue cancelado. No se puede enviar");

    public void Entregar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Fue cancelado. No se puede entregar");

    public void Cancelar(Pedido pedido) =>
        Console.WriteLine($"  ⚠️  Pedido #{pedido.Id}: Ya está cancelado");
}

public static class StateDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔄 STATE — Comportamiento que cambia según el estado\n");
        Console.WriteLine("  Escenario: Ciclo de vida de un pedido (Nuevo→Pagado→Enviado→Entregado)\n");

        var pedido = new Pedido(1001, "Laptop Dell XPS 15");

        // Mostrar estado inicial
        Console.WriteLine($"  🏁 Estado inicial: {pedido.Estado.Nombre}");
        Console.WriteLine();

        // Secuencia correcta
        Console.WriteLine("  ── Flujo normal ──");
        pedido.Pagar();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        pedido.Enviar();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        pedido.Entregar();
        Console.WriteLine($"     Estado ahora: {pedido.Estado.Nombre}\n");

        // Intentar acciones inválidas (el estado actual las rechaza)
        Console.WriteLine("  ── Acciones inválidas (el estado las rechaza) ──");
        pedido.Enviar();   // Ya entregado, no se puede
        pedido.Cancelar(); // Ya entregado, no se puede
        Console.WriteLine();

        // Segundo pedido para mostrar cancelación
        Console.WriteLine("  ── Cancelación en estado Pagado ──");
        var pedido2 = new Pedido(1002, "Mouse Inalámbrico");

        pedido2.Pagar();
        Console.WriteLine($"     Estado: {pedido2.Estado.Nombre}");
        pedido2.Cancelar(); // Reembolso
        Console.WriteLine($"     Estado: {pedido2.Estado.Nombre}");

        Console.WriteLine();
        Console.WriteLine("  ✅ Cada estado define QUÉ se puede hacer y QUÉ no.");
        Console.WriteLine("     Sin State: if/else gigante con flags y validaciones.");
        Console.WriteLine("     Con State: cada estado es una clase independiente.");
    }
}
