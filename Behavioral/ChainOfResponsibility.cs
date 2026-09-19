namespace DesignPatterns.Behavioral;

/// PATRÓN CHAIN OF RESPONSIBILITY
/// ─────────────────────────────
/// Permite pasar solicitudes a lo largo de una CADENA de manejadores.
/// Cada manejador decide si procesa la solicitud o la pasa al
/// siguiente eslabón. Desacopla el emisor del receptor.
///
/// USO REAL: Middleware en ASP.NET Core, filtros de validación,
///           aprobaciones jerárquicas, soporte técnico escalable,
///           manejo de quejas en un restaurante.

// --- Handler abstracto ---
public abstract class ManejadorQueja
{
    protected ManejadorQueja? _siguiente;

    public void EstablecerSiguiente(ManejadorQueja siguiente)
    {
        _siguiente = siguiente;
    }

    public virtual void Manejar(Queja queja)
    {
        if (_siguiente != null)
        {
            _siguiente.Manejar(queja);
        }
        else
        {
            Console.WriteLine($"    ❌ Queja #{queja.Id}: NADIE pudo atender '{queja.Motivo}'");
        }
    }
}

public enum GravedadQueja
{
    Molestia = 1,
    Reclamo,
    Grave,
    Catastrofe
}

public record Queja(int Id, string Motivo, GravedadQueja Gravedad);

// --- Handlers concretos ---
public class AtencionMesero : ManejadorQueja
{
    public override void Manejar(Queja queja)
    {
        if (queja.Gravedad <= GravedadQueja.Molestia)
        {
            Console.WriteLine($"    ✅ [Mesero] Queja #{queja.Id}: '{queja.Motivo}' resuelta (cambia el plato)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Mesero] Queja #{queja.Id}: Escalando al Jefe de Cocina...");
            base.Manejar(queja);
        }
    }
}

public class JefeDeCocina : ManejadorQueja
{
    public override void Manejar(Queja queja)
    {
        if (queja.Gravedad <= GravedadQueja.Reclamo)
        {
            Console.WriteLine($"    ✅ [Jefe de Cocina] Queja #{queja.Id}: '{queja.Motivo}' resuelta (rehace el platillo)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Jefe de Cocina] Queja #{queja.Id}: Escalando al Gerente...");
            base.Manejar(queja);
        }
    }
}

public class Gerente : ManejadorQueja
{
    public override void Manejar(Queja queja)
    {
        if (queja.Gravedad <= GravedadQueja.Grave)
        {
            Console.WriteLine($"    ✅ [Gerente] Queja #{queja.Id}: '{queja.Motivo}' resuelta (compensa al cliente)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Gerente] Queja #{queja.Id}: Escalando al Dueño...");
            base.Manejar(queja);
        }
    }
}

public class Dueno : ManejadorQueja
{
    public override void Manejar(Queja queja)
    {
        Console.WriteLine($"    ✅ [Dueño] Queja #{queja.Id}: '{queja.Motivo}' atendida personalmente por el Dueño");
    }
}

public static class ChainOfResponsibilityDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔗 CHAIN OF RESPONSIBILITY — Cadena de manejadores\n");
        Console.WriteLine("  Escenario: Las quejas escalan según su gravedad\n");

        // Construir la cadena
        var mesero = new AtencionMesero();
        var jefe = new JefeDeCocina();
        var gerente = new Gerente();
        var dueno = new Dueno();

        mesero.EstablecerSiguiente(jefe);
        jefe.EstablecerSiguiente(gerente);
        gerente.EstablecerSiguiente(dueno);

        // Quejas de prueba
        var quejas = new Queja[]
        {
            new(101, "La mesa está sucia", GravedadQueja.Molestia),
            new(102, "El platillo llegó frío", GravedadQueja.Reclamo),
            new(103, "La comida estaba en mal estado", GravedadQueja.Grave),
            new(104, "¡Se incendió la cocina!", GravedadQueja.Catastrofe),
        };

        foreach (var queja in quejas)
        {
            Console.WriteLine($"  ── Queja #{queja.Id} ({queja.Gravedad}): {queja.Motivo} ──");
            mesero.Manejar(queja);
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Cada nivel decide si puede resolverla o pasa al siguiente.");
        Console.WriteLine("     El cliente no sabe QUIÉN la resolverá.");
        Console.WriteLine("     Se pueden reordenar, agregar o quitar niveles sin tocar al cliente.");
    }
}
