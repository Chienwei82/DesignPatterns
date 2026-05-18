namespace DesignPatterns.Behavioral;

/// PATRÓN CHAIN OF RESPONSIBILITY
/// ─────────────────────────────
/// Permite pasar solicitudes a lo largo de una CADENA de manejadores.
/// Cada manejador decide si procesa la solicitud o la pasa al
/// siguiente eslabón. Desacopla el emisor del receptor.
///
/// USO REAL: Middleware en ASP.NET Core, filtros de validación,
///           aprobaciones jerárquicas, soporte técnico escalable,
///           manejadores de eventos en sistemas distribuidos.

// --- Handler abstracto ---
public abstract class SoporteHandler
{
    protected SoporteHandler? _siguiente;

    public void EstablecerSiguiente(SoporteHandler siguiente)
    {
        _siguiente = siguiente;
    }

    public virtual void Manejar(TicketSoporte ticket)
    {
        if (_siguiente != null)
        {
            _siguiente.Manejar(ticket);
        }
        else
        {
            Console.WriteLine($"    ❌ Ticket #{ticket.Id}: NADIE pudo manejar '{ticket.Titulo}'");
        }
    }
}

public class TicketSoporte
{
    public int Id { get; }
    public string Titulo { get; }
    public int Severidad { get; } // 1=Leve, 2=Moderada, 3=Crítica, 4=Empresarial

    public TicketSoporte(int id, string titulo, int severidad)
    {
        Id = id;
        Titulo = titulo;
        Severidad = severidad;
    }
}

// --- Handlers concretos ---
public class SoporteNivel1 : SoporteHandler
{
    public override void Manejar(TicketSoporte ticket)
    {
        if (ticket.Severidad <= 1)
        {
            Console.WriteLine($"    ✅ [Nivel 1] Ticket #{ticket.Id}: '{ticket.Titulo}' resuelto (FAQ + reboot)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Nivel 1] Ticket #{ticket.Id}: Escalando a Nivel 2...");
            base.Manejar(ticket);
        }
    }
}

public class SoporteNivel2 : SoporteHandler
{
    public override void Manejar(TicketSoporte ticket)
    {
        if (ticket.Severidad <= 2)
        {
            Console.WriteLine($"    ✅ [Nivel 2] Ticket #{ticket.Id}: '{ticket.Titulo}' resuelto (configuración avanzada)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Nivel 2] Ticket #{ticket.Id}: Escalando a Nivel 3...");
            base.Manejar(ticket);
        }
    }
}

public class SoporteNivel3 : SoporteHandler
{
    public override void Manejar(TicketSoporte ticket)
    {
        if (ticket.Severidad <= 3)
        {
            Console.WriteLine($"    ✅ [Nivel 3] Ticket #{ticket.Id}: '{ticket.Titulo}' resuelto (hotfix de emergencia)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Nivel 3] Ticket #{ticket.Id}: Escalando a Director...");
            base.Manejar(ticket);
        }
    }
}

public class DirectorSoporte : SoporteHandler
{
    public override void Manejar(TicketSoporte ticket)
    {
        Console.WriteLine($"    ✅ [Director] Ticket #{ticket.Id}: '{ticket.Titulo}' atendido personalmente por el Director");
    }
}

public static class ChainOfResponsibilityDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔗 CHAIN OF RESPONSIBILITY — Cadena de manejadores\n");
        Console.WriteLine("  Escenario: Soporte técnico escalable\n");

        // Construir la cadena
        var nivel1 = new SoporteNivel1();
        var nivel2 = new SoporteNivel2();
        var nivel3 = new SoporteNivel3();
        var director = new DirectorSoporte();

        nivel1.EstablecerSiguiente(nivel2);
        nivel2.EstablecerSiguiente(nivel3);
        nivel3.EstablecerSiguiente(director);

        // Tickets de prueba
        var tickets = new TicketSoporte[]
        {
            new(101, "¿Cómo reinicio mi contraseña?", 1),
            new(102, "La VPN no conecta desde casa", 2),
            new(103, "Servidor de producción caído", 3),
            new(104, "Ciberataque en curso — datos expuestos", 4),
        };

        foreach (var ticket in tickets)
        {
            Console.WriteLine($"  ── Ticket #{ticket.Id} (Severidad {ticket.Severidad}): {ticket.Titulo} ──");
            nivel1.Manejar(ticket);
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Cada nivel decide si puede resolverlo o pasa al siguiente.");
        Console.WriteLine("     El emisor (cliente) no sabe QUIÉN lo resolverá.");
        Console.WriteLine("     Se pueden reordenar, agregar o quitar niveles sin tocar el cliente.");
    }
}
