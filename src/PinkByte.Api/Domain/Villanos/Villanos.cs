using PinkByte.Api.Domain.Habilidades;
using PinkByte.Api.Domain.Personajes;

namespace PinkByte.Api.Domain.Villanos;

// Cada villano: hereda de Villano y sobreescribe (override) su forma de pelear.

/// <summary>Nivel 1. Aparece y desaparece. 30% de esquivar. Enseña JavaScript.</summary>
public class BugFantasma() : Villano("Bug Fantasma", vida: 60, ataque: 10, defensa: 2, experiencia: 60, Lenguaje.JavaScript)
{
    public override string Descripcion => "Un error que solo aparece en producción. Esquiva el 30% de los ataques.";
    public override string Frase => "En mi máquina funciona... jajaja 👻";

    // override de un método virtual del abuelo (Personaje).
    public override int RecibirDanio(int danioBruto)
    {
        if (Random.Shared.Next(100) < 30) return 0;   // ¡esquivó!
        return base.RecibirDanio(danioBruto);         // `base` = usa la versión del padre
    }

    public override string RealizarTurno(Protagonista objetivo)
    {
        int danio = objetivo.RecibirDanio(TirarAtaque());
        return Narrador.Golpe(this, objetivo, "lanza un NullReferenceException 👻", danio);
    }
}

/// <summary>Nivel 2. Golpea de 1 a 3 veces, enredado. Enseña C#.</summary>
public class SpaghettiCode() : Villano("Señor Spaghetti Code", vida: 90, ataque: 7, defensa: 3, experiencia: 90, Lenguaje.CSharp)
{
    public override string Descripcion => "Un monstruo de 5000 líneas en un solo archivo. Ataca de 1 a 3 veces.";
    public override string Frase => "¿Clases? ¿Para qué? Todo cabe en el Main 🍝";

    public override string RealizarTurno(Protagonista objetivo)
    {
        int golpes = Random.Shared.Next(1, 4);
        int total = 0;
        for (int i = 0; i < golpes; i++)
            total += objetivo.RecibirDanio(TirarAtaque());

        return $"🍝 {Nombre} te enreda con {golpes} goto(s): {total} de daño total.";
    }
}

/// <summary>Nivel 3. Cada turno se hace más fuerte. Enseña SQL.</summary>
public class LaDeadline() : Villano("La Deadline", vida: 120, ataque: 8, defensa: 5, experiencia: 130, Lenguaje.Sql)
{
    public override string Descripcion => "El reloj no perdona. Su ataque sube +3 cada turno: ¡termina rápido!";
    public override string Frase => "Era para ayer ⏰";

    public override string RealizarTurno(Protagonista objetivo)
    {
        Ataque += 3;   // puede hacerlo porque Ataque tiene `protected set`
        int danio = objetivo.RecibirDanio(TirarAtaque());
        return $"⏰ {Nombre} adelanta la entrega (ataque {Ataque}): {danio} de daño.";
    }
}

/// <summary>JEFE FINAL. Más fuerte si sabes pocos lenguajes. No enseña nada: te deja pasar a la entrevista.</summary>
public class SindromeDelImpostor() : Villano("Síndrome del Impostor", vida: 170, ataque: 13, defensa: 6, experiencia: 300, ensena: null)
{
    private const int LenguajesParaResistir = 4;

    public override string Descripcion =>
        $"Se alimenta de tus dudas. Si sabes menos de {LenguajesParaResistir} lenguajes, pega el doble.";
    public override string Frase => "No eres suficiente para esa empresa... ¿o sí? 🖤";

    public override string RealizarTurno(Protagonista objetivo)
    {
        bool dudas = objetivo.Lenguajes.Count < LenguajesParaResistir;
        int danio = objetivo.RecibirDanio(dudas ? TirarAtaque() * 2 : TirarAtaque());

        return dudas
            ? $"🖤 {Nombre} susurra \"no sabes lo suficiente\": {danio} de daño (¡aprende más lenguajes!)."
            : $"🖤 {Nombre} intenta hacerte dudar, pero tu experiencia te protege: solo {danio} de daño.";
    }
}
