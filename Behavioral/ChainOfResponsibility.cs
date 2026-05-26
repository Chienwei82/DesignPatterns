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

public enum SeveridadTicket
{
    Leve = 1,
    Moderada,
    Critica,
    Empresarial
}

public record TicketSoporte(int Id, string Titulo, SeveridadTicket Severidad);

// --- Handlers concretos ---
public class SoporteNivel1 : SoporteHandler
{
    public override void Manejar(TicketSoporte ticket)
    {
        if (ticket.Severidad <= SeveridadTicket.Leve)
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
        if (ticket.Severidad <= SeveridadTicket.Moderada)
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
        if (ticket.Severidad <= SeveridadTicket.Critica)
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
            new(101, "¿Cómo reinicio mi contraseña?", SeveridadTicket.Leve),
            new(102, "La VPN no conecta desde casa", SeveridadTicket.Moderada),
            new(103, "Servidor de producción caído", SeveridadTicket.Critica),
            new(104, "Ciberataque en curso — datos expuestos", SeveridadTicket.Empresarial),
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
