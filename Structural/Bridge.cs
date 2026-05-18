namespace DesignPatterns.Structural;

/// PATRÓN BRIDGE
/// ─────────────
/// Separa una ABSTRACCIÓN de su IMPLEMENTACIÓN, permitiendo que ambas
/// evolucionen independientemente. Es útil cuando una clase tiene
/// múltiples dimensiones de variación (ej. forma + color).
///
/// USO REAL: Drivers de base de datos (abstracción: consulta SQL,
///             implementación: driver MySQL/PostgreSQL/SQLite),
///             controles UI multiplataforma, APIs de pago.

// --- Implementación (la parte que varía) ---
public interface IPlataformaTV
{
    string Marca { get; }
    void Encender();
    void Apagar();
    void SintonizarCanal(int canal);
}

public class SamsungTV : IPlataformaTV
{
    public string Marca => "Samsung";

    public void Encender() => Console.WriteLine("    [Samsung] Smart Hub iniciado. Bienvenido.");
    public void Apagar() => Console.WriteLine("    [Samsung] Smart Hub cerrado.");
    public void SintonizarCanal(int canal) => Console.WriteLine($"    [Samsung] Sintonizando canal {canal} via Tizen OS");
}

public class LgTV : IPlataformaTV
{
    public string Marca => "LG";

    public void Encender() => Console.WriteLine("    [LG] webOS iniciado. Hola.");
    public void Apagar() => Console.WriteLine("    [LG] webOS cerrado.");
    public void SintonizarCanal(int canal) => Console.WriteLine($"    [LG] Sintonizando canal {canal} via webOS");
}

public class SonyTV : IPlataformaTV
{
    public string Marca => "Sony";

    public void Encender() => Console.WriteLine("    [Sony] Google TV iniciado.");
    public void Apagar() => Console.WriteLine("    [Sony] Google TV cerrado.");
    public void SintonizarCanal(int canal) => Console.WriteLine($"    [Sony] Sintonizando canal {canal} via Google TV");
}

// --- Abstracción (la parte que el cliente usa) ---
public abstract class ControlRemoto
{
    protected IPlataformaTV _tv;

    protected ControlRemoto(IPlataformaTV tv)
    {
        _tv = tv;
    }

    public abstract void Encender();
    public abstract void Apagar();
    public abstract void CambiarCanal(int canal);
}

public class ControlBasico : ControlRemoto
{
    public ControlBasico(IPlataformaTV tv) : base(tv) { }

    public override void Encender()
    {
        Console.WriteLine($"  [Control Básico] Encendiendo {_tv.Marca}...");
        _tv.Encender();
    }

    public override void Apagar()
    {
        Console.WriteLine($"  [Control Básico] Apagando {_tv.Marca}...");
        _tv.Apagar();
    }

    public override void CambiarCanal(int canal)
    {
        Console.WriteLine($"  [Control Básico] Canal {canal}");
        _tv.SintonizarCanal(canal);
    }
}

public class ControlAvanzado : ControlRemoto
{
    private int _canalAnterior = 1;

    public ControlAvanzado(IPlataformaTV tv) : base(tv) { }

    public override void Encender()
    {
        Console.WriteLine($"  [Control Avanzado] Encendiendo {_tv.Marca} con voz...");
        _tv.Encender();
    }

    public override void Apagar()
    {
        Console.WriteLine($"  [Control Avanzado] Apagando {_tv.Marca} con voz...");
        _tv.Apagar();
    }

    public override void CambiarCanal(int canal)
    {
        _canalAnterior = canal;
        Console.WriteLine($"  [Control Avanzado] Cambiando a canal {canal} (guardado en favoritos)");
        _tv.SintonizarCanal(canal);
    }

    public void VolverCanalAnterior()
    {
        Console.WriteLine($"  [Control Avanzado] Volviendo al canal {_canalAnterior}");
        _tv.SintonizarCanal(_canalAnterior);
    }
}

public static class BridgeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌉 BRIDGE — Abstracción e implementación independientes\n");
        Console.WriteLine("  Escenario: Controles remotos para distintas marcas de TV\n");

        var tvs = new IPlataformaTV[]
        {
            new SamsungTV(),
            new LgTV(),
            new SonyTV()
        };

        foreach (var tv in tvs)
        {
            Console.WriteLine($"  ── TV: {tv.Marca} ──");

            // Mismo control básico, distinta implementación
            var controlBasico = new ControlBasico(tv);
            controlBasico.Encender();
            controlBasico.CambiarCanal(7);
            controlBasico.Apagar();
            Console.WriteLine();
        }

        Console.WriteLine("  ── Control Avanzado en LG ──");
        var controlAvanzado = new ControlAvanzado(new LgTV());
        controlAvanzado.Encender();
        controlAvanzado.CambiarCanal(42);
        controlAvanzado.VolverCanalAnterior();
        controlAvanzado.Apagar();
        Console.WriteLine();

        Console.WriteLine("  ✅ Agregar una nueva marca de TV NO requiere cambiar los controles.");
        Console.WriteLine("     Agregar un nuevo tipo de control NO requiere cambiar las TVs.");
    }
}
