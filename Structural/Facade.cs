namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN FACADE — Interfaz simple para subsistemas complejos
/// ============================================================================
/// Intención: exponer una interfaz simple que orchestre un subsistema complejo,
/// escondiendo sus detalles internos.
///
/// Analogía: el "Protocolo Vengadores": Nick Fury no ejecuta cada sistema de
/// la nave (armas, defensa, energía, comunicaciones) por separado; da UNA orden
/// y la fachada coordina todos los subsistemas por él.
///
/// Cuándo usarlo: cuando usar un sistema requiere llamar a varios subsistemas
/// en orden.
///
/// Cuándo NO usarlo: si los clientes siempre necesitan el control fino del
/// subsistema; una fachada innecesaria es una capa más de indirección.
///
/// USO REAL: APIs de frameworks (controller de ASP.NET simplifica el pipeline),
/// bibliotecas de logging, sistemas de pedidos.
/// ============================================================================

// --- Subsistema complejo #1: Arsenal ---
public class Arsenal
{
    private readonly Dictionary<string, int> _armas = new()
    {
        ["Repulsores"] = 2,
        ["Escudo de vibranium"] = 1,
        ["Mjolnir"] = 1,
        ["Arco de Ojo de Halcón"] = 3
    };

    public bool VerificarArma(string arma, int cantidad)
    {
        var disponible = _armas.GetValueOrDefault(arma, 0);
        Console.WriteLine($"  [Arsenal] {arma}: pedidas {cantidad}, disponibles {disponible}");
        return disponible >= cantidad;
    }

    public void Asignar(string arma, int cantidad)
    {
        if (_armas.ContainsKey(arma)) _armas[arma] -= cantidad;
        Console.WriteLine($"  [Arsenal] {cantidad}x {arma} asignadas. Quedan: {_armas.GetValueOrDefault(arma, 0)}");
    }
}

// --- Subsistema complejo #2: Repulsores / energía ---
public class Repulsores
{
    public bool Cargar(decimal energia)
    {
        Console.WriteLine($"  [Energía] Cargando repulsores al {energia:N0}%...");
        Console.WriteLine("  [Energía] ⚡ Núcleo ARC estable. Listo para disparar.");
        return true;
    }
}

// --- Subsistema complejo #3: Defensa ---
public class EscudoDefensor
{
    private static int _barrera = 400;

    public string Activar(string objetivo)
    {
        _barrera++;
        var id = $"BARRERA-{_barrera:D4}";
        Console.WriteLine($"  [Defensa] Barrera {id} activada para {objetivo}");
        Console.WriteLine($"  [Defensa] Coordenadas: {objetivo}");
        return id;
    }
}

// --- Subsistema complejo #4: Comunicaciones ---
public class Comunicaciones
{
    public void AvisarAEscenario(string destinatario, string id)
    {
        Console.WriteLine($"  [Comms] 📻 Transmisión cifrada enviada a {destinatario}");
        Console.WriteLine($"  [Comms] 📱 SMS de estado {id} enviado al equipo");
    }
}

// --- FACADE: la interfaz simple que el cliente usa ---
public class CentroDeMando
{
    private readonly Arsenal _arsenal;
    private readonly Repulsores _repulsores;
    private readonly EscudoDefensor _defensa;
    private readonly Comunicaciones _comms;

    public CentroDeMando()
        : this(new Arsenal(), new Repulsores(), new EscudoDefensor(), new Comunicaciones())
    { }

    public CentroDeMando(Arsenal arsenal, Repulsores repulsores,
                         EscudoDefensor defensa, Comunicaciones comms)
    {
        _arsenal = arsenal;
        _repulsores = repulsores;
        _defensa = defensa;
        _comms = comms;
    }

    // Un solo método que orquesta todo. Devuelve la barrera, o null si falla.
    public string? ActivarProtocolo(string objetivo, string arma, int cantidad)
    {
        Console.WriteLine($"\n  🛡️  Protocolo Vengadores: objetivo {objetivo}...");
        Console.WriteLine($"  Arma solicitada: {arma} x{cantidad}");

        if (!_arsenal.VerificarArma(arma, cantidad))
        {
            Console.WriteLine("  ❌ Arma no disponible. Protocolo abortado.");
            return null;
        }

        _arsenal.Asignar(arma, cantidad);

        var energia = cantidad * 25m;
        if (!_repulsores.Cargar(energia))
        {
            Console.WriteLine("  ❌ Carga fallida. Protocolo abortado.");
            return null;
        }

        var id = _defensa.Activar(objetivo);
        _comms.AvisarAEscenario("Vengadores", id);

        return id;
    }
}

public static class FacadeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏛️  FACADE — Interfaz simple para subsistemas complejos\n");
        Console.WriteLine("  Escenario: Nick Fury solo dice 'Protocolo Vengadores' y la fachada");
        Console.WriteLine("  coordina arsenal, energía, defensa y comunicaciones\n");

        var centro = new CentroDeMando();

        var id = centro.ActivarProtocolo(
            objetivo: "Nueva York",
            arma: "Repulsores",
            cantidad: 1
        );

        if (id is not null)
            Console.WriteLine($"\n  🎉 Protocolo activo. Barrera: {id}");

        // ── Protocolo que falla: la fachada orquesta el camino de error ──
        Console.WriteLine("\n  ── Protocolo sin recursos suficientes ──");
        centro.ActivarProtocolo(
            objetivo: "Espacio exterior",
            arma: "Escudo de vibranium",
            cantidad: 5
        );

        Console.WriteLine();
        Console.WriteLine("  ✅ El cliente ve 1 solo método: 'ActivarProtocolo'.");
        Console.WriteLine("     La complejidad de arsenal, energía, defensa y");
        Console.WriteLine("     comunicaciones queda oculta... incluso cuando falla.");
    }
}
