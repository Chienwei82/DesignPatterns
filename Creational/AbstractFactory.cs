namespace DesignPatterns.Creational;

/// ============================================================================
/// PATRÓN ABSTRACT FACTORY — Familias de objetos compatibles
/// ============================================================================
/// Intención: crear FAMILIAS de objetos relacionados sin acoplarse a sus
/// clases concretas.
///
/// Analogía: tienda de muebles con líneas "moderna" o "vintage": compras el
/// set y todo combina. En este proyecto: un tema de UI (claro/oscuro) donde
/// botón, ventana y diálogo siempre combinan.
///
/// Cuándo usarlo: familias de objetos que deben usarse juntos (temas de UI,
/// conectores a distintas bases de datos, kits multiplataforma).
///
/// Cuándo NO usarlo: si solo hay un producto por familia (para eso es
/// Factory Method, más simple).
///
/// USO REAL: Interfaces UI multiplataforma, conectores a distintos motores
/// de BD, temas visuales completos.
/// ============================================================================

// --- Productos abstractos ---
public interface IBoton { void Dibujar(); }
public interface IVentana { void Dibujar(); }
public interface IDialogo { void Dibujar(); }

// --- Familia: Tema Claro ---
public class BotonClaro : IBoton
{
    public void Dibujar() => Console.WriteLine("  [☀️ Botón claro] fondo blanco, texto oscuro");
}

public class VentanaClara : IVentana
{
    public void Dibujar() => Console.WriteLine("  [☀️ Ventana clara] borde gris suave, sombra ligera");
}

public class DialogoClaro : IDialogo
{
    public void Dibujar() => Console.WriteLine("  [☀️ Diálogo claro] fondo blanco, borde fino");
}

// --- Familia: Tema Oscuro ---
public class BotonOscuro : IBoton
{
    public void Dibujar() => Console.WriteLine("  [🌙 Botón oscuro] fondo negro, texto blanco");
}

public class VentanaOscura : IVentana
{
    public void Dibujar() => Console.WriteLine("  [🌙 Ventana oscura] borde gris oscuro, sin sombra");
}

public class DialogoOscuro : IDialogo
{
    public void Dibujar() => Console.WriteLine("  [🌙 Diálogo oscuro] fondo gris carbón, borde luminoso");
}

// --- Abstract Factory ---
public interface ITemaFactory
{
    IBoton CrearBoton();
    IVentana CrearVentana();
    IDialogo CrearDialogo();
}

// --- Factories concretas ---
public class TemaClaroFactory : ITemaFactory
{
    public IBoton CrearBoton() => new BotonClaro();
    public IVentana CrearVentana() => new VentanaClara();
    public IDialogo CrearDialogo() => new DialogoClaro();
}

public class TemaOscuroFactory : ITemaFactory
{
    public IBoton CrearBoton() => new BotonOscuro();
    public IVentana CrearVentana() => new VentanaOscura();
    public IDialogo CrearDialogo() => new DialogoOscuro();
}

public static class AbstractFactoryDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏭🏭 ABSTRACT FACTORY — Familias de componentes que combinan\n");
        Console.WriteLine("  Escenario: La app arma pantallas completas por tema visual\n");

        var temas = new (string nombre, ITemaFactory factory)[]
        {
            ("Claro", new TemaClaroFactory()),
            ("Oscuro", new TemaOscuroFactory())
        };

        foreach (var (nombre, factory) in temas)
        {
            Console.WriteLine($"  ── TEMA {nombre.ToUpper()} ──");

            // El cliente usa la factory sin saber qué clases concretas se crean
            var boton = factory.CrearBoton();
            var ventana = factory.CrearVentana();
            var dialogo = factory.CrearDialogo();

            boton.Dibujar();
            ventana.Dibujar();
            dialogo.Dibujar();
            Console.WriteLine();
        }

        Console.WriteLine("  ✅ Los componentes de cada familia son compatibles entre sí.");
        Console.WriteLine("  ✅ Cambiar de tema = cambiar de factory. El cliente no cambia.");
    }
}
