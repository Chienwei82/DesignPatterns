namespace DesignPatterns.Creational;

/// PATRÓN FACTORY METHOD
/// ─────────────────────
/// Define una interfaz para crear objetos, pero DELEGA a las subclases
/// qué clase concreta instanciar. Permite que una clase difiera la
/// creación de sus objetos a las subclases.
///
/// USO REAL: Frameworks ORM (DbConnection según la BD), generación de
///           documentos (PDF, Excel, Word), factories de controles UI.

// --- Producto abstracto ---
public abstract class Notificador
{
    public abstract void Enviar(string mensaje);
}

// --- Productos concretos ---
public class NotificadorEmail : Notificador
{
    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  ✉️ [EMAIL]    Enviando correo a usuarios: \"{mensaje}\"");
        // Aquí iría SmtpClient.Send(...)
    }
}

public class NotificadorSMS : Notificador
{
    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  📱 [SMS]      Enviando mensaje de texto: \"{mensaje}\"");
        // Aquí iría Twilio API
    }
}

public class NotificadorPush : Notificador
{
    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  🔔 [PUSH]     Notificación push enviada: \"{mensaje}\"");
        // Aquí iría Firebase Cloud Messaging
    }
}

// --- Creator abstracto (Factory Method) ---
public abstract class NotificadorFactory
{
    // Factory Method — las subclases deciden qué crear
    public abstract Notificador CrearNotificador();

    // Método que usa el producto creado
    public void Notificar(string mensaje)
    {
        var notificador = CrearNotificador();
        Console.WriteLine($"  Preparando envío...");
        Thread.Sleep(100);
        notificador.Enviar(mensaje);
    }
}

// --- Creators concretos ---
public class FactoryEmail : NotificadorFactory
{
    public override Notificador CrearNotificador() => new NotificadorEmail();
}

public class FactorySMS : NotificadorFactory
{
    public override Notificador CrearNotificador() => new NotificadorSMS();
}

public class FactoryPush : NotificadorFactory
{
    public override Notificador CrearNotificador() => new NotificadorPush();
}

public static class FactoryMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭 FACTORY METHOD — Creación delegada a subclases\n");
        Console.WriteLine("  Escenario: Sistema de notificaciones multicanal\n");

        // El cliente no sabe qué clase concreta se crea
        var factories = new NotificadorFactory[]
        {
            new FactoryEmail(),
            new FactorySMS(),
            new FactoryPush()
        };

        string[] canales = { "Email", "SMS", "Push" };
        for (int i = 0; i < factories.Length; i++)
        {
            Console.WriteLine($"  [{i + 1}] Usando canal: {canales[i]}");
            factories[i].Notificar("Su pedido ha sido enviado 🎉");
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El cliente depende de la abstracción (Notificador),");
        Console.WriteLine("     no de las clases concretas. Agregar un nuevo canal");
        Console.WriteLine("     solo requiere crear una nueva factory.");
    }
}
