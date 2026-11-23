namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN CHAIN OF RESPONSIBILITY — Cadena de manejadores
/// ============================================================================
/// Intención: pasar solicitudes a lo largo de una cadena de manejadores.
/// Cada manejador decide si procesa la solicitud o la pasa al siguiente
/// eslabón. Desacopla al emisor del receptor.
///
/// Analogía: la escalada de una amenaza entre superhéroes (Liga de la Justicia):
/// el guardián local intenta manejarla; si no puede, escala a un superhéroe,
/// luego al equipo y por último a la Liga completa.
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
public abstract class ManejadorAmenaza
{
    protected ManejadorAmenaza? _siguiente;

    public void EstablecerSiguiente(ManejadorAmenaza siguiente) => _siguiente = siguiente;

    public virtual void Manejar(Amenaza amenaza)
    {
        if (_siguiente != null)
            _siguiente.Manejar(amenaza);
        else
            Console.WriteLine($"    ❌ Amenaza #{amenaza.Id}: ¡NADIE pudo detener '{amenaza.Asunto}'!");
    }
}

public enum NivelAmenaza
{
    Menor = 1,
    Moderada,
    Alta,
    Apocaliptica
}

public record Amenaza(int Id, string Asunto, NivelAmenaza Nivel);

// --- Handlers concretos ---
public class GuardianCiudad : ManejadorAmenaza
{
    public override void Manejar(Amenaza amenaza)
    {
        if (amenaza.Nivel <= NivelAmenaza.Menor)
        {
            Console.WriteLine($"    ✅ [Guardián] Amenaza #{amenaza.Id}: '{amenaza.Asunto}' neutralizada (un carterista)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Guardián] Amenaza #{amenaza.Id}: Escalando a un superhéroe...");
            base.Manejar(amenaza);
        }
    }
}

public class Superheroe : ManejadorAmenaza
{
    public override void Manejar(Amenaza amenaza)
    {
        if (amenaza.Nivel <= NivelAmenaza.Moderada)
        {
            Console.WriteLine($"    ✅ [Superhéroe] Amenaza #{amenaza.Id}: '{amenaza.Asunto}' resuelta (un villano menor)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Superhéroe] Amenaza #{amenaza.Id}: Escalando al equipo...");
            base.Manejar(amenaza);
        }
    }
}

public class Vengadores : ManejadorAmenaza
{
    public override void Manejar(Amenaza amenaza)
    {
        if (amenaza.Nivel <= NivelAmenaza.Alta)
        {
            Console.WriteLine($"    ✅ [Vengadores] Amenaza #{amenaza.Id}: '{amenaza.Asunto}' contenida (¡Vengadores, reúnanse!)");
        }
        else
        {
            Console.WriteLine($"    ⏩ [Vengadores] Amenaza #{amenaza.Id}: ¡Es Apocalíptica! Escalando a la Liga...");
            base.Manejar(amenaza);
        }
    }
}

public class LigaDeLaJusticia : ManejadorAmenaza
{
    public override void Manejar(Amenaza amenaza) =>
        Console.WriteLine($"    ✅ [Liga de la Justicia] Amenaza #{amenaza.Id}: '{amenaza.Asunto}' detenida por Superman y Wonder Woman");
}

public static class ChainOfResponsibilityDemo
{
    public static void Run()
    {
        Console.WriteLine("  🔗 CHAIN OF RESPONSIBILITY — Cadena de manejadores (escalada de amenazas)\n");
        Console.WriteLine("  Escenario: Las amenazas escalan por la cadena de héroes según su gravedad\n");

        // Construir la cadena
        var guardian = new GuardianCiudad();
        var heroe = new Superheroe();
        var vengadores = new Vengadores();
        var liga = new LigaDeLaJusticia();

        guardian.EstablecerSiguiente(heroe);
        heroe.EstablecerSiguiente(vengadores);
        vengadores.EstablecerSiguiente(liga);

        // Amenazas de prueba
        var amenazas = new Amenaza[]
        {
            new(101, "Un carterista en el metro", NivelAmenaza.Menor),
            new(102, "El Pingüino roba un banco", NivelAmenaza.Moderada),
            new(103, "Loki invade Nueva York", NivelAmenaza.Alta),
            new(104, "Darkseid ataca la Tierra", NivelAmenaza.Apocaliptica),
        };

        foreach (var amenaza in amenazas)
        {
            Console.WriteLine($"  ── Amenaza #{amenaza.Id} ({amenaza.Nivel}): {amenaza.Asunto} ──");
            guardian.Manejar(amenaza);
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Cada nivel decide si puede manejarla o la pasa al siguiente.");
        Console.WriteLine("     El emisor no sabe QUIÉN la resolverá.");
        Console.WriteLine("     Se pueden reordenar, agregar o quitar niveles sin tocar al emisor.");
    }
}
