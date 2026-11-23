namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN FACTORY METHOD — Creación delegada a subclases
/// ============================================================================
/// Intención: definir una interfaz para crear objetos, pero dejar que las
/// subclases decidan qué clase concreta instanciar.
///
/// Analogía: en Pokémon, cada profesor regional (Oak, Elm, Birch) entrega un
/// Pokémon inicial distinto; el proceso de "entregar inicial" es el mismo, pero
/// cada laboratorio decide cuál crear.
///
/// Cuándo usarlo: no sabes de antemano los tipos exactos que crearás; quieres
/// que otros extiendan tu librería sin modificarla.
///
/// Cuándo NO usarlo: si solo hay un tipo de producto y no habrá variaciones,
/// un simple `new` es más simple.
///
/// USO REAL: Frameworks ORM (DbConnection según la BD), generación de
/// documentos (PDF, Excel, Word), factories de controles UI.
/// ============================================================================

// --- Producto abstracto ---
public interface IInicialPokemon
{
    string Nombre { get; }
    string Tipo { get; }
    void Entregar(string entrenador);
}

// --- Productos concretos ---
public class Bulbasaur : IInicialPokemon
{
    public string Nombre => "Bulbasaur";
    public string Tipo => "Planta/Veneno";
    public void Entregar(string entrenador) =>
        Console.WriteLine($"  🌿 [{Nombre}] tipo {Tipo} entregado a {entrenador}");
}

public class Cyndaquil : IInicialPokemon
{
    public string Nombre => "Cyndaquil";
    public string Tipo => "Fuego";
    public void Entregar(string entrenador) =>
        Console.WriteLine($"  🔥 [{Nombre}] tipo {Tipo} entregado a {entrenador}");
}

public class Mudkip : IInicialPokemon
{
    public string Nombre => "Mudkip";
    public string Tipo => "Agua";
    public void Entregar(string entrenador) =>
        Console.WriteLine($"  💧 [{Nombre}] tipo {Tipo} entregado a {entrenador}");
}

// --- Creator abstracto ---
public abstract class LaboratorioPokemon
{
    public string Region { get; }

    protected LaboratorioPokemon(string region) => Region = region;

    // Factory Method — cada laboratorio decide qué inicial crear
    public abstract IInicialPokemon CrearInicial();

    public void EntregarInicial(string entrenador)
    {
        Console.WriteLine($"  Laboratorio de {Region}: preparando inicial para {entrenador}...");
        var pokemon = CrearInicial();
        pokemon.Entregar(entrenador);
        Console.WriteLine($"     ¡Buena suerte, {entrenador}! Tu aventura empieza con {pokemon.Nombre}.");
    }
}

// --- Creators concretos ---
public class LaboratorioKanto : LaboratorioPokemon
{
    public LaboratorioKanto() : base("Kanto (Prof. Oak)") { }
    public override IInicialPokemon CrearInicial() => new Bulbasaur();
}

public class LaboratorioJohto : LaboratorioPokemon
{
    public LaboratorioJohto() : base("Johto (Prof. Elm)") { }
    public override IInicialPokemon CrearInicial() => new Cyndaquil();
}

public class LaboratorioHoenn : LaboratorioPokemon
{
    public LaboratorioHoenn() : base("Hoenn (Prof. Birch)") { }
    public override IInicialPokemon CrearInicial() => new Mudkip();
}

public static class FactoryMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭 FACTORY METHOD — Cada laboratorio decide qué inicial entregar\n");
        Console.WriteLine("  Escenario: Entrenadores reciben su Pokémon inicial en distintas regiones\n");

        var entregas = new (string entrenador, LaboratorioPokemon lab)[]
        {
            ("Ash", new LaboratorioKanto()),
            ("Ethan", new LaboratorioJohto()),
            ("Brendan", new LaboratorioHoenn())
        };

        foreach (var (entrenador, lab) in entregas)
        {
            lab.EntregarInicial(entrenador);
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ El cliente depende de la abstracción (LaboratorioPokemon),");
        Console.WriteLine("     no de las clases concretas. Para una nueva región,");
        Console.WriteLine("     solo se crea otra subclase de laboratorio.");
    }
}
