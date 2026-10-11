using PinkByte.Api.Domain.Comun;
using PinkByte.Api.Domain.Personajes;
using PinkByte.Api.Domain.Villanos;

namespace PinkByte.Api.Domain.Combates;

public enum EstadoCombate { EnCurso, Victoria, Derrota }

// El combate es dueño de sus reglas: nadie puede cambiar el Estado desde afuera,
// solo jugando turnos. Eso es ENCAPSULAMIENTO a nivel de "objeto de negocio".
public class Combate : IEntidad
{
    private readonly List<string> _registro = [];

    public Guid Id { get; } = Guid.NewGuid();
    public Protagonista Protagonista { get; }
    public Villano Villano { get; }
    public int Turno { get; private set; } = 1;
    public EstadoCombate Estado { get; private set; } = EstadoCombate.EnCurso;
    public IReadOnlyList<string> Registro => _registro;

    public Combate(Protagonista protagonista, Villano villano)
    {
        Protagonista = protagonista;
        Villano = villano;
        Protagonista.PrepararParaCombate();
        _registro.Add($"⚔️ {villano.Nombre} aparece: \"{villano.Frase}\"");
    }

    public void JugarTurno(string nombreHabilidad)
    {
        if (Estado != EstadoCombate.EnCurso)
            throw new ReglaDelJuegoException("Este combate ya terminó.");

        var habilidad = Protagonista.Habilidades
            .FirstOrDefault(h => h.Nombre.Equals(nombreHabilidad, StringComparison.OrdinalIgnoreCase))
            ?? throw new ReglaDelJuegoException($"{Protagonista.Nombre} no conoce '{nombreHabilidad}'.");

        Protagonista.GastarCafe(habilidad.CostoCafe);

        _registro.Add($"— Turno {Turno} —");
        _registro.Add(habilidad.Ejecutar(Protagonista, Villano));   // 🎭 polimorfismo
        if (!Villano.EstaVivo)
        {
            Ganar();
            return;
        }

        _registro.Add(Villano.RealizarTurno(Protagonista));          // 🎭 polimorfismo
        if (!Protagonista.EstaVivo)
        {
            Estado = EstadoCombate.Derrota;
            _registro.Add($"💀 {Protagonista.Nombre} cae... pero las chicas rudas siempre vuelven a intentarlo.");
            return;
        }

        Protagonista.RecargarCafe(1);
        Turno++;
    }

    private void Ganar()
    {
        Estado = EstadoCombate.Victoria;
        _registro.Add($"🏆 ¡{Villano.Nombre} derrotado! +{Villano.ExperienciaQueOtorga} XP");

        if (Protagonista.GanarExperiencia(Villano.ExperienciaQueOtorga))
            _registro.Add($"✨ ¡Subiste a nivel {Protagonista.Nivel}!");

        // Pattern matching: "si no es null, llámalo `lenguaje`"
        if (Villano.LenguajeQueEnsena is { } lenguaje && Protagonista.AprenderLenguaje(lenguaje))
            _registro.Add($"📚 ¡Aprendiste {lenguaje.Nombre} {lenguaje.Icono}! Nueva habilidad: {lenguaje.Habilidad.Nombre}");
    }
}
