using PinkByte.Api.Domain.Habilidades;
using PinkByte.Api.Domain.Personajes;

namespace PinkByte.Api.Domain.Villanos;

// 🧬 Villano también hereda de Personaje, pero sigue siendo abstract:
// es una categoría. Los villanos concretos (BugFantasma, LaDeadline...) heredan de aquí.
public abstract class Villano : Personaje
{
    public int ExperienciaQueOtorga { get; }

    // `?` = puede ser null. El jefe final no enseña ningún lenguaje.
    public Lenguaje? LenguajeQueEnsena { get; }

    // El nombre de la clase sirve como "tipo" (ej: "BugFantasma").
    public string Tipo => GetType().Name;

    protected Villano(string nombre, int vida, int ataque, int defensa, int experiencia, Lenguaje? ensena)
        : base(nombre, vida, ataque, defensa)
    {
        ExperienciaQueOtorga = experiencia;
        LenguajeQueEnsena = ensena;
    }

    public abstract string Frase { get; }

    // 🎭 POLIMORFISMO: el combate solo dice "villano, haz tu turno".
    // CADA villano decide qué hacer. El combate no necesita ni un solo if por tipo.
    public abstract string RealizarTurno(Protagonista objetivo);

    // Ayuda para las hijas: ataque con un poco de azar.
    protected int TirarAtaque() => Ataque + Random.Shared.Next(0, 5);
}
