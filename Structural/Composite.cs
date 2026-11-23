namespace DesignPatterns.Structural;

/// ============================================================================
/// PATRÓN COMPOSITE — Estructuras árbol parte-todo
/// ============================================================================
/// Intención: tratar objetos individuales y composiciones de objetos de manera
/// uniforme. Los objetos se organizan en un árbol (parte-todo) y el cliente
/// trata hojas y compuestos de la misma forma.
///
/// Analogía: un sistema de archivos: un archivo y una carpeta completa responden
/// la misma pregunta ("¿cuánto ocupa?").
///
/// Cuándo usarlo: sistemas de archivos, menús, organigramas, árboles de
/// componentes UI.
///
/// Cuándo NO usarlo: si la estructura nunca es anidada (solo hay hojas), el
/// árbol es innecesario.
///
/// USO REAL: Sistemas de archivos (archivos y carpetas), menús (items y
/// submenús), estructuras organizacionales, árboles de componentes UI.
/// ============================================================================

// --- Componente ---
public interface FSNode
{
    string Nombre { get; }
    int GetTamano();
    void Listar(int indentacion = 0);
}

// --- Hoja: un archivo individual ---
public class FileLeaf : FSNode
{
    public string Nombre { get; }
    public int Tamano { get; }

    public FileLeaf(string nombre, int tamano)
    {
        Nombre = nombre;
        Tamano = tamano;
    }

    public int GetTamano() => Tamano;

    public void Listar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}📄 {Nombre} ({Tamano} KB)");
    }
}

// --- Compuesto: un directorio que contiene archivos u otros directorios ---
public class DirectoryComposite : FSNode
{
    public string Nombre { get; }
    private readonly List<FSNode> _hijos = [];

    public DirectoryComposite(string nombre) => Nombre = nombre;

    public void Agregar(FSNode nodo)
    {
        _hijos.Add(nodo);
        Console.WriteLine($"  [Dir {Nombre}] + {nodo.Nombre}");
    }

    public void Remover(FSNode nodo)
    {
        _hijos.Remove(nodo);
        Console.WriteLine($"  [Dir {Nombre}] - {nodo.Nombre}");
    }

    // El compuesto delega a sus hijos y suma resultados (LINQ)
    public int GetTamano() => _hijos.Sum(h => h.GetTamano());

    public void Listar(int indentacion = 0)
    {
        var indent = new string(' ', indentacion * 3);
        Console.WriteLine($"{indent}📁 {Nombre}/ ({GetTamano()} KB)");

        foreach (var hijo in _hijos)
            hijo.Listar(indentacion + 1);
    }
}

public static class CompositeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🌳 COMPOSITE — Estructuras árbol parte-todo\n");
        Console.WriteLine("  Escenario: Sistema de archivos con directorios dentro de directorios\n");

        // ── Hojas: archivos ──
        var readme = new FileLeaf("README.md", 12);
        var config = new FileLeaf("config.json", 4);
        var imagen = new FileLeaf("foto.png", 2048);
        var video = new FileLeaf("video.mp4", 15360);
        var codigo = new FileLeaf("Program.cs", 28);
        var log = new FileLeaf("app.log", 512);

        // ── Directorios (compuestos) ──
        var docs = new DirectoryComposite("Documentos");
        docs.Agregar(readme);
        docs.Agregar(config);

        var media = new DirectoryComposite("Media");
        media.Agregar(imagen);
        media.Agregar(video);

        var proyecto = new DirectoryComposite("Proyecto");
        proyecto.Agregar(docs);
        proyecto.Agregar(media);
        proyecto.Agregar(codigo);
        proyecto.Agregar(log);

        // ── Mostrar estructura completa ──
        Console.WriteLine("  Estructura de archivos:\n");
        proyecto.Listar();
        Console.WriteLine();
        Console.WriteLine($"  💾 Tamaño total: {proyecto.GetTamano():N0} KB");
        Console.WriteLine();

        // El compuesto también permite quitar elementos
        Console.WriteLine("  ── Quitamos el log del proyecto ──");
        proyecto.Remover(log);
        Console.WriteLine();
        proyecto.Listar();
        Console.WriteLine();
        Console.WriteLine($"  💾 Tamaño total ahora: {proyecto.GetTamano():N0} KB");
        Console.WriteLine();

        Console.WriteLine("  ✅ GetTamano() funciona igual para un archivo o un directorio.");
        Console.WriteLine("     El cliente no necesita saber si es hoja o compuesto.");
    }
}
