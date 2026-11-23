namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN OBSERVER — Suscripción uno-a-muchos
/// ============================================================================
/// Intención: definir una dependencia uno-a-muchos entre objetos, de modo que
/// cuando un objeto cambia su estado, todos sus dependientes son notificados
/// automáticamente.
///
/// Analogía: canal de YouTube: te suscribes y recibes cada video nuevo sin
/// preguntar.
///
/// Cuándo usarlo: eventos de UI, notificaciones push, sincronizar vistas.
///
/// Cuándo NO usarlo: si un solo receptor escucha siempre, un callback directo
/// es más simple. Cuidado con el orden de desuscripción (memory leaks).
///
/// USO REAL: Eventos en UI (click, keypress), cotizaciones de bolsa en tiempo
/// real, notificaciones push, actualización de vistas en MVVM, events/delegates
/// de C#.
/// ============================================================================

// --- Sujeto (observable) ---
public interface IOrderService
{
    void Suscribir(IOrderObserver observador);
    void Desuscribir(IOrderObserver observador);
    void Notificar(string orderId, string producto);
}

// --- Observador ---
public interface IOrderObserver
{
    string Nombre { get; }
    void Recibir(string orderId, string producto);
}

// --- Sujeto concreto ---
public class OrderService : IOrderService
{
    private readonly List<IOrderObserver> _suscriptores = [];

    public void Suscribir(IOrderObserver observador)
    {
        _suscriptores.Add(observador);
        Console.WriteLine($"  📦 [OrderService] {observador.Nombre} se suscribió a los eventos");
    }

    public void Desuscribir(IOrderObserver observador)
    {
        _suscriptores.Remove(observador);
        Console.WriteLine($"  📦 [OrderService] {observador.Nombre} canceló su suscripción");
    }

    public void Notificar(string orderId, string producto)
    {
        Console.WriteLine($"    Avisando a {_suscriptores.Count} suscriptor(es)...");

        foreach (var suscriptor in _suscriptores)
            suscriptor.Recibir(orderId, producto);

        Console.WriteLine();
    }

    // El servicio termina de procesar un pedido
    public void PedidoConfirmado(string orderId, string producto)
    {
        Console.WriteLine($"\n  🔔 ORDER SERVICE: Pedido #{orderId} confirmado — {producto}");
        Notificar(orderId, producto);
    }
}

// --- Observadores concretos ---
public class EmailNotifier : IOrderObserver
{
    public string Nombre { get; }
    public EmailNotifier(string nombre) => Nombre = nombre;
    public void Recibir(string orderId, string producto) =>
        Console.WriteLine($"    ✉️ [{Nombre}] Enviando email: Tu pedido #{orderId} está confirmado");
}

public class StockUpdater : IOrderObserver
{
    public string Nombre => "Actualizador de stock";
    public void Recibir(string orderId, string producto) =>
        Console.WriteLine($"    📦 [Stock] Descontando 1 unidad de '{producto}' del inventario");
}

public class AnalyticsLogger : IOrderObserver
{
    public string Nombre { get; }
    public AnalyticsLogger(string nombre) => Nombre = nombre;
    public void Recibir(string orderId, string producto) =>
        Console.WriteLine($"    📊 [Analytics] Registrando evento: order_confirmed #{orderId}");
}

// --- Observador con filtro (solo le interesan los productos de electrónica) ---
public class ObservadorElectronica : IOrderObserver
{
    public string Nombre => "Departamento de electrónica";

    public void Recibir(string orderId, string producto)
    {
        var esElectronica = producto.Contains("Teclado") ||
                            producto.Contains("Mouse") ||
                            producto.Contains("Monitor") ||
                            producto.Contains("Laptop");

        if (!esElectronica)
        {
            Console.WriteLine($"    🔇 [{Nombre}] Ignora '{producto}' (no es electrónica)");
            return;
        }
        Console.WriteLine($"    🔌 [{Nombre}] ¡Producto de electrónica vendido: {producto}!");
    }
}

public static class ObserverDemo
{
    public static void Run()
    {
        Console.WriteLine("  👁️  OBSERVER — Suscripción y notificación en tiempo real\n");
        Console.WriteLine("  Escenario: El servicio de pedidos avisa cuando se confirma una orden\n");

        var service = new OrderService();

        var email = new EmailNotifier("Email");
        var stock = new StockUpdater();
        var analytics = new AnalyticsLogger("Analytics");
        var electronica = new ObservadorElectronica();

        service.Suscribir(email);
        service.Suscribir(stock);
        service.Suscribir(analytics);
        service.Suscribir(electronica);
        Console.WriteLine();

        service.PedidoConfirmado("ORD-001", "Teclado mecánico");
        service.PedidoConfirmado("ORD-002", "Camiseta básica");

        service.Desuscribir(analytics);
        Console.WriteLine();

        service.PedidoConfirmado("ORD-003", "Mouse gamer");

        Console.WriteLine("  ✅ Observer desacopla el emisor de los receptores.");
        Console.WriteLine("     Nuevos canales se agregan sin modificar el OrderService.");
    }
}
