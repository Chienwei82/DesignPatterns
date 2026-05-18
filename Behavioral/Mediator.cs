namespace DesignPatterns.Behavioral;

/// PATRÓN MEDIATOR
/// ───────────────
/// Define un objeto que encapsula CÓMO interactúan un conjunto de objetos.
/// Promueve el acoplamiento débil evitando que los objetos se referencien
/// explícitamente entre sí. Es como un "switchboard" o sala de chat.
///
/// USO REAL: Salas de chat, control de tráfico aéreo, coordenadores
///           de workflows, mediadores en frameworks UI (MVVM).

// --- Mediator ---
public interface ISalaChat
{
    void EnviarMensaje(string mensaje, Usuario remitente);
    void RegistrarUsuario(Usuario usuario);
}

// --- Colleague ---
public class Usuario
{
    public string Nombre { get; }
    private ISalaChat? _sala;

    public Usuario(string nombre)
    {
        Nombre = nombre;
    }

    public void UnirseASala(ISalaChat sala)
    {
        _sala = sala;
        _sala.RegistrarUsuario(this);
    }

    public void Enviar(string mensaje)
    {
        Console.WriteLine($"  [{Nombre}] >> {mensaje}");
        _sala?.EnviarMensaje(mensaje, this);
    }

    public void Recibir(string mensaje, string remitente)
    {
        if (remitente != Nombre)
            Console.WriteLine($"    [{Nombre}] << {remitente}: {mensaje}");
    }
}

// --- Concrete Mediator ---
public class SalaChatGrupal : ISalaChat
{
    private readonly List<Usuario> _usuarios = [];
    private readonly string _nombre;

    public SalaChatGrupal(string nombre)
    {
        _nombre = nombre;
    }

    public void RegistrarUsuario(Usuario usuario)
    {
        _usuarios.Add(usuario);
        Console.WriteLine($"  [Sala '{_nombre}'] {usuario.Nombre} se unió. Total: {_usuarios.Count}");
    }

    public void EnviarMensaje(string mensaje, Usuario remitente)
    {
        foreach (var usuario in _usuarios)
        {
            usuario.Recibir(mensaje, remitente.Nombre);
        }
    }
}

public static class MediatorDemo
{
    public static void Run()
    {
        Console.WriteLine("  🗣️  MEDIATOR — Comunicación centralizada\n");
        Console.WriteLine("  Escenario: Sala de chat grupal\n");

        var sala = new SalaChatGrupal("Developers CR");

        var ana = new Usuario("Ana");
        var carlos = new Usuario("Carlos");
        var maria = new Usuario("María");

        ana.UnirseASala(sala);
        carlos.UnirseASala(sala);
        maria.UnirseASala(sala);
        Console.WriteLine();

        ana.Enviar("¡Hola equipo! ¿Listos para el sprint?");
        Console.WriteLine();

        carlos.Enviar("Sí, ya terminé mis tareas.");
        Console.WriteLine();

        maria.Enviar("Yo también. Hagamos la daily mañana.");
        Console.WriteLine();

        Console.WriteLine("  ✅ Los usuarios NO se conocen entre sí.");
        Console.WriteLine("     Todo pasa por la sala (mediator).");
        Console.WriteLine("     Agregar/eliminar usuarios no rompe el código de nadie.");
    }
}
