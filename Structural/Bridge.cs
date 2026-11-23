namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN BRIDGE — Abstracción e implementación independientes
/// ============================================================================
/// Intención: separar una abstracción de su implementación, permitiendo que
/// ambas evolucionen independientemente. Útil cuando una clase tiene múltiples
/// dimensiones de variación.
///
/// Analogía: receta (casera/gourmet) × método (horno/freidora/parrilla): dos
/// perillas independientes.
///
/// Cuándo usarlo: cuando tienes dos dimensiones de variación (sin Bridge: 2
/// recetas × 3 métodos = 6 clases; con Bridge: 2 + 3 = 5).
///
/// Cuándo NO usarlo: con una sola dimensión, herencia simple basta.
///
/// USO REAL: Drivers de base de datos (abstracción: consulta SQL,
/// implementación: driver MySQL/PostgreSQL/SQLite), controles UI
/// multiplataforma, APIs de pago.
/// ============================================================================

// --- Implementación (la parte que varía: el renderizador) ---
public interface IRenderizador
{
    string Nombre { get; }
    void Renderizar(string contenido);
}

public class RenderizadorPNG : IRenderizador
{
    public string Nombre => "PNG";
    public void Renderizar(string contenido) =>
        Console.WriteLine($"    [PNG] Generando imagen PNG con: {contenido}");
}

public class RenderizadorSVG : IRenderizador
{
    public string Nombre => "SVG";
    public void Renderizar(string contenido) =>
        Console.WriteLine($"    [SVG] Generando SVG vectorial con: {contenido}");
}

public class RenderizadorConsola : IRenderizador
{
    public string Nombre => "Consola";
    public void Renderizar(string contenido) =>
        Console.WriteLine($"    [Consola] Imprimiendo en terminal: {contenido}");
}

// --- Abstracción (la parte que el cliente usa: el gráfico) ---
public abstract class ChartBase
{
    protected readonly IRenderizador _renderizador;

    protected ChartBase(IRenderizador renderizador) => _renderizador = renderizador;

    public abstract void Mostrar();
}

public class ChartBarras : ChartBase
{
    public ChartBarras(IRenderizador renderizador) : base(renderizador) { }

    public override void Mostrar()
    {
        Console.WriteLine("  [Chart de Barras] Preparando datos...");
        _renderizador.Renderizar("Barras: [A:50, B:75, C:100]");
    }
}

public class ChartLineas : ChartBase
{
    public ChartLineas(IRenderizador renderizador) : base(renderizador) { }

    public override void Mostrar()
    {
        Console.WriteLine("  [Chart de Líneas] Preparando datos...");
        _renderizador.Renderizar("Líneas: [Ene:10, Feb:25, Mar:40]");
    }
}

public static class BridgeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌉 BRIDGE — Abstracción e implementación independientes\n");
        Console.WriteLine("  Escenario: Gráficos × renderizadores (PNG/SVG/Consola)\n");

        var renderizadores = new IRenderizador[] {
            new RenderizadorPNG(),
            new RenderizadorSVG(),
            new RenderizadorConsola()
        };

        foreach (var renderizador in renderizadores)
        {
            Console.WriteLine($"  ── Renderizador: {renderizador.Nombre} ──");
            var barras = new ChartBarras(renderizador);
            barras.Mostrar();
            Console.WriteLine();
        }

        Console.WriteLine("  ── Gráfico de líneas en SVG ──");
        var lineas = new ChartLineas(new RenderizadorSVG());
        lineas.Mostrar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Agregar un renderizador NO requiere cambiar los gráficos.");
        Console.WriteLine("     Agregar un tipo de gráfico NO requiere cambiar los renderizadores.");
    }
}
