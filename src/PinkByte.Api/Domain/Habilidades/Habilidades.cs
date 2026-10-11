using PinkByte.Api.Domain.Personajes;

namespace PinkByte.Api.Domain.Habilidades;

// Cada clase implementa IHabilidad a SU manera. Mismo contrato, comportamientos distintos.

public class Teclazo : IHabilidad
{
    public string Nombre => "Teclazo";
    public string Descripcion => "Golpe básico con el teclado mecánico rosa.";
    public int CostoCafe => 0;

    public string Ejecutar(Protagonista lanzadora, Personaje objetivo)
    {
        int danio = objetivo.RecibirDanio(lanzadora.Ataque + Random.Shared.Next(0, 5));
        return Narrador.Golpe(lanzadora, objetivo, "lanza un Teclazo ⌨️", danio);
    }
}

public class CafeRosa : IHabilidad
{
    public string Nombre => "Café Rosa";
    public string Descripcion => "Recupera 5 de café y 10 de vida.";
    public int CostoCafe => 0;

    public string Ejecutar(Protagonista lanzadora, Personaje objetivo)
    {
        lanzadora.RecargarCafe(5);
        lanzadora.Curar(10);
        return $"☕ {lanzadora.Nombre} toma un latte de fresa. +5 café, +10 vida.";
    }
}

// --- Habilidades que se desbloquean al aprender lenguajes ---

public class MaquetarEscudo : IHabilidad
{
    public string Nombre => "<div> Escudo";
    public string Descripcion => "HTML: maqueta una barrera y recupera 25 de vida.";
    public int CostoCafe => 3;

    public string Ejecutar(Protagonista lanzadora, Personaje objetivo)
    {
        lanzadora.Curar(25);
        return $"🧱 {lanzadora.Nombre} envuelve todo en un <div> protector. +25 vida.";
    }
}

public class PromesaAsincrona : IHabilidad
{
    public string Nombre => "Promise.all()";
    public string Descripcion => "JavaScript: dos ataques en paralelo.";
    public int CostoCafe => 3;

    public string Ejecutar(Protagonista lanzadora, Personaje objetivo)
    {
        int golpe1 = objetivo.RecibirDanio(lanzadora.Ataque);
        int golpe2 = objetivo.RecibirDanio(lanzadora.Ataque);
        return $"⚡ {lanzadora.Nombre} resuelve dos promesas a la vez: {Narrador.Danio(golpe1)} y {Narrador.Danio(golpe2)}.";
    }
}

public class PolimorfismoLetal : IHabilidad
{
    public string Nombre => "Polimorfismo Letal";
    public string Descripcion => "C#: un override que hace el doble de daño.";
    public int CostoCafe => 5;

    public string Ejecutar(Protagonista lanzadora, Personaje objetivo)
    {
        int danio = objetivo.RecibirDanio(lanzadora.Ataque * 2);
        return Narrador.Golpe(lanzadora, objetivo, "hace override de la realidad 💜", danio);
    }
}

public class DropTable : IHabilidad
{
    public string Nombre => "DROP TABLE";
    public string Descripcion => "SQL: borra el 35% de la vida máxima del enemigo, ignora defensa.";
    public int CostoCafe => 6;

    public string Ejecutar(Protagonista lanzadora, Personaje objetivo)
    {
        // Se suma la defensa para que RecibirDanio la "cancele" y el daño sea exacto.
        int danio = objetivo.RecibirDanio(objetivo.VidaMaxima * 35 / 100 + objetivo.Defensa);
        return Narrador.Golpe(lanzadora, objetivo, "ejecuta DROP TABLE villanos; 🗄️", danio);
    }
}

// `static`: no se instancia; son funciones de ayuda para narrar el combate.
public static class Narrador
{
    public static string Danio(int danio) => danio == 0 ? "¡esquivado!" : $"{danio} de daño";

    public static string Golpe(Personaje atacante, Personaje objetivo, string accion, int danio) =>
        danio == 0
            ? $"{atacante.Nombre} {accion}... ¡pero {objetivo.Nombre} lo esquiva!"
            : $"{atacante.Nombre} {accion}: {danio} de daño a {objetivo.Nombre}.";
}
