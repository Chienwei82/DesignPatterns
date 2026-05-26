namespace DesignPatterns.Creational;

/// PATRÓN BUILDER
/// ──────────────
/// Separa la construcción de un objeto complejo de su representación,
/// permitiendo que el MISMO proceso de construcción cree DIFERENTES
/// representaciones. Ideal cuando un objeto tiene muchos parámetros
/// opcionales o pasos de configuración.
///
/// USO REAL: Construcción de consultas SQL, objetos HTTP Request,
///           constructores de pizzas/hamburguesas, configuración
///           de objetos con muchos parámetros.

// --- Producto final ---
public class PedidoComplejo
{
    public string Cliente { get; set; } = "";
    public List<string> Productos { get; set; } = [];
    public string DireccionEnvio { get; set; } = "";
    public bool EnvolverParaRegalo { get; set; }
    public bool EnvioExprés { get; set; }
    public string? Notas { get; set; }
    public string MetodoPago { get; set; } = "Efectivo";

    public void Resumen()
    {
        Console.WriteLine($"  Cliente:      {Cliente}");
        Console.WriteLine($"  Productos:    {string.Join(", ", Productos)}");
        Console.WriteLine($"  Dirección:    {DireccionEnvio}");
        Console.WriteLine($"  Regalo:       {(EnvolverParaRegalo ? "Sí" : "No")}");
        Console.WriteLine($"  Envío exprés: {(EnvioExprés ? "Sí" : "No")}");
        Console.WriteLine($"  Notas:        {Notas ?? "(sin notas)"}");
        Console.WriteLine($"  Pago:         {MetodoPago}");
    }
}

// --- Builder ---
public interface IPedidoBuilder
{
    void AgregarCliente(string nombre);
    void AgregarProducto(string producto);
    void AgregarDireccion(string direccion);
    void ConfigurarEnvioExprés();
    void ConfigurarRegalo();
    void AgregarNotas(string notas);
    void SeleccionarPago(string metodo);
    PedidoComplejo Construir();
}

// --- Builder concreto ---
public class PedidoBuilder : IPedidoBuilder
{
    private PedidoComplejo _pedido = new();

    public void AgregarCliente(string nombre)
    {
        _pedido.Cliente = nombre;
        Console.WriteLine($"  📝 Cliente registrado: {nombre}");
    }

    public void AgregarProducto(string producto)
    {
        _pedido.Productos.Add(producto);
        Console.WriteLine($"  📦 Producto agregado: {producto}");
    }

    public void AgregarDireccion(string direccion)
    {
        _pedido.DireccionEnvio = direccion;
        Console.WriteLine($"  📍 Dirección: {direccion}");
    }

    public void ConfigurarEnvioExprés()
    {
        _pedido.EnvioExprés = true;
        Console.WriteLine($"  ⚡ Envío exprés activado");
    }

    public void ConfigurarRegalo()
    {
        _pedido.EnvolverParaRegalo = true;
        Console.WriteLine($"  🎁 Envoltura para regalo");
    }

    public void AgregarNotas(string notas)
    {
        _pedido.Notas = notas;
        Console.WriteLine($"  📌 Notas: {notas}");
    }

    public void SeleccionarPago(string metodo)
    {
        _pedido.MetodoPago = metodo;
        Console.WriteLine($"  💳 Método de pago: {metodo}");
    }

    public PedidoComplejo Construir()
    {
        Console.WriteLine($"  🏁 Pedido construido exitosamente");
        var resultado = _pedido;
        _pedido = new PedidoComplejo(); // Reset para permitir reutilización
        return resultado;
    }
}

// --- Director (opcional) — guía el proceso de construcción ---
public class DirectorPedidos
{
    private readonly IPedidoBuilder _builder;

    public DirectorPedidos(IPedidoBuilder builder)
    {
        _builder = builder;
    }

    // Recipe predefinida: pedido simple sin registro
    public PedidoComplejo ConstruirPedidoSimple(string cliente, string producto, string direccion)
    {
        _builder.AgregarCliente(cliente);
        _builder.AgregarProducto(producto);
        _builder.AgregarDireccion(direccion);
        _builder.SeleccionarPago("Tarjeta");
        return _builder.Construir();
    }

    // Recipe predefinida: pedido premium con regalo
    public PedidoComplejo ConstruirPedidoPremium(string cliente, string[] productos, string direccion)
    {
        _builder.AgregarCliente(cliente);
        foreach (var p in productos) _builder.AgregarProducto(p);
        _builder.AgregarDireccion(direccion);
        _builder.ConfigurarEnvioExprés();
        _builder.ConfigurarRegalo();
        _builder.SeleccionarPago("PayPal");
        return _builder.Construir();
    }
}

public static class BuilderDemo
{
    public static void Run()
    {
        Console.WriteLine("  🧱 BUILDER — Objetos complejos paso a paso\n");
        Console.WriteLine("  Escenario: Sistema de pedidos con múltiples configuraciones\n");

        // ── Pedido simple (cliente usa el builder directamente) ──
        Console.WriteLine("  ── Pedido Simple ──");
        var builder = new PedidoBuilder();
        builder.AgregarCliente("Ana López");
        builder.AgregarProducto("Laptop HP");
        builder.AgregarDireccion("San José, Rohrmoser");
        builder.SeleccionarPago("Tarjeta");
        var pedido1 = builder.Construir();
        Console.WriteLine();
        pedido1.Resumen();
        Console.WriteLine();

        // ── Pedido premium (usando Director con recipe predefinida) ──
        Console.WriteLine("  ── Pedido Premium (vía Director) ──");
        var director = new DirectorPedidos(new PedidoBuilder());
        var pedido2 = director.ConstruirPedidoPremium(
            "Carlos Méndez",
            ["Monitor 27\"", "Teclado Mecánico", "Mouse Inalámbrico"],
            "Heredia, Santo Domingo"
        );
        Console.WriteLine();
        pedido2.Resumen();
        Console.WriteLine();

        Console.WriteLine("  ✅ El Builder permite crear objetos con diferentes");
        Console.WriteLine("     configuraciones sin constructores gigantes.");
    }
}
