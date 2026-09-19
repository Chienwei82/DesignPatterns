namespace DesignPatterns.Behavioral;

/// PATRÓN OBSERVER
/// ───────────────
/// Define una dependencia UNO-A-MUCHOS entre objetos, de modo que
/// cuando UN objeto cambia su estado, TODOS sus dependientes son
/// notificados automáticamente. Es como un sistema de suscripción.
///
/// USO REAL: Eventos en UI (click, keypress), cotizaciones de bolsa
///           en tiempo real, notificaciones push, actualización de
///           vistas en MVVM, el patrón event/delegate de C#.

// --- Sujeto (observable) ---
public interface IPublicadorPedidos
{
    void Suscribir(IObservadorPedido observador);
    void Desuscribir(IObservadorPedido observador);
    void Notificar(string platillo, string mesa);
}

// --- Observador ---
public interface IObservadorPedido
{
    string Nombre { get; }
    void RecibirPedido(string platillo, string mesa);
}

// --- Sujeto concreto ---
public class Cocina : IPublicadorPedidos
{
    private readonly List<IObservadorPedido> _suscriptores = [];

    public void Suscribir(IObservadorPedido observador)
    {
        _suscriptores.Add(observador);
        Console.WriteLine($"  🍳 [Cocina] {observador.Nombre} se suscribió a los pedidos");
    }

    public void Desuscribir(IObservadorPedido observador)
    {
        _suscriptores.Remove(observador);
        Console.WriteLine($"  🍳 [Cocina] {observador.Nombre} canceló su suscripción");
    }

    public void Notificar(string platillo, string mesa)
    {
        Console.WriteLine($"    Avisando a {_suscriptores.Count} suscriptor(es)...");

        foreach (var suscriptor in _suscriptores)
            suscriptor.RecibirPedido(platillo, mesa);

        Console.WriteLine();
    }

    // Método de alto nivel: la cocina terminó un platillo
    public void PedidoListo(string platillo, string mesa)
    {
        Console.WriteLine($"\n  🔔 COCINA: {platillo} listo para la mesa {mesa}");
        Notificar(platillo, mesa);
    }
}

// --- Observadores concretos ---

public class MeseroNotificador : IObservadorPedido
{
    public string Nombre { get; }

    public MeseroNotificador(string nombre) => Nombre = nombre;

    public void RecibirPedido(string platillo, string mesa)
    {
        Console.WriteLine($"    🏃 [{Nombre}] Llevando {platillo} a la mesa {mesa}");
    }
}

public class PantallaSalon : IObservadorPedido
{
    public string Nombre => "Pantalla del salón";

    public void RecibirPedido(string platillo, string mesa)
    {
        Console.WriteLine($"    🖥️  [Pantalla] Mostrando: \"{platillo} → mesa {mesa}\"");
    }
}

public class AppCliente : IObservadorPedido
{
    public string Nombre { get; }

    public AppCliente(string nombre) => Nombre = nombre;

    public void RecibirPedido(string platillo, string mesa)
    {
        Console.WriteLine($"    📱 [App de {Nombre}] Notificación: \"{platillo} va en camino\"");
    }
}

// --- Observador con filtro (solo le interesan los postres) ---
public class ObservadorDePostres : IObservadorPedido
{
    public string Nombre => "Chef de postres";

    public void RecibirPedido(string platillo, string mesa)
    {
        if (!platillo.Contains("Pastel") && !platillo.Contains("Helado"))
        {
            Console.WriteLine($"    🔇 [{Nombre}] Ignora '{platillo}' (no es postre)");
            return;
        }
        Console.WriteLine($"    🍰 [{Nombre}] ¡Postre para la mesa {mesa}: {platillo}!");
    }
}

public static class ObserverDemo
{
    public static void Run()
    {
        Console.WriteLine("  👁️  OBSERVER — Suscripción y notificación en tiempo real\n");
        Console.WriteLine("  Escenario: La cocina avisa cuando un pedido está listo\n");

        // Sujeto
        var cocina = new Cocina();

        // Crear suscriptores
        var luis = new MeseroNotificador("Luis");
        var pantalla = new PantallaSalon();
        var app = new AppCliente("María");
        var postres = new ObservadorDePostres();

        // Suscribir
        cocina.Suscribir(luis);
        cocina.Suscribir(pantalla);
        cocina.Suscribir(app);
        cocina.Suscribir(postres);
        Console.WriteLine();

        // Preparar pedidos
        cocina.PedidoListo("Pizza margarita", "Mesa 4");
        cocina.PedidoListo("Pastel de chocolate", "Mesa 2");

        // Cancelar suscripción
        cocina.Desuscribir(app);
        Console.WriteLine();

        // Otro pedido — la app ya no recibe
        cocina.PedidoListo("Ensalada césar", "Mesa 7");

        Console.WriteLine("  ✅ Observer desacopla el emisor de los receptores.");
        Console.WriteLine("     Nuevos canales se agregan sin modificar la cocina.");
    }
}
