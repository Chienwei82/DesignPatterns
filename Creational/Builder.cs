namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN BUILDER — Construcción paso a paso
/// ============================================================================
/// Intención: separar la construcción de un objeto complejo de su
/// representación, para que el mismo proceso pueda crear diferentes
/// variantes. Ideal para objetos con muchos parámetros opcionales.
///
/// Analogía: armar un sable de luz de Star Wars pieza por pieza (empuñadura,
/// cristal kyber, color de hoja, extras): el mismo proceso construye el sable
/// de un Jedi o el de un Sith.
///
/// Cuándo usarlo: objetos con muchos parámetros opcionales (consultas SQL,
/// HTTP requests, pedidos).
///
/// Cuándo NO usarlo: con 2-3 parámetros simples; el patrón agrega más código
/// del que resuelve.
///
/// USO REAL: Construcción de consultas SQL (SELECT/FROM/WHERE), objetos HTTP
/// Request, armado de reportes, objetos con muchos campos.
/// ============================================================================

// --- Producto final ---
public class SableDeLuz
{
    public string Empunadura { get; set; } = "";
    public string Cristal { get; set; } = "";
    public string Color { get; set; } = "";
    public bool DobleFilo { get; set; }
    public bool Electrico { get; set; }

    public void Descripcion()
    {
        Console.WriteLine($"  🗡️  Empuñadura: {Empunadura}");
        Console.WriteLine($"  💎 Cristal kyber: {Cristal}");
        Console.WriteLine($"  🌈 Hoja: {Color}{(DobleFilo ? " (doble filo)" : "")}{(Electrico ? " ⚡ (hoja eléctrica)" : "")}");
    }
}

// --- Builder ---
public interface ISableBuilder
{
    ISableBuilder ConEmpunadura(string material);
    ISableBuilder ConCristal(string cristal);
    ISableBuilder ConColor(string color);
    ISableBuilder DobleFilo();
    ISableBuilder ConDescargaElectrica();
    SableDeLuz Build();
}

// --- Builder concreto ---
public class SableBuilder : ISableBuilder
{
    private SableDeLuz _sable = new();

    public ISableBuilder ConEmpunadura(string material)
    {
        _sable.Empunadura = material;
        Console.WriteLine($"  🔧 Empuñadura: {material}");
        return this;
    }

    public ISableBuilder ConCristal(string cristal)
    {
        _sable.Cristal = cristal;
        Console.WriteLine($"  💎 Cristal kyber: {cristal}");
        return this;
    }

    public ISableBuilder ConColor(string color)
    {
        _sable.Color = color;
        Console.WriteLine($"  🌈 Color de hoja: {color}");
        return this;
    }

    public ISableBuilder DobleFilo()
    {
        _sable.DobleFilo = true;
        Console.WriteLine("  🌀 Montando hoja de doble filo");
        return this;
    }

    public ISableBuilder ConDescargaElectrica()
    {
        _sable.Electrico = true;
        Console.WriteLine("  ⚡ Añadiendo módulo de carga eléctrica");
        return this;
    }

    public SableDeLuz Build()
    {
        Console.WriteLine("  🏁 Sable ensamblado");
        var resultado = _sable;
        _sable = new SableDeLuz(); // Reset para permitir reutilización
        return resultado;
    }
}

// --- Director (opcional): encapsula recetas predefinidas ---
public class ArtificeDirector
{
    private readonly ISableBuilder _builder;

    public ArtificeDirector(ISableBuilder builder) => _builder = builder;

    public SableDeLuz SableJedi()
    {
        _builder.ConEmpunadura("aleación ligera")
               .ConCristal("kyber azul")
               .ConColor("azul");
        return _builder.Build();
    }

    public SableDeLuz SableSith()
    {
        _builder.ConEmpunadura("metal negro")
               .ConCristal("kyber rojo sintético")
               .ConColor("rojo")
               .ConDescargaElectrica();
        return _builder.Build();
    }

    public SableDeLuz SableDobleFilo()
    {
        _builder.ConEmpunadura("mango largo")
               .ConCristal("kyber doble")
               .ConColor("rojo")
               .DobleFilo();
        return _builder.Build();
    }
}

public static class BuilderDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧱 BUILDER — Forjar un sable de luz paso a paso (Star Wars)\n");
        Console.WriteLine("  Escenario: Un artífice arma sables con distintas configuraciones\n");

        // ── A medida: API fluida (fluent API) ──
        Console.WriteLine("  ── Sable a medida (fluida) ──");
        var aMedida = new SableBuilder()
            .ConEmpunadura("madera de afromosia")
            .ConCristal("kyber verde")
            .ConColor("verde")
            .Build();
        aMedida.Descripcion();

        Console.WriteLine();
        Console.WriteLine("  ── Sable Jedi (vía Director) ──");
        var director = new ArtificeDirector(new SableBuilder());
        director.SableJedi().Descripcion();

        Console.WriteLine();
        Console.WriteLine("  ── Sable Sith (vía Director) ──");
        director.SableSith().Descripcion();

        Console.WriteLine();
        Console.WriteLine("  ── Sable de doble filo (vía Director) ──");
        director.SableDobleFilo().Descripcion();

        Console.WriteLine();
        Console.WriteLine("  ✅ El Builder crea objetos con distintas configuraciones");
        Console.WriteLine("     sin constructores gigantes. El Director reutiliza el mismo builder.");
    }
}
