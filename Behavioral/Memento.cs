namespace DesignPatterns.Behavioral;

/// PATRÓN MEMENTO
/// ─────────────
/// Permite capturar y externalizar el estado interno de un objeto
/// SIN violar su encapsulamiento, de modo que el objeto pueda ser
/// restaurado a ese estado más tarde.
///
/// USO REAL: Guardado de juegos (checkpoints), undo en editores de
///           gráficos, snapshots de máquinas virtuales, historial
///           de configuraciones.

// --- Memento: almacena el estado (inmutable desde fuera) ---
public record EstadoPizza(string Masa, string Salsa, List<string> Ingredientes, bool Horneada, DateTimeOffset GuardadoEn);

// --- Originator: el objeto cuyo estado se guarda ---
public class PizzaEnProgreso
{
    public string Masa { get; set; } = "masa fina";
    public string Salsa { get; set; } = "salsa de tomate";
    public List<string> Ingredientes { get; set; } = [];
    public bool Horneada { get; set; }

    public EstadoPizza Guardar()
    {
        Console.WriteLine("  💾 Guardando el avance de la pizza...");
        // Copiamos la lista para que el memento sea una foto real
        return new EstadoPizza(Masa, Salsa, [.. Ingredientes], Horneada, DateTimeOffset.UtcNow);
    }

    public void Restaurar(EstadoPizza estado)
    {
        Console.WriteLine($"  ⏪ Restaurando pizza desde {estado.GuardadoEn:HH:mm:ss}Z...");
        Masa = estado.Masa;
        Salsa = estado.Salsa;
        Ingredientes = [.. estado.Ingredientes];
        Horneada = estado.Horneada;
    }

    public void Mostrar()
    {
        var ing = Ingredientes.Count > 0 ? string.Join(", ", Ingredientes) : "(ninguno)";
        var estado = Horneada ? "horneada" : "cruda";
        Console.WriteLine($"    🍕 {Masa} + {Salsa} + [{ing}] ({estado})");
    }

    public void AgregarIngrediente(string ingrediente)
    {
        Ingredientes.Add(ingrediente);
        Console.WriteLine($"    ➕ Agregado: {ingrediente}");
    }

    public void Hornear()
    {
        Horneada = true;
        Console.WriteLine("    🔥 ¡Pizza al horno!");
    }

    public void Arruinar()
    {
        Ingredientes.Clear();
        Horneada = true;
        Console.WriteLine("    💥 ¡Se cayó la pizza al piso y se quemó!");
    }
}

// --- Caretaker: administra los mementos (historial de guardados) ---
public class GestorRecetas
{
    private readonly Stack<EstadoPizza> _checkpoints = new();

    public void Guardar(EstadoPizza estado)
    {
        _checkpoints.Push(estado);
        Console.WriteLine($"    📚 Checkpoints guardados: {_checkpoints.Count}");
    }

    public EstadoPizza? Deshacer()
    {
        if (_checkpoints.Count == 0)
        {
            Console.WriteLine("    ⚠️  No hay checkpoints guardados");
            return null;
        }
        return _checkpoints.Pop();
    }
}

public static class MementoDemo
{
    public static void Run()
    {
        Console.WriteLine("  💾 MEMENTO — Guardar y restaurar estado sin romper encapsulamiento\n");
        Console.WriteLine("  Escenario: Guardas la pizza a medio armar por si la arruinas\n");

        var pizza = new PizzaEnProgreso();
        var gestor = new GestorRecetas();

        // Inicio
        Console.WriteLine("  ── Empezamos la pizza ──");
        pizza.Mostrar();
        gestor.Guardar(pizza.Guardar());
        Console.WriteLine();

        // Agregamos ingredientes
        Console.WriteLine("  ── Agregando ingredientes ──");
        pizza.AgregarIngrediente("queso");
        pizza.AgregarIngrediente("pepperoni");
        pizza.Mostrar();
        gestor.Guardar(pizza.Guardar());
        Console.WriteLine();

        // Se arruina
        Console.WriteLine("  ── ¡Desastre en la cocina! ──");
        pizza.Arruinar();
        pizza.Mostrar();
        Console.WriteLine();

        // Restaurar al último checkpoint
        Console.WriteLine("  ── Restaurando el último checkpoint ──");
        var checkpoint = gestor.Deshacer();
        if (checkpoint is not null)
        {
            pizza.Restaurar(checkpoint);
            pizza.Mostrar();
        }
        Console.WriteLine();

        // Restaurar al inicio
        Console.WriteLine("  ── Restaurando al inicio ──");
        var inicio = gestor.Deshacer();
        if (inicio is not null)
        {
            pizza.Restaurar(inicio);
            pizza.Mostrar();
        }
        Console.WriteLine();

        Console.WriteLine("  ✅ El memento encapsula el estado sin exponer la Pizza.");
        Console.WriteLine("     El Gestor (Caretaker) nunca modifica el estado directamente.");
    }
}
