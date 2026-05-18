namespace DesignPatterns.Structural;

/// PATRÓN FACADE
/// ─────────────
/// Proporciona una interfaz SIMPLIFICADA para un subsistema complejo.
/// Es como la fachada de un edificio — esconde la complejidad interna
/// y expone solo lo que el cliente necesita.
///
/// USO REAL: APIs de frameworks (ASP.NET MVC controller simplifica
///           el pipeline), bibliotecas de logging, sistemas de pedidos.

// --- Subsistema complejo #1: Inventario ---
public class SubsistemaInventario
{
    private readonly Dictionary<string, int> _stock = new()
    {
        ["Laptop"] = 5,
        ["Mouse"] = 50,
        ["Teclado"] = 20,
        ["Monitor"] = 8
    };

    public bool VerificarDisponibilidad(string producto, int cantidad)
    {
        var disponible = _stock.GetValueOrDefault(producto, 0);
        Console.WriteLine($"  [Inventario] {producto}: solicitados {cantidad}, disponibles {disponible}");
        return disponible >= cantidad;
    }

    public void Reservar(string producto, int cantidad)
    {
        if (_stock.ContainsKey(producto))
            _stock[producto] -= cantidad;
        Console.WriteLine($"  [Inventario] {cantidad}x {producto} reservados. Stock restante: {_stock.GetValueOrDefault(producto, 0)}");
    }
}

// --- Subsistema complejo #2: Pagos ---
public class SubsistemaPagos
{
    public bool ProcesarPago(string cliente, decimal monto, string metodo)
    {
        Console.WriteLine($"  [Pagos] Procesando ¢{monto:N2} de {cliente} vía {metodo}...");
        Thread.Sleep(100); // simula proceso
        Console.WriteLine($"  [Pagos] ✅ Pago aprobado");
        return true;
    }
}

// --- Subsistema complejo #3: Envíos ---
public class SubsistemaEnvios
{
    private static int _guiaCount = 1000;

    public string GenerarGuia(string cliente, string direccion)
    {
        _guiaCount++;
        var guia = $"CR-{_guiaCount:D5}";
        Console.WriteLine($"  [Envíos] Guía {guia} generada para {cliente}");
        Console.WriteLine($"  [Envíos] Dirección: {direccion}");
        return guia;
    }
}

// --- Subsistema complejo #4: Notificaciones ---
public class SubsistemaNotificaciones
{
    public void EnviarConfirmacion(string cliente, string guia)
    {
        Console.WriteLine($"  [Notif] ✉️ Correo de confirmación enviado a {cliente}");
        Console.WriteLine($"  [Notif] 📱 SMS con guía {guia} enviado");
    }
}

// --- FACADE: la interfaz simple que el cliente usa ---
public class FacadePedido
{
    private readonly SubsistemaInventario _inventario = new();
    private readonly SubsistemaPagos _pagos = new();
    private readonly SubsistemaEnvios _envios = new();
    private readonly SubsistemaNotificaciones _notificaciones = new();

    // Un solo método que orquesta todo el proceso complejo
    public string RealizarPedido(string cliente, string producto, int cantidad,
                                  decimal precio, string metodoPago, string direccion)
    {
        Console.WriteLine($"\n  🏪 Procesando pedido de {cliente}...");
        Console.WriteLine($"  Producto: {producto} x{cantidad}");

        // Paso 1: Verificar inventario
        if (!_inventario.VerificarDisponibilidad(producto, cantidad))
        {
            Console.WriteLine("  ❌ Producto agotado. Pedido cancelado.");
            return "";
        }

        // Paso 2: Reservar
        _inventario.Reservar(producto, cantidad);

        // Paso 3: Cobrar
        var total = precio * cantidad;
        if (!_pagos.ProcesarPago(cliente, total, metodoPago))
        {
            Console.WriteLine("  ❌ Pago fallido. Pedido cancelado.");
            return "";
        }

        // Paso 4: Generar envío
        var guia = _envios.GenerarGuia(cliente, direccion);

        // Paso 5: Notificar
        _notificaciones.EnviarConfirmacion(cliente, guia);

        return guia;
    }
}

public static class FacadeDemo
{
    public static void Run()
    {
        Console.WriteLine("  🏛️  FACADE — Interfaz simple para subsistemas complejos\n");
        Console.WriteLine("  Escenario: Tienda online — el cliente solo llama a\n" +
                         "  'RealizarPedido' y el Facade orquesta 4 subsistemas\n");

        var tienda = new FacadePedido();

        var guia = tienda.RealizarPedido(
            cliente: "María Rodríguez",
            producto: "Laptop",
            cantidad: 1,
            precio: 650000m,
            metodoPago: "Tarjeta",
            direccion: "Escazú, Plaza Itskatzú"
        );

        if (!string.IsNullOrEmpty(guia))
        {
            Console.WriteLine($"\n  🎉 Pedido completado. Guía: {guia}");
        }

        Console.WriteLine();
        Console.WriteLine("  ✅ El cliente solo ve 1 método.");
        Console.WriteLine("     La complejidad del inventario, pagos, envíos y");
        Console.WriteLine("     notificaciones queda oculta tras la fachada.");
    }
}
