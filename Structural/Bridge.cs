namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN BRIDGE — Abstracción e implementación independientes
/// ============================================================================
/// Intención: separar una abstracción de su implementación, permitiendo que
/// ambas evolucionen independientemente. Útil cuando una clase tiene múltiples
/// dimensiones de variación.
///
/// Analogía: en un RPG combinas un arma (espada, arco, bastón) con un elemento
/// mágico (fuego, hielo, rayo). "Arma" y "elemento" son dos perillas
/// independientes: no necesitas una clase por cada combinación.
///
/// Cuándo usarlo: cuando tienes dos dimensiones de variación (sin Bridge: 3
/// armas × 3 elementos = 9 clases; con Bridge: 3 + 3 = 6).
///
/// Cuándo NO usarlo: con una sola dimensión, herencia simple basta.
///
/// USO REAL: Drivers de base de datos (abstracción: consulta SQL,
/// implementación: driver MySQL/PostgreSQL/SQLite), controles UI
/// multiplataforma, APIs de pago.
/// ============================================================================

// --- Implementación (la parte que varía: el elemento mágico) ---
public interface IElementoMagico
{
    string Nombre { get; }
    void Aplicar(string arma);
}

public class Fuego : IElementoMagico
{
    public string Nombre => "Fuego";
    public void Aplicar(string arma) =>
        Console.WriteLine($"    🔥 {arma} envuelta en llamas: daño de quemadura");
}

public class Hielo : IElementoMagico
{
    public string Nombre => "Hielo";
    public void Aplicar(string arma) =>
        Console.WriteLine($"    ❄️ {arma} cubierta de escarcha: congela al enemigo");
}

public class Rayo : IElementoMagico
{
    public string Nombre => "Rayo";
    public void Aplicar(string arma) =>
        Console.WriteLine($"    ⚡ {arma} crepita con electricidad: daño en cadena");
}

// --- Abstracción (la parte que el cliente usa: el arma) ---
public abstract class ArmaElemental
{
    protected readonly IElementoMagico _elemento;
    protected string NombreArma { get; }

    protected ArmaElemental(string nombreArma, IElementoMagico elemento)
    {
        NombreArma = nombreArma;
        _elemento = elemento;
    }

    public abstract void Atacar();
}

public class Arco : ArmaElemental
{
    public Arco(IElementoMagico elemento) : base("Arco del bosque", elemento) { }

    public override void Atacar()
    {
        Console.WriteLine($"  🏹 [{NombreArma}] Preparando flecha...");
        _elemento.Aplicar(NombreArma);
    }
}

public class Baston : ArmaElemental
{
    public Baston(IElementoMagico elemento) : base("Bastón arcano", elemento) { }

    public override void Atacar()
    {
        Console.WriteLine($"  🪄 [{NombreArma}] Concentrando magia...");
        _elemento.Aplicar(NombreArma);
    }
}

public class EspadaElemental : ArmaElemental
{
    public EspadaElemental(IElementoMagico elemento) : base("Espada rúnica", elemento) { }

    public override void Atacar()
    {
        Console.WriteLine($"  ⚔️ [{NombreArma}] Blandiendo el acero...");
        _elemento.Aplicar(NombreArma);
    }
}

public static class BridgeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌉 BRIDGE — Abstracción e implementación independientes (armas × elementos)\n");
        Console.WriteLine("  Escenario: Un RPG combina cada arma con distintos elementos mágicos\n");

        var elementos = new IElementoMagico[] {
            new Fuego(),
            new Hielo(),
            new Rayo()
        };

        foreach (var elemento in elementos)
        {
            Console.WriteLine($"  ── Elemento: {elemento.Nombre} ──");
            var arco = new Arco(elemento);
            arco.Atacar();
            Console.WriteLine();
        }

        Console.WriteLine("  ── Bastón con elemento Rayo ──");
        new Baston(new Rayo()).Atacar();
        Console.WriteLine();

        Console.WriteLine("  ── Espada rúnica con elemento Fuego ──");
        new EspadaElemental(new Fuego()).Atacar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Agregar un elemento NO requiere cambiar las armas.");
        Console.WriteLine("     Agregar un arma NO requiere cambiar los elementos.");
        Console.WriteLine("     Las dos dimensiones evolucionan por separado (3 + 3, no 3 × 3).");
    }
}
