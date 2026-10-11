using PinkByte.Api.Domain.Comun;
using PinkByte.Api.Domain.Habilidades;

namespace PinkByte.Api.Domain.Personajes;

// 🧬 HERENCIA: Protagonista "ES UN" Personaje. Hereda Vida, Ataque, RecibirDanio...
// y agrega lo suyo: nivel, café, lenguajes y habilidades.
public class Protagonista : Personaje
{
    // Listas privadas: afuera solo se pueden LEER (IReadOnlyList), no modificar.
    private readonly List<Lenguaje> _lenguajes = [];
    private readonly List<IHabilidad> _habilidades = [];

    public Estilo Estilo { get; }
    public string ColorPelo { get; }
    public int Nivel { get; private set; } = 1;
    public int Experiencia { get; private set; }
    public int CafeMaximo { get; private set; } = 10;
    public int Cafe { get; private set; }

    public IReadOnlyList<Lenguaje> Lenguajes => _lenguajes;
    public IReadOnlyList<IHabilidad> Habilidades => _habilidades;
    public int ExperienciaParaSubir => Nivel * 100;

    public Protagonista(string nombre, Estilo estilo, string colorPelo)
        : base(nombre, vidaMaxima: 100, ataque: 12, defensa: 4)   // 👈 llama al constructor del padre
    {
        Estilo = estilo;
        ColorPelo = colorPelo;
        Cafe = CafeMaximo;

        AplicarBonoDeEstilo();
        _habilidades.Add(new Teclazo());
        _habilidades.Add(new CafeRosa());
        AprenderLenguaje(Lenguaje.Html);   // todas empezamos por HTML 💅
    }

    public override string Descripcion =>
        $"{Nombre}, programadora {Estilo} de nivel {Nivel}. Sabe: {string.Join(", ", _lenguajes.Select(l => l.Nombre))}.";

    private void AplicarBonoDeEstilo()
    {
        switch (Estilo)
        {
            case Estilo.Gotica: Defensa += 3; break;
            case Estilo.Punk: Ataque += 4; break;
            case Estilo.Kawaii: VidaMaxima += 25; RestaurarVida(); break;
        }
    }

    /// <returns>true si lo aprendió; false si ya lo sabía.</returns>
    public bool AprenderLenguaje(Lenguaje lenguaje)
    {
        if (_lenguajes.Contains(lenguaje)) return false;

        _lenguajes.Add(lenguaje);
        _habilidades.Add(lenguaje.Habilidad);
        return true;
    }

    /// <returns>true si subió de nivel.</returns>
    public bool GanarExperiencia(int puntos)
    {
        Experiencia += puntos;
        bool subio = false;

        while (Experiencia >= ExperienciaParaSubir)
        {
            Experiencia -= ExperienciaParaSubir;
            Nivel++;
            VidaMaxima += 15;
            Ataque += 3;
            Defensa += 1;
            CafeMaximo += 2;
            subio = true;
        }
        return subio;
    }

    public void GastarCafe(int cantidad)
    {
        if (cantidad > Cafe)
            throw new ReglaDelJuegoException($"No te alcanza el café ☕ (tienes {Cafe}, necesitas {cantidad}). Usa Café Rosa.");
        Cafe -= cantidad;
    }

    public void RecargarCafe(int cantidad) => Cafe = Math.Min(CafeMaximo, Cafe + cantidad);

    public void PrepararParaCombate()
    {
        RestaurarVida();
        Cafe = CafeMaximo;
    }
}
