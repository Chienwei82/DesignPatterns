namespace DesignPatterns.Behavioral;

/// PATRÓN TEMPLATE METHOD
/// ──────────────────────
/// Define el ESQUELETO de un algoritmo en un método, dejando que
/// las subclases implementen ciertos pasos SIN cambiar la estructura
/// del algoritmo. "Invertir el control" — Hollywood Principle:
/// "No nos llames, nosotros te llamamos".
///
/// USO REAL: Pipelines de procesamiento de datos, generación de
///           reportes (PDF/Excel/HTML), frameworks de testing,
///           procesos ETL, recetas de cocina.

// --- Clase abstracta con Template Method ---
public abstract class RecetaBase
{
    /// Template Method — define la estructura del algoritmo.
    /// No es virtual ni abstract, por lo que las subclases NO pueden
    /// sobrescribirlo. Esto garantiza que el flujo siempre sea el mismo.
    public void Preparar(string nombrePlato)
    {
        Console.WriteLine("\n  ⚙️  Iniciando preparación...\n");

        // Paso 1: Validar (hook opcional)
        if (!ValidarPedido(nombrePlato))
        {
            Console.WriteLine("  ❌ Pedido inválido. Abortando.");
            return;
        }

        // Paso 2: Reunir ingredientes (varía según la receta)
        var ingredientes = ReunirIngredientes();

        // Paso 3: Cocinar (varía según la receta)
        Cocinar(ingredientes);

        // Paso 4: Emplatar (varía según la receta)
        Emplatar(ingredientes);

        // Paso 5: Notificar (hook opcional)
        NotificarListo(nombrePlato);

        Console.WriteLine("  ✅ Preparación completada.\n");
    }

    // Pasos abstractos — las subclases DEBEN implementarlos
    protected abstract string ReunirIngredientes();
    protected abstract void Cocinar(string ingredientes);
    protected abstract void Emplatar(string ingredientes);

    // Paso con implementación por defecto (común a todos)
    protected virtual bool ValidarPedido(string nombrePlato)
    {
        if (string.IsNullOrEmpty(nombrePlato))
        {
            Console.WriteLine("  [Validación] No hay nombre de platillo");
            return false;
        }
        Console.WriteLine($"  [Validación] Pedido válido: {nombrePlato}");
        return true;
    }

    // Hook — las subclases PUEDEN sobrescribirlo, pero no es obligatorio
    protected virtual void NotificarListo(string nombrePlato)
    {
        Console.WriteLine($"  [Notificación] '{nombrePlato}' listo para servir.");
    }
}

// --- Subclase concreta: Pizza ---
public class RecetaPizza : RecetaBase
{
    protected override string ReunirIngredientes()
    {
        Console.WriteLine("  [Pizza] Reuniendo masa, tomate y queso");
        return "masa, tomate, queso";
    }

    protected override void Cocinar(string ingredientes)
    {
        Console.WriteLine($"  [Pizza] Horneando {ingredientes} a 250°C por 12 min");
    }

    protected override void Emplatar(string ingredientes)
    {
        Console.WriteLine("  [Pizza] Cortando en 8 porciones y sirviendo en tabla");
    }
}

// --- Subclase concreta: Sushi ---
public class RecetaSushi : RecetaBase
{
    protected override string ReunirIngredientes()
    {
        Console.WriteLine("  [Sushi] Reuniendo arroz, salmón y algas");
        return "arroz, salmón, algas";
    }

    protected override void Cocinar(string ingredientes)
    {
        Console.WriteLine($"  [Sushi] Sin cocción: armando rollos con {ingredientes}");
    }

    protected override void Emplatar(string ingredientes)
    {
        Console.WriteLine("  [Sushi] Sirviendo en plato frío con wasabi y jengibre");
    }

    // Hook sobrescrito: el sushi tiene su propia notificación
    protected override void NotificarListo(string nombrePlato)
    {
        Console.WriteLine($"  [Sushi] 🍣 '{nombrePlato}' listo. ¡Consumir de inmediato!");
    }
}

// --- Subclase concreta: Pasta ---
public class RecetaPasta : RecetaBase
{
    protected override string ReunirIngredientes()
    {
        Console.WriteLine("  [Pasta] Reuniendo pasta y salsa alfredo");
        return "pasta, salsa alfredo";
    }

    protected override void Cocinar(string ingredientes)
    {
        Console.WriteLine($"  [Pasta] Hirviendo {ingredientes} por 8 min");
    }

    protected override void Emplatar(string ingredientes)
    {
        Console.WriteLine("  [Pasta] Sirviendo en plato hondo con parmesano");
    }
}

public static class TemplateMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  📋 TEMPLATE METHOD — Esqueleto de algoritmo\n");
        Console.WriteLine("  Escenario: Preparación de recetas (Pizza/Sushi/Pasta)\n");
        Console.WriteLine("  El flujo es siempre el mismo, pero cada paso varía");

        // Preparar usando el template method
        var recetas = new (string nombre, RecetaBase receta)[]
        {
            ("Pizza margarita", new RecetaPizza()),
            ("Sushi de salmón", new RecetaSushi()),
            ("Pasta alfredo", new RecetaPasta()),
        };

        foreach (var (nombre, receta) in recetas)
        {
            Console.WriteLine($"\n  ══ Preparando: {nombre} ══");
            receta.Preparar(nombre);
        }

        Console.WriteLine("  ✅ El Template Method define el QUÉ y el ORDEN.");
        Console.WriteLine("     Las subclases definen el CÓMO de cada paso.");
        Console.WriteLine("     El algoritmo NUNCA cambia su estructura.");
    }
}
