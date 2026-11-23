namespace DesignPatterns.Behavioral;

/// ============================================================================
/// PATRÓN MEDIATOR — Comunicación centralizada
/// ============================================================================
/// Intención: definir un objeto que encapsula cómo interactúan un conjunto de
/// objetos. Promueve el acoplamiento débil evitando que los objetos se
/// referencien explícitamente entre sí.
///
/// Analogía: torre de control: ningún piloto coordina con otro directamente;
/// todo pasa por la torre.
///
/// Cuándo usarlo: salas de chat, paneles UI que se afectan entre sí,
/// coordinación de formularios.
///
/// Cuándo NO usarlo: si solo dos objetos se comunican, el mediador es un
/// intermediario innecesario.
///
/// USO REAL: Salas de chat, control de tráfico aéreo, coordinadores de
/// workflows, mediadores en frameworks UI (MVVM).
/// ============================================================================

// --- Mediator ---
public interface IChatRoom
{
    void Registrar(Usuario usuario);
    void Enviar(string mensaje, Usuario remitente);
}

// --- Colega ---
public abstract class Usuario
{
    public string Nombre { get; }
    private IChatRoom? _chat;

    protected Usuario(string nombre) => Nombre = nombre;

    public void UnirseAlChat(IChatRoom chat)
    {
        _chat = chat;
        _chat.Registrar(this);
    }

    public void Enviar(string mensaje)
    {
        Console.WriteLine($"  [{Nombre}] >> {mensaje}");
        _chat?.Enviar(mensaje, this);
    }

    public abstract void Recibir(string mensaje, string remitente);
}

// --- Mediador concreto: la sala de chat ---
public class ChatRoom : IChatRoom
{
    private readonly List<Usuario> _usuarios = [];
    private readonly string _nombre;

    public ChatRoom(string nombre) => _nombre = nombre;

    public void Registrar(Usuario usuario)
    {
        _usuarios.Add(usuario);
        Console.WriteLine($"  [Chat '{_nombre}'] {usuario.Nombre} se unió. Total: {_usuarios.Count} usuarios");
    }

    public void Enviar(string mensaje, Usuario remitente)
    {
        foreach (var usuario in _usuarios)
        {
            if (usuario != remitente)
                usuario.Recibir(mensaje, remitente.Nombre);
        }
    }
}

// --- Colegas concretos ---
public class UsuarioNormal : Usuario
{
    public UsuarioNormal(string nombre) : base(nombre) { }
    public override void Recibir(string mensaje, string remitente) =>
        Console.WriteLine($"    💬 [{Nombre}] Nuevo mensaje de {remitente}: \"{mensaje}\"");
}

public class UsuarioAdmin : Usuario
{
    public UsuarioAdmin(string nombre) : base(nombre) { }
    public override void Recibir(string mensaje, string remitente) =>
        Console.WriteLine($"    👑 [{Nombre}] [ADMIN] Mensaje de {remitente}: \"{mensaje}\"");
}

public static class MediatorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🗣️  MEDIATOR — Comunicación centralizada\n");
        Console.WriteLine("  Escenario: Una sala de chat donde los usuarios se comunican a través del mediador\n");

        var chat = new ChatRoom("Sala General");

        var ana = new UsuarioNormal("Ana");
        var admin = new UsuarioAdmin("Admin");

        ana.UnirseAlChat(chat);
        admin.UnirseAlChat(chat);
        Console.WriteLine();

        ana.Enviar("¡Hola a todos!");
        Console.WriteLine();

        admin.Enviar("Bienvenidos al chat.");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los usuarios NO se conocen entre sí.");
        Console.WriteLine("     Todo pasa por el chat (mediator).");
        Console.WriteLine("     Agregar/eliminar gente no rompe el código de nadie.");
    }
}
