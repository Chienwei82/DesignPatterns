namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN DECORATOR — Añadir responsabilidades dinámicamente
/// ============================================================================
/// Intención: envolver un objeto con otros que le agregan comportamiento, sin
/// herencia ni modificar la clase original. Los decoradores se apilan.
///
/// Analogía: vestirse en capas: camiseta, suéter, abrigo — la persona es la
/// misma, se agregan capas.
///
/// Cuándo usarlo: cuando tendrías una explosión de subclases (pastel con
/// glaseado+chispas+velitas...). Es la base de streams y middleware en .NET.
///
/// Cuándo NO usarlo: si solo hay una variante posible, la herencia simple es
/// más directa.
///
/// USO REAL: Middleware en ASP.NET Core (logging, auth, caching), Streams con
/// buffering/compresión en .NET, agregar funcionalidades a objetos GUI.
/// ============================================================================

// --- Componente abstracto ---
public interface INotificador
{
    void Enviar(string mensaje);
}

// --- Componente concreto ---
public class EmailNotificador : INotificador
{
    public void Enviar(string mensaje) =>
        Console.WriteLine($"  [Email] ✉️ Enviando: '{mensaje}'");
}

// --- Decorador base ---
public abstract class NotificadorDecorator : INotificador
{
    protected readonly INotificador _notificador;

    protected NotificadorDecorator(INotificador notificador) => _notificador = notificador;

    public virtual void Enviar(string mensaje) => _notificador.Enviar(mensaje);
}

// --- Decoradores concretos ---
public class ConRetraso : NotificadorDecorator
{
    public ConRetraso(INotificador notificador) : base(notificador) { }
    public override void Enviar(string mensaje)
    {
        Console.WriteLine("  [Retraso] ⏳ Esperando 1s antes de enviar...");
        Thread.Sleep(100);
        base.Enviar(mensaje);
    }
}

public class ConReintento : NotificadorDecorator
{
    public ConReintento(INotificador notificador) : base(notificador) { }
    public override void Enviar(string mensaje)
    {
        Console.WriteLine("  [Reintento] 🔁 Reintentando hasta 3 veces...");
        base.Enviar(mensaje);
    }
}

public class ConBitacora : NotificadorDecorator
{
    public ConBitacora(INotificador notificador) : base(notificador) { }
    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  [Bitácora] 📝 Registrando envío: '{mensaje}'");
        base.Enviar(mensaje);
    }
}

public static class DecoratorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🎄 DECORATOR — Añadir funcionalidad dinámicamente\n");
        Console.WriteLine("  Escenario: Notificaciones por email con extras\n");

        // Notificador básico
        INotificador notificador = new EmailNotificador();
        Mostrar(notificador, "Base");

        // Vamos decorando paso a paso
        notificador = new ConRetraso(notificador);
        Mostrar(notificador, "+ retraso");

        notificador = new ConReintento(notificador);
        Mostrar(notificador, "+ reintento");

        notificador = new ConBitacora(notificador);
        Mostrar(notificador, "+ bitácora");

        Console.WriteLine();
        Console.WriteLine($"  🧾 Resultado: {notificador.GetType().Name}");
        Console.WriteLine();

        // Otra combinación (solo bitácora + reintento)
        Console.WriteLine("  ── Otra combinación: Base + bitácora + reintento ──");
        INotificador otro = new ConReintento(new ConBitacora(new EmailNotificador()));
        Mostrar(otro, "Otra");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los decoradores se pueden combinar en cualquier orden.");
        Console.WriteLine("     No necesitas crear N clases por cada combinación posible.");
    }

    static void Mostrar(INotificador n, string etiqueta)
    {
        Console.WriteLine($"  [{etiqueta}]");
        n.Enviar("Tu pedido ha sido confirmado");
        Console.WriteLine();
    }
}
