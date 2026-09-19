namespace DesignPatterns.Structural;

/// PATRÓN FACADE
/// ─────────────
/// Proporciona una interfaz SIMPLIFICADA para un subsistema complejo.
/// Es como la fachada de un edificio — esconde la complejidad interna
/// y expone solo lo que el cliente necesita.
///
/// USO REAL: APIs de frameworks (un controller de ASP.NET simplifica
///           el pipeline), bibliotecas de logging, sistemas de pedidos.

// --- Subsistema complejo #1: Inventario ---
public class Inventario
{
    private readonly Dictionary<string, int> _stock = new()
    {
        ["Pizza"] = 5,
        ["Ensalada"] = 50,
        ["Pasta"] = 20,
        ["Sopa"] = 8
    };

    public bool VerificarDisponibilidad(string platillo, int cantidad)
    {
        var disponible = _stock.GetValueOrDefault(platillo, 0);
        Console.WriteLine($"  [Inventario] {platillo}: pedidos {cantidad}, disponibles {disponible}");
        return disponible >= cantidad;
    }

    public void Reservar(string platillo, int cantidad)
    {
        if (_stock.ContainsKey(platillo))
            _stock[platillo] -= cantidad;
        Console.WriteLine($"  [Inventario] {cantidad}x {platillo} reservados. Quedan: {_stock.GetValueOrDefault(platillo, 0)}");
    }
}

// --- Subsistema complejo #2: Cocina ---
public class Cocina
{
    public bool Preparar(string platillo, int cantidad)
    {
        Console.WriteLine($"  [Cocina] Preparando {cantidad}x {platillo}...");
        Thread.Sleep(100); // simula el tiempo de cocción
        Console.WriteLine("  [Cocina] ✅ Platillo listo");
        return true;
    }
}

// --- Subsistema complejo #3: Repartidor ---
public class Repartidor
{
    private static int _guiaCount = 1000;

    public string AsignarReparto(string cliente, string direccion)
    {
        _guiaCount++;
        var guia = $"CR-{_guiaCount:D5}";
        Console.WriteLine($"  [Reparto] Guía {guia} asignada a {cliente}");
        Console.WriteLine($"  [Reparto] Dirección: {direccion}");
        return guia;
    }
}

// --- Subsistema complejo #4: Notificaciones ---
public class Notificaciones
{
    public void EnviarConfirmacion(string cliente, string guia)
    {
        Console.WriteLine($"  [Notif] ✉️ Correo de confirmación enviado a {cliente}");
        Console.WriteLine($"  [Notif] 📱 SMS con guía {guia} enviado");
    }
}

// --- FACADE: la interfaz simple que el cliente usa ---
public class ServicioDelivery
{
    private readonly Inventario _inventario;
    private readonly Cocina _cocina;
    private readonly Repartidor _repartidor;
    private readonly Notificaciones _notificaciones;

    // Constructor por defecto (conveniencia educativa)
    public ServicioDelivery()
        : this(new Inventario(), new Cocina(), new Repartidor(), new Notificaciones())
    { }

    // Constructor con inyección de dependencias (desacoplamiento real)
    public ServicioDelivery(Inventario inventario, Cocina cocina,
                            Repartidor repartidor, Notificaciones notificaciones)
    {
        _inventario = inventario;
        _cocina = cocina;
        _repartidor = repartidor;
        _notificaciones = notificaciones;
    }

    // Un solo método que orquesta todo el proceso complejo.
    // Devuelve la guía del pedido, o null si algo falló.
    public string? PedirCombo(string cliente, string platillo, int cantidad,
                              decimal precio, string direccion)
    {
        Console.WriteLine($"\n  🍽️  Procesando pedido de {cliente}...");
        Console.WriteLine($"  Platillo: {platillo} x{cantidad}");

        // Paso 1: Verificar inventario
        if (!_inventario.VerificarDisponibilidad(platillo, cantidad))
        {
            Console.WriteLine("  ❌ No hay ingredientes suficientes. Pedido cancelado.");
            return null;
        }

        // Paso 2: Reservar
        _inventario.Reservar(platillo, cantidad);

        // Paso 3: Cocinar
        if (!_cocina.Preparar(platillo, cantidad))
        {
            Console.WriteLine("  ❌ La cocina falló. Pedido cancelado.");
            return null;
        }

        // Paso 4: Asignar reparto
        var guia = _repartidor.AsignarReparto(cliente, direccion);

        // Paso 5: Notificar
        _notificaciones.EnviarConfirmacion(cliente, guia);

        return guia;
    }
}

public static class FacadeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏛️  FACADE — Interfaz simple para subsistemas complejos\n");
        Console.WriteLine("  Escenario: Delivery — el cliente solo llama a\n" +
                         "  'PedirCombo' y el Facade orquesta 4 subsistemas\n");

        var delivery = new ServicioDelivery();

        var guia = delivery.PedirCombo(
            cliente: "María Rodríguez",
            platillo: "Pizza",
            cantidad: 1,
            precio: 9500m,
            direccion: "Escazú, Plaza Itskatzú"
        );

        if (guia is not null)
        {
            Console.WriteLine($"\n  🎉 Pedido completado. Guía: {guia}");
        }

        // ── Pedido que falla: la fachada también orquesta el camino de error ──
        Console.WriteLine("\n  ── Pedido sin ingredientes suficientes ──");
        delivery.PedirCombo(
            cliente: "Pedro Gómez",
            platillo: "Pasta",
            cantidad: 100,   // solo hay 20 en stock
            precio: 7500m,
            direccion: "Cartago, Centro"
        );

        Console.WriteLine();
        Console.WriteLine("  ✅ El cliente solo ve 1 método.");
        Console.WriteLine("     La complejidad de inventario, cocina, reparto y");
        Console.WriteLine("     notificaciones queda oculta tras la fachada...");
        Console.WriteLine("     incluso cuando el pedido falla.");
    }
}
