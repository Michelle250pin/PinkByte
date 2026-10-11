namespace PinkByte.Api.Domain.Habilidades;

// Un lenguaje de programación que la protagonista puede aprender.
// Constructor PRIVADO + instancias estáticas = solo existen los lenguajes definidos aquí.
// (Se le llama "enum inteligente": como un enum, pero con datos y comportamiento).
public sealed class Lenguaje
{
    public string Nombre { get; }
    public string Icono { get; }
    public IHabilidad Habilidad { get; }

    private Lenguaje(string nombre, string icono, IHabilidad habilidad)
    {
        Nombre = nombre;
        Icono = icono;
        Habilidad = habilidad;
    }

    public static readonly Lenguaje Html = new("HTML", "🧱", new MaquetarEscudo());
    public static readonly Lenguaje JavaScript = new("JavaScript", "⚡", new PromesaAsincrona());
    public static readonly Lenguaje CSharp = new("C#", "💜", new PolimorfismoLetal());
    public static readonly Lenguaje Sql = new("SQL", "🗄️", new DropTable());
}
