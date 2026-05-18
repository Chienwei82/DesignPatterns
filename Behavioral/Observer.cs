namespace DesignPatterns.Behavioral;

/// PATRÓN OBSERVER
/// ───────────────
/// Define una dependencia UNO-A-MUCHOS entre objetos, de modo que
/// cuando UN objeto cambia su estado, TODOS sus dependientes son
/// notificados automáticamente. Es como un sistema de suscripción.
///
/// USO REAL: Eventos en UI (click, keypress), sistemas de colas,
///           notificaciones push, actualización de vistas en MVVM,
///           el patrón event/delegate de C#.

// --- Sujeto (observable) ---
public interface ISujetoNoticias
{
    void Suscribir(IObservadorNoticias observador);
    void Desuscribir(IObservadorNoticias observador);
    void Notificar(string categoria, string titular);
}

// --- Observador ---
public interface IObservadorNoticias
{
    string Nombre { get; }
    void RecibirNoticia(string categoria, string titular);
}

// --- Sujeto concreto ---
public class AgenciaNoticias : ISujetoNoticias
{
    private readonly List<IObservadorNoticias> _suscriptores = [];

    public void Suscribir(IObservadorNoticias observador)
    {
        _suscriptores.Add(observador);
        Console.WriteLine($"  📰 [Agencia] {observador.Nombre} se suscribió a noticias");
    }

    public void Desuscribir(IObservadorNoticias observador)
    {
        _suscriptores.Remove(observador);
        Console.WriteLine($"  📰 [Agencia] {observador.Nombre} canceló suscripción");
    }

    public void Notificar(string categoria, string titular)
    {
        Console.WriteLine($"  📰 [Agencia] Publicando noticia [{categoria}]: {titular}");
        Console.WriteLine($"    Notificando a {_suscriptores.Count} suscriptor(es)...");

        foreach (var suscriptor in _suscriptores)
        {
            suscriptor.RecibirNoticia(categoria, titular);
        }
        Console.WriteLine();
    }

    // Método de alto nivel para publicar
    public void PublicarNoticia(string categoria, string titular)
    {
        Console.WriteLine($"\n  📢 NUEVA NOTICIA: [{categoria}] {titular}");
        Notificar(categoria, titular);
    }
}

// --- Observadores concretos ---

public class SuscriptorEmail : IObservadorNoticias
{
    public string Nombre { get; }

    public SuscriptorEmail(string nombre) => Nombre = nombre;

    public void RecibirNoticia(string categoria, string titular)
    {
        Console.WriteLine($"    ✉️ [Email->{Nombre}] Noticia enviada a su correo");
    }
}

public class SuscriptorSMS : IObservadorNoticias
{
    public string Nombre { get; }
    private readonly string _telefono;

    public SuscriptorSMS(string nombre, string telefono)
    {
        Nombre = nombre;
        _telefono = telefono;
    }

    public void RecibirNoticia(string categoria, string titular)
    {
        Console.WriteLine($"    📱 [SMS->{Nombre}] SMS enviado al {_telefono}");
    }
}

public class SuscriptorApp : IObservadorNoticias
{
    public string Nombre { get; }

    public SuscriptorApp(string nombre) => Nombre = nombre;

    public void RecibirNoticia(string categoria, string titular)
    {
        Console.WriteLine($"    🔔 [App->{Nombre}] Notificación push recibida: \"{titular}\"");
    }
}

// --- Observador con filtro (solo recibe ciertas categorías) ---
public class SuscriptorFiltrado : IObservadorNoticias
{
    public string Nombre { get; }
    private readonly string _categoriaInteres;

    public SuscriptorFiltrado(string nombre, string categoriaInteres)
    {
        Nombre = nombre;
        _categoriaInteres = categoriaInteres;
    }

    public void RecibirNoticia(string categoria, string titular)
    {
        if (categoria != _categoriaInteres)
        {
            Console.WriteLine($"    🔇 [{Nombre}] Noticia ignorada (categoría '{categoria}' no es '{_categoriaInteres}')");
            return;
        }
        Console.WriteLine($"    ✅ [{Nombre}] Noticia de '{categoria}' recibida: {titular}");
    }
}

public static class ObserverDemo
{
    public static void Run()
    {
        Console.WriteLine("  👁️  OBSERVER — Suscripción y notificación en tiempo real\n");
        Console.WriteLine("  Escenario: Aplicación de noticias con múltiples canales\n");

        // Sujeto
        var agencia = new AgenciaNoticias();

        // Crear suscriptores
        var jose = new SuscriptorEmail("José");
        var maria = new SuscriptorSMS("María", "+5068888-1234");
        var ana = new SuscriptorApp("Ana");
        var deportes = new SuscriptorFiltrado("Carlos 🏀", "Deportes");

        // Suscribir
        agencia.Suscribir(jose);
        agencia.Suscribir(maria);
        agencia.Suscribir(ana);
        agencia.Suscribir(deportes);
        Console.WriteLine();

        // Publicar noticias
        agencia.PublicarNoticia("Tecnología", "Microsoft lanza .NET 11 preview");
        agencia.PublicarNoticia("Deportes", "Costa Rica clasifica al mundial 2030");

        // Cancelar suscripción
        agencia.Desuscribir(ana);
        Console.WriteLine();

        // Publicar otra — Ana ya no recibe
        agencia.PublicarNoticia("Economía", "Tipo de cambio del dólar estable");

        Console.WriteLine("  ✅ Observer desacopla el emisor de los receptores.");
        Console.WriteLine("     Nuevos canales se agregan sin modificar la agencia.");
    }
}
