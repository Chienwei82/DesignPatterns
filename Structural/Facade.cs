namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN FACADE — Interfaz simple para subsistemas complejos
/// ============================================================================
/// Intención: exponer una interfaz simple que orchestre un subsistema complejo,
/// escondiendo sus detalles internos.
///
/// Analogía: recepción de un hotel: un solo punto para todo (check-in, equipaje,
/// conserjería).
///
/// Cuándo usarlo: cuando usar un sistema requiere llamar a varios subsistemas
/// en orden (procesar un pedido: inventario → pago → envío → notificación).
///
/// Cuándo NO usarlo: si los clientes siempre necesitan el control fino del
/// subsistema; una fachada innecesaria es una capa más de indirección.
///
/// USO REAL: APIs de frameworks (controller de ASP.NET simplifica el pipeline),
/// bibliotecas de logging, sistemas de pedidos.
/// ============================================================================

// --- Subsistema complejo #1: Inventario ---
public class Inventario
{
    private readonly Dictionary<string, int> _stock = new()
    {
        ["Teclado mecánico"] = 5,
        ["Mouse gamer"] = 50,
        ["Monitor 4K"] = 20,
        ["Laptop"] = 8
    };

    public bool VerificarDisponibilidad(string producto, int cantidad)
    {
        var disponible = _stock.GetValueOrDefault(producto, 0);
        Console.WriteLine($"  [Inventario] {producto}: pedidos {cantidad}, disponibles {disponible}");
        return disponible >= cantidad;
    }

    public void Reservar(string producto, int cantidad)
    {
        if (_stock.ContainsKey(producto))
            _stock[producto] -= cantidad;
        Console.WriteLine($"  [Inventario] {cantidad}x {producto} reservados. Quedan: {_stock.GetValueOrDefault(producto, 0)}");
    }
}

// --- Subsistema complejo #2: Pago ---
public class ProcesadorPago
{
    public bool Procesar(decimal monto, string metodo)
    {
        Console.WriteLine($"  [Pago] Procesando ¢{monto:N2} con {metodo}...");
        Console.WriteLine("  [Pago] ✅ Pago aprobado");
        return true;
    }
}

// --- Subsistema complejo #3: Envío ---
public class ServicioEnvio
{
    private static int _guiaCount = 1000;

    public string AsignarGuia(string cliente, string direccion)
    {
        _guiaCount++;
        var guia = $"ENV-{_guiaCount:D5}";
        Console.WriteLine($"  [Envío] Guía {guia} asignada a {cliente}");
        Console.WriteLine($"  [Envío] Dirección: {direccion}");
        return guia;
    }
}

// --- Subsistema complejo #4: Notificaciones ---
public class ServicioNotificaciones
{
    public void EnviarConfirmacion(string cliente, string guia)
    {
        Console.WriteLine($"  [Notif] ✉️ Correo de confirmación enviado a {cliente}");
        Console.WriteLine($"  [Notif] 📱 SMS con guía {guia} enviado");
    }
}

// --- FACADE: la interfaz simple que el cliente usa ---
public class TiendaFacade
{
    private readonly Inventario _inventario;
    private readonly ProcesadorPago _pago;
    private readonly ServicioEnvio _envio;
    private readonly ServicioNotificaciones _notificaciones;

    // Constructor por defecto (conveniencia educativa)
    public TiendaFacade()
        : this(new Inventario(), new ProcesadorPago(), new ServicioEnvio(), new ServicioNotificaciones())
    { }

    // Constructor con inyección de dependencias (desacoplamiento real)
    public TiendaFacade(Inventario inventario, ProcesadorPago pago,
                        ServicioEnvio envio, ServicioNotificaciones notificaciones)
    {
        _inventario = inventario;
        _pago = pago;
        _envio = envio;
        _notificaciones = notificaciones;
    }

    // Un solo método que orquesta todo el proceso complejo.
    // Devuelve la guía del pedido, o null si algo falló.
    public string? Comprar(string cliente, string producto, int cantidad,
                            string direccion, string metodoPago)
    {
        Console.WriteLine($"\n  🛒 Procesando compra de {cliente}...");
        Console.WriteLine($"  Producto: {producto} x{cantidad}");

        // Paso 1: Verificar inventario
        if (!_inventario.VerificarDisponibilidad(producto, cantidad))
        {
            Console.WriteLine("  ❌ No hay stock suficiente. Compra cancelada.");
            return null;
        }

        // Paso 2: Reservar
        _inventario.Reservar(producto, cantidad);

        // Paso 3: Procesar pago
        var subtotal = cantidad * 100m; // precio simulado
        if (!_pago.Procesar(subtotal, metodoPago))
        {
            Console.WriteLine("  ❌ El pago falló. Compra cancelada.");
            return null;
        }

        // Paso 4: Asignar envío
        var guia = _envio.AsignarGuia(cliente, direccion);

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
        Console.WriteLine("  Escenario: Compra online — el cliente solo llama a");
        Console.WriteLine("  'Comprar' y el Facade orquesta 4 subsistemas\n");

        var tienda = new TiendaFacade();

        var guia = tienda.Comprar(
            cliente: "María Rodríguez",
            producto: "Teclado mecánico",
            cantidad: 1,
            direccion: "Escazú, Plaza Itskatzú",
            metodoPago: "Tarjeta de crédito"
        );

        if (guia is not null)
        {
            Console.WriteLine($"\n  🎉 Compra completada. Guía: {guia}");
        }

        // ── Compra que falla: la fachada también orquesta el camino de error ──
        Console.WriteLine("\n  ── Compra sin stock suficiente ──");
        tienda.Comprar(
            cliente: "Pedro Gómez",
            producto: "Laptop",
            cantidad: 100,   // solo hay 8 en stock
            direccion: "Cartago, Centro",
            metodoPago: "PayPal"
        );

        Console.WriteLine();
        Console.WriteLine("  ✅ El cliente solo ve 1 método.");
        Console.WriteLine("     La complejidad de inventario, pago, envío y");
        Console.WriteLine("     notificaciones queda oculta tras la fachada...");
        Console.WriteLine("     incluso cuando la compra falla.");
    }
}
