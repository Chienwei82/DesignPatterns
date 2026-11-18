namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN BUILDER — Construcción paso a paso
/// ============================================================================
/// Intención: separar la construcción de un objeto complejo de su
/// representación, para que el mismo proceso pueda crear diferentes
/// variantes. Ideal para objetos con muchos parámetros opcionales.
///
/// Analogía: armar una hamburguesa por pasos (pan, carne, extras).
///
/// Cuándo usarlo: objetos con muchos parámetros opcionales (consultas SQL,
/// HTTP requests, pedidos).
///
/// Cuándo NO usarlo: con 2-3 parámetros simples; el patrón agrega más código
/// del que resuelve.
///
/// USO REAL: Construcción de consultas SQL, objetos HTTP Request, armado de
/// pizzas/hamburguesas, objetos con muchos campos.
/// ============================================================================

// --- Producto final ---
public class Hamburguesa
{
    public string Pan { get; set; } = "";
    public string Carne { get; set; } = "";
    public bool ConQueso { get; set; }
    public bool SinCebolla { get; set; }
    public string Salsa { get; set; } = "ninguna";
    public List<string> Extras { get; set; } = [];

    public void Resumen()
    {
        Console.WriteLine($"  Pan:        {Pan}");
        Console.WriteLine($"  Carne:      {Carne}");
        Console.WriteLine($"  Queso:      {(ConQueso ? "Sí" : "No")}");
        Console.WriteLine($"  Cebolla:    {(SinCebolla ? "Sin cebolla" : "Normal")}");
        Console.WriteLine($"  Salsa:      {Salsa}");
        Console.WriteLine($"  Extras:     {(Extras.Count > 0 ? string.Join(", ", Extras) : "(ninguno)")}");
    }
}

// --- Builder ---
public interface IHamburguesaBuilder
{
    IHamburguesaBuilder ConPan(string tipo);
    IHamburguesaBuilder ConCarne(string tipo);
    IHamburguesaBuilder ConQueso();
    IHamburguesaBuilder SinCebolla();
    IHamburguesaBuilder ConSalsa(string salsa);
    IHamburguesaBuilder AgregarExtra(string extra);
    Hamburguesa Construir();
}

// --- Builder concreto ---
public class HamburguesaBuilder : IHamburguesaBuilder
{
    private Hamburguesa _hamburguesa = new();

    public IHamburguesaBuilder ConPan(string tipo)
    {
        _hamburguesa.Pan = tipo;
        Console.WriteLine($"  🍞 Pan: {tipo}");
        return this;
    }

    public IHamburguesaBuilder ConCarne(string tipo)
    {
        _hamburguesa.Carne = tipo;
        Console.WriteLine($"  🥩 Carne: {tipo}");
        return this;
    }

    public IHamburguesaBuilder ConQueso()
    {
        _hamburguesa.ConQueso = true;
        Console.WriteLine("  🧀 Queso agregado");
        return this;
    }

    public IHamburguesaBuilder SinCebolla()
    {
        _hamburguesa.SinCebolla = true;
        Console.WriteLine("  🧅 Sin cebolla");
        return this;
    }

    public IHamburguesaBuilder ConSalsa(string salsa)
    {
        _hamburguesa.Salsa = salsa;
        Console.WriteLine($"  🥫 Salsa: {salsa}");
        return this;
    }

    public IHamburguesaBuilder AgregarExtra(string extra)
    {
        _hamburguesa.Extras.Add(extra);
        Console.WriteLine($"  ➕ Extra: {extra}");
        return this;
    }

    public Hamburguesa Construir()
    {
        Console.WriteLine("  🏁 Hamburguesa armada");
        var resultado = _hamburguesa;
        _hamburguesa = new Hamburguesa(); // Reset para permitir reutilización
        return resultado;
    }
}

// --- Director (opcional): encapsula recetas predefinidas ---
public class ChefDirector
{
    private readonly IHamburguesaBuilder _builder;

    public ChefDirector(IHamburguesaBuilder builder) => _builder = builder;

    public Hamburguesa PrepararClasica()
    {
        _builder.ConPan("brioche");
        _builder.ConCarne("res");
        _builder.ConQueso();
        _builder.ConSalsa("tomate");
        return _builder.Construir();
    }

    public Hamburguesa PrepararMonstruosa()
    {
        _builder.ConPan("pretzel");
        _builder.ConCarne("doble res y tocino");
        _builder.ConQueso();
        _builder.ConSalsa("barbacoa");
        _builder.AgregarExtra("aros de cebolla");
        _builder.AgregarExtra("jalapeños");
        _builder.AgregarExtra("huevo frito");
        return _builder.Construir();
    }
}

public static class BuilderDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧱 BUILDER — Armar una hamburguesa paso a paso\n");
        Console.WriteLine("  Escenario: El chef arma hamburguesas con distintas configuraciones\n");

        // ── A medida: API fluida (fluent API) ──
        Console.WriteLine("  ── Hamburguesa a medida (fluida) ──");
        var aMedida = new HamburguesaBuilder()
            .ConPan("brioche")
            .ConCarne("pollo crujiente")
            .ConQueso()
            .SinCebolla()
            .ConSalsa("mostaza y miel")
            .AgregarExtra("lechuga")
            .Construir();
        Console.WriteLine();
        aMedida.Resumen();

        Console.WriteLine();
        Console.WriteLine("  ── La Clásica (vía Director) ──");
        var chef = new ChefDirector(new HamburguesaBuilder());
        chef.PrepararClasica().Resumen();

        Console.WriteLine();
        Console.WriteLine("  ── La Monstruosa (vía Director) ──");
        chef.PrepararMonstruosa().Resumen();

        Console.WriteLine();
        Console.WriteLine("  ✅ El Builder permite crear objetos con diferentes");
        Console.WriteLine("     configuraciones sin constructores gigantes.");
        Console.WriteLine("     El Director reutiliza el mismo builder con recetas distintas.");
    }
}
