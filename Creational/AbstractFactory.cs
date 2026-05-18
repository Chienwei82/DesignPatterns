namespace DesignPatterns.Creational;

/// PATRÓN ABSTRACT FACTORY
/// ────────────────────────
/// Proporciona una interfaz para crear FAMILIAS de objetos relacionados
/// sin especificar sus clases concretas. Es como un Factory Method
/// pero para crear múltiples productos que funcionan juntos.
///
/// USO REAL: Interfaces UI multiplataforma (Windows/Mac/Linux),
///           conectores a distintos motores de BD, familias de
///           temas visuales (claro/oscuro).

// --- Productos abstractos ---
public interface IBoton
{
    void Dibujar();
}

public interface IVentana
{
    void Mostrar();
}

// --- Productos concretos: Tema Claro ---
public class BotonClaro : IBoton
{
    public void Dibujar()
    {
        Console.WriteLine("  [☀️ Claro] Botón: fondo blanco, texto negro, bordes suaves");
    }
}

public class VentanaClaro : IVentana
{
    public void Mostrar()
    {
        Console.WriteLine("  [☀️ Claro] Ventana: fondo blanco, barras gris claro");
    }
}

// --- Productos concretos: Tema Oscuro ---
public class BotonOscuro : IBoton
{
    public void Dibujar()
    {
        Console.WriteLine("  [🌙 Oscuro] Botón: fondo #333, texto blanco, bordes sutiles");
    }
}

public class VentanaOscuro : IVentana
{
    public void Mostrar()
    {
        Console.WriteLine("  [🌙 Oscuro] Ventana: fondo #1e1e1e, barras #2d2d2d");
    }
}

// --- Productos concretos: Tema Alto Contraste ---
public class BotonAltoContraste : IBoton
{
    public void Dibujar()
    {
        Console.WriteLine("  [♿ Alto Contraste] Botón: amarillo brillante, borde negro grueso");
    }
}

public class VentanaAltoContraste : IVentana
{
    public void Mostrar()
    {
        Console.WriteLine("  [♿ Alto Contraste] Ventana: fondo negro, texto blanco grande");
    }
}

// --- Abstract Factory ---
public interface IUIFactory
{
    IBoton CrearBoton();
    IVentana CrearVentana();
}

// --- Factories concretas ---
public class TemaClaroFactory : IUIFactory
{
    public IBoton CrearBoton() => new BotonClaro();
    public IVentana CrearVentana() => new VentanaClaro();
}

public class TemaOscuroFactory : IUIFactory
{
    public IBoton CrearBoton() => new BotonOscuro();
    public IVentana CrearVentana() => new VentanaOscuro();
}

public class TemaAltoContrasteFactory : IUIFactory
{
    public IBoton CrearBoton() => new BotonAltoContraste();
    public IVentana CrearVentana() => new VentanaAltoContraste();
}

// --- Cliente ---
public static class AbstractFactoryDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭🏭 ABSTRACT FACTORY — Familias de objetos relacionados\n");
        Console.WriteLine("  Escenario: Aplicación que cambia de tema (claro/oscuro/contraste)\n");

        var temas = new (string nombre, IUIFactory factory)[]
        {
            ("Claro", new TemaClaroFactory()),
            ("Oscuro", new TemaOscuroFactory()),
            ("Alto Contraste", new TemaAltoContrasteFactory())
        };

        foreach (var (nombre, factory) in temas)
        {
            Console.WriteLine($"  ── TEMA: {nombre} ──");

            // El cliente usa la factory sin saber qué clases concretas se crean
            var boton = factory.CrearBoton();
            var ventana = factory.CrearVentana();

            boton.Dibujar();
            ventana.Mostrar();
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Los productos de cada familia son compatibles entre sí.");
        Console.WriteLine("  ✅ Cambiar de tema = cambiar de factory. El cliente no cambia.");
    }
}
