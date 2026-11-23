namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN CHAIN OF RESPONSIBILITY — Cadena de manejadores
/// ============================================================================
/// Intención: pasar solicitudes a lo largo de una cadena de manejadores.
/// Cada manejador decide si procesa la solicitud o la pasa al siguiente
/// eslabón. Desacopla al emisor del receptor.
///
/// Analogía: una queja en el restaurante: el mesero intenta resolverla; si no
/// puede, escala al jefe de cocina, luego al gerente y por último al dueño. En
/// este proyecto: un ticket de soporte que escala Nivel 1 → Nivel 2 → Gerente
/// → Director.
///
/// Cuándo usarlo: middleware, validaciones encadenadas, escalado de aprobaciones.
///
/// Cuándo NO usarlo: si siempre sabes quién maneja la solicitud, un `switch`
/// o llamada directa es más claro.
///
/// USO REAL: Middleware en ASP.NET Core, filtros de validación, aprobaciones
/// jerárquicas, soporte técnico escalable.
/// ============================================================================

// --- Handler abstracto ---
public abstract class ManejadorTicket
{
    protected ManejadorTicket? _siguiente;

    public void EstablecerSiguiente(ManejadorTicket siguiente) => _siguiente = siguiente;

    public virtual void Manejar(Ticket ticket)
    {
        if (_siguiente != null)
            _siguiente.Manejar(ticket);
        else
            Console.WriteLine($"    ❌ Ticket #{ticket.Id}: NADIE pudo atender '{ticket.Asunto}'");
    }
}

public enum PrioridadTicket
{
    Baja = 1,
    Media,
    Alta,
    Urgente
}

public record Ticket(int Id, string Asunto, PrioridadTicket Prioridad);

// --- Handlers concretos ---
public class SoporteNivel1 : ManejadorTicket
{
    public override void Manejar(Ticket ticket)
    {
        if (ticket.Prioridad <= PrioridadTicket.Baja)
        {
            Console.WriteLine($"    ✅ [Nivel 1] Ticket #{ticket.Id}: '{ticket.Asunto}' resuelto (FAQ)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Nivel 1] Ticket #{ticket.Id}: Escalando al Nivel 2...");
            base.Manejar(ticket);
        }
    }
}

public class SoporteNivel2 : ManejadorTicket
{
    public override void Manejar(Ticket ticket)
    {
        if (ticket.Prioridad <= PrioridadTicket.Media)
        {
            Console.WriteLine($"    ✅ [Nivel 2] Ticket #{ticket.Id}: '{ticket.Asunto}' resuelto (técnico)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Nivel 2] Ticket #{ticket.Id}: Escalando al Gerente...");
            base.Manejar(ticket);
        }
    }
}

public class GerenteTienda : ManejadorTicket
{
    public override void Manejar(Ticket ticket)
    {
        if (ticket.Prioridad <= PrioridadTicket.Alta)
        {
            Console.WriteLine($"    ✅ [Gerente] Ticket #{ticket.Id}: '{ticket.Asunto}' resuelto (decisión gerencial)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Gerente] Ticket #{ticket.Id}: Escalando al Director...");
            base.Manejar(ticket);
        }
    }
}

public class DirectorGeneral : ManejadorTicket
{
    public override void Manejar(Ticket ticket) =>
        Console.WriteLine($"    ✅ [Director] Ticket #{ticket.Id}: '{ticket.Asunto}' atendido personalmente por el Director");
}

public static class ChainOfResponsibilityDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔗 CHAIN OF RESPONSIBILITY — Cadena de manejadores\n");
        Console.WriteLine("  Escenario: Los tickets de soporte escalan según su prioridad\n");

        // Construir la cadena
        var nivel1 = new SoporteNivel1();
        var nivel2 = new SoporteNivel2();
        var gerente = new GerenteTienda();
        var director = new DirectorGeneral();

        nivel1.EstablecerSiguiente(nivel2);
        nivel2.EstablecerSiguiente(gerente);
        gerente.EstablecerSiguiente(director);

        // Tickets de prueba
        var tickets = new Ticket[]
        {
            new(101, "No puedo iniciar sesión", PrioridadTicket.Baja),
            new(102, "Error al cargar el carrito", PrioridadTicket.Media),
            new(103, "Página caída en producción", PrioridadTicket.Alta),
            new(104, "¡Fuga de datos críticos!", PrioridadTicket.Urgente),
        };

        foreach (var ticket in tickets)
        {
            Console.WriteLine($"  ── Ticket #{ticket.Id} ({ticket.Prioridad}): {ticket.Asunto} ──");
            nivel1.Manejar(ticket);
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Cada nivel decide si puede resolverlo o pasa al siguiente.");
        Console.WriteLine("     El cliente no sabe QUIÉN lo resolverá.");
        Console.WriteLine("     Se pueden reordenar, agregar o quitar niveles sin tocar al cliente.");
    }
}
