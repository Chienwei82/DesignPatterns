namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN TEMPLATE METHOD — Esqueleto de algoritmo
/// ============================================================================
/// Intención: definir el esqueleto de un algoritmo en un método, dejando que
/// las subclases implementen ciertos pasos sin cambiar la estructura del
/// algoritmo. "Hollywood Principle": "No nos llames, nosotros te llamamos".
///
/// Analogía: el "Viaje del Héroe" (monomito de Joseph Campbell) que siguen
/// Luke Skywalker, Frodo o Simba: los pasos son fijos (llamado → pruebas →
/// retorno), pero cada héroe los vive a su manera.
///
/// Cuándo usarlo: pipelines con pasos comunes que varían en el detalle
/// (procesar CSV/JSON, generar reportes).
///
/// Cuándo NO usarlo: si cada implementación difiere mucho en el flujo, mejor
/// composición (Strategy).
///
/// USO REAL: Pipelines de procesamiento de datos, generación de reportes
/// (PDF/Excel/HTML), frameworks de testing, procesos ETL.
/// ============================================================================

// --- Clase abstracta con Template Method ---
public abstract class ViajeDelHeroe
{
    public string NombreHeroe { get; }

    protected ViajeDelHeroe(string nombreHeroe) => NombreHeroe = nombreHeroe;

    // Template Method: define la estructura del algoritmo. Las subclases NO
    // pueden sobrescribirlo (no es virtual).
    public void VivirAventura()
    {
        Console.WriteLine($"\n  ⚙️  Comienza el viaje de {NombreHeroe}...\n");

        // Paso 1: Llamado a la aventura (hook opcional)
        if (!RecibirLlamado())
        {
            Console.WriteLine("  ❌ El héroe ignora el llamado. No hay aventura.");
            return;
        }

        // Paso 2: Travesía y pruebas (varía según el héroe)
        var prueba = SuperarPruebas();

        // Paso 3: Transformación (varía según el héroe)
        Transformarse(prueba);

        // Paso 4: Retorno con el don (varía según el héroe)
        Retornar(prueba);

        // Paso 5: Celebración (hook opcional)
        CelebrarFinal();

        Console.WriteLine($"  ✅ La aventura de {NombreHeroe} ha concluido.\n");
    }

    // Pasos abstractos — las subclases DEBEN implementarlos
    protected abstract string SuperarPruebas();
    protected abstract void Transformarse(string prueba);
    protected abstract void Retornar(string prueba);

    // Paso con implementación por defecto (común a todos)
    protected virtual bool RecibirLlamado()
    {
        Console.WriteLine($"  [Llamado] {NombreHeroe} recibe una señal de aventura");
        return true;
    }

    // Hook — las subclases PUEDEN sobrescribirlo, pero no es obligatorio
    protected virtual void CelebrarFinal()
    {
        Console.WriteLine($"  [Final] {NombreHeroe} es aclamado como héroe.");
    }
}

// --- Subclase concreta: Luke Skywalker ---
public class LukeSkywalker : ViajeDelHeroe
{
    public LukeSkywalker() : base("Luke Skywalker") { }

    protected override string SuperarPruebas()
    {
        Console.WriteLine("  [Luke] Entrena con Yoda en Dagobah y se enfrenta a Vader");
        return "la Fuerza";
    }

    protected override void Transformarse(string prueba) =>
        Console.WriteLine($"  [Luke] Domina {prueba} y se convierte en Caballero Jedi");

    protected override void Retornar(string prueba) =>
        Console.WriteLine("  [Luke] Regresa para salvar la Rebelión y redime a su padre");
}

// --- Subclase concreta: Frodo Bolsón ---
public class FrodoBolsón : ViajeDelHeroe
{
    public FrodoBolsón() : base("Frodo Bolsón") { }

    protected override string SuperarPruebas()
    {
        Console.WriteLine("  [Frodo] Cruza Mordor cargando el Anillo Único");
        return "el Anillo";
    }

    protected override void Transformarse(string prueba) =>
        Console.WriteLine($"  [Frodo] Resiste la tentación de {prueba} hasta el Monte del Destino");

    protected override void Retornar(string prueba) =>
        Console.WriteLine("  [Frodo] Destruye el Anillo y regresa a la Comarca");

    // Hook sobrescrito: Frodo no celebra como los demás (queda marcado)
    protected override void CelebrarFinal() =>
        Console.WriteLine("  [Final] Frodo parte hacia los Puertos Grises con los Elfos.");
}

public static class TemplateMethodDemo
{
    public static void Run()
    {
        Console.WriteLine("  📋 TEMPLATE METHOD — Esqueleto de algoritmo (El Viaje del Héroe)\n");
        Console.WriteLine("  Escenario: Distintos héroes recorren el mismo monomito paso a paso\n");
        Console.WriteLine("  El flujo es siempre el mismo, pero cada paso varía");

        var heroes = new (string nombre, ViajeDelHeroe heroe)[]
        {
            ("Luke Skywalker", new LukeSkywalker()),
            ("Frodo Bolsón", new FrodoBolsón()),
        };

        foreach (var (nombre, heroe) in heroes)
        {
            Console.WriteLine($"\n  ══ Aventura de: {nombre} ══");
            heroe.VivirAventura();
        }

        Console.WriteLine("  ✅ El Template Method define el QUÉ y el ORDEN.");
        Console.WriteLine("     Las subclases definen el CÓMO de cada paso.");
        Console.WriteLine("     El esqueleto del viaje NUNCA cambia.");
    }
}
