namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN BUILDER — Construcción paso a paso
/// ============================================================================
/// Intención: separar la construcción de un objeto complejo de su
/// representación, para que el mismo proceso pueda crear diferentes
/// variantes. Ideal para objetos con muchos parámetros opcionales.
///
/// Analogía: armar una hamburguesa por pasos (pan, carne, extras): el mismo
/// proceso construye variantes distintas.
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
public class ConsultaSQL
{
    public List<string> Columnas { get; set; } = [];
    public string Tabla { get; set; } = "";
    public List<string> Condiciones { get; set; } = [];
    public string Orden { get; set; } = "";
    public int? Limite { get; set; }

    public override string ToString()
    {
        var cols = Columnas.Count > 0 ? string.Join(", ", Columnas) : "*";
        var sql = $"SELECT {cols} FROM {Tabla}";
        if (Condiciones.Count > 0)
            sql += " WHERE " + string.Join(" AND ", Condiciones);
        if (!string.IsNullOrEmpty(Orden))
            sql += $" ORDER BY {Orden}";
        if (Limite.HasValue)
            sql += $" LIMIT {Limite.Value}";
        return sql + ";";
    }
}

// --- Builder ---
public interface IConsultaSQLBuilder
{
    IConsultaSQLBuilder Select(params string[] columnas);
    IConsultaSQLBuilder From(string tabla);
    IConsultaSQLBuilder Where(string condicion);
    IConsultaSQLBuilder OrderBy(string campo, bool descendente = false);
    IConsultaSQLBuilder Limit(int cantidad);
    ConsultaSQL Build();
}

// --- Builder concreto ---
public class ConsultaSQLBuilder : IConsultaSQLBuilder
{
    private ConsultaSQL _consulta = new();

    public IConsultaSQLBuilder Select(params string[] columnas)
    {
        _consulta.Columnas.AddRange(columnas);
        Console.WriteLine($"  📋 SELECT: {string.Join(", ", columnas)}");
        return this;
    }

    public IConsultaSQLBuilder From(string tabla)
    {
        _consulta.Tabla = tabla;
        Console.WriteLine($"  📋 FROM: {tabla}");
        return this;
    }

    public IConsultaSQLBuilder Where(string condicion)
    {
        _consulta.Condiciones.Add(condicion);
        Console.WriteLine($"  📋 WHERE: {condicion}");
        return this;
    }

    public IConsultaSQLBuilder OrderBy(string campo, bool descendente = false)
    {
        _consulta.Orden = $"{campo}{(descendente ? " DESC" : " ASC")}";
        Console.WriteLine($"  📋 ORDER BY: {_consulta.Orden}");
        return this;
    }

    public IConsultaSQLBuilder Limit(int cantidad)
    {
        _consulta.Limite = cantidad;
        Console.WriteLine($"  📋 LIMIT: {cantidad}");
        return this;
    }

    public ConsultaSQL Build()
    {
        Console.WriteLine("  🏁 Consulta SQL construida");
        var resultado = _consulta;
        _consulta = new ConsultaSQL(); // Reset para permitir reutilización
        return resultado;
    }
}

// --- Director (opcional): encapsula consultas predefinidas ---
public class ConsultorDirector
{
    private readonly IConsultaSQLBuilder _builder;

    public ConsultorDirector(IConsultaSQLBuilder builder) => _builder = builder;

    public ConsultaSQL TopUsuarios()
    {
        _builder.Select("id", "nombre", "email")
               .From("usuarios")
               .Where("activo = 1")
               .OrderBy("nombre")
               .Limit(10);
        return _builder.Build();
    }

    public ConsultaSQL VentasRecientes()
    {
        _builder.Select("producto_id", "cantidad", "total")
               .From("ventas")
               .Where("fecha >= '2024-01-01'")
               .OrderBy("fecha", descendente: true)
               .Limit(50);
        return _builder.Build();
    }
}

public static class BuilderDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧱 BUILDER — Construir una consulta SQL paso a paso\n");
        Console.WriteLine("  Escenario: El backend arma consultas SQL con distintas configuraciones\n");

        // ── A medida: API fluida (fluent API) ──
        Console.WriteLine("  ── Consulta a medida (fluida) ──");
        var aMedida = new ConsultaSQLBuilder()
            .Select("id", "nombre")
            .From("productos")
            .Where("precio > 100")
            .OrderBy("precio", descendente: true)
            .Limit(20)
            .Build();
        Console.WriteLine();
        Console.WriteLine($"  {aMedida}");

        Console.WriteLine();
        Console.WriteLine("  ── Top Usuarios (vía Director) ──");
        var director = new ConsultorDirector(new ConsultaSQLBuilder());
        Console.WriteLine($"  {director.TopUsuarios()}");

        Console.WriteLine();
        Console.WriteLine("  ── Ventas Recientes (vía Director) ──");
        Console.WriteLine($"  {director.VentasRecientes()}");

        Console.WriteLine();
        Console.WriteLine("  ✅ El Builder permite crear objetos con diferentes");
        Console.WriteLine("     configuraciones sin constructores gigantes.");
        Console.WriteLine("     El Director reutiliza el mismo builder con consultas distintas.");
    }
}
