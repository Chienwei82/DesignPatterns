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
    private readonly string _smtpServer;

    public NotificadorEmail(string smtpServer = "smtp.gmail.com")
    {
        _smtpServer = smtpServer;
    }

    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  ✉️ [EMAIL]    Conectando a {_smtpServer}...");
        Console.WriteLine($"  ✉️ [EMAIL]    Enviando correo a usuarios: \"{mensaje}\"");
    }
}

public class NotificadorSMS : Notificador
{
    private readonly string _numeroOrigen;

    public NotificadorSMS(string numeroOrigen = "+5068000-1234")
    {
        _numeroOrigen = numeroOrigen;
    }

    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  📱 [SMS]      Enviando desde {_numeroOrigen}: \"{mensaje}\"");
    }
}

public class NotificadorPush : Notificador
{
    private readonly string _appId;

    public NotificadorPush(string appId = "com.miempresa.app")
    {
        _appId = appId;
    }

    public override void Enviar(string mensaje)
    {
        Console.WriteLine($"  🔔 [PUSH]     App {_appId}: Notificación push enviada: \"{mensaje}\"");
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
    private readonly string _smtpServer;
    public FactoryEmail(string smtpServer = "smtp.gmail.com")
    {
        _smtpServer = smtpServer;
    }

    public override Notificador CrearNotificador()
    {
        Console.WriteLine($"    [FactoryEmail] Configurando servidor SMTP: {_smtpServer}");
        return new NotificadorEmail(_smtpServer);
    }
}

public class FactorySMS : NotificadorFactory
{
    private readonly string _numeroOrigen;
    public FactorySMS(string numeroOrigen = "+5068000-1234")
    {
        _numeroOrigen = numeroOrigen;
    }

    public override Notificador CrearNotificador()
    {
        Console.WriteLine($"    [FactorySMS] Número de origen: {_numeroOrigen}");
        return new NotificadorSMS(_numeroOrigen);
    }
}

public class FactoryPush : NotificadorFactory
{
    private readonly string _appId;
    public FactoryPush(string appId = "com.miempresa.app")
    {
        _appId = appId;
    }

    public override Notificador CrearNotificador()
    {
        Console.WriteLine($"    [FactoryPush] App ID: {_appId}");
        return new NotificadorPush(_appId);
    }
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
            new FactoryEmail("smtp.outlook.com"),
            new FactorySMS("+5068888-9999"),
            new FactoryPush("com.empresa.notifications")
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
