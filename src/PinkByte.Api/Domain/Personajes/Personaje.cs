using PinkByte.Api.Domain.Comun;

namespace PinkByte.Api.Domain.Personajes;

// 🧱 ABSTRACCIÓN: "Personaje" es una idea general. No existe un personaje "a secas",
// existen Protagonistas y Villanos. Por eso es abstract: no se puede hacer `new Personaje()`.
public abstract class Personaje : IEntidad
{
    // 🔒 ENCAPSULAMIENTO: el dato real está escondido (private).
    // Nadie desde afuera puede hacer `personaje._vida = -500`.
    private int _vida;

    public Guid Id { get; } = Guid.NewGuid();
    public string Nombre { get; }

    // `protected set`: solo esta clase y sus HIJAS pueden modificarlo.
    public int VidaMaxima { get; protected set; }
    public int Ataque { get; protected set; }
    public int Defensa { get; protected set; }

    // La propiedad controla las reglas: la vida nunca baja de 0 ni pasa del máximo.
    public int Vida
    {
        get => _vida;
        protected set => _vida = Math.Clamp(value, 0, VidaMaxima);
    }

    // Propiedad calculada: no guarda nada, se calcula cada vez que la lees.
    public bool EstaVivo => Vida > 0;

    // Constructor protected: solo las clases hijas lo llaman (con `: base(...)`).
    protected Personaje(string nombre, int vidaMaxima, int ataque, int defensa)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ReglaDelJuegoException("Todo personaje necesita un nombre.");

        Nombre = nombre.Trim();
        VidaMaxima = vidaMaxima;
        Vida = vidaMaxima;
        Ataque = ataque;
        Defensa = defensa;
    }

    // 🎭 virtual: las hijas PUEDEN cambiar cómo reciben daño (ej: el Bug Fantasma esquiva).
    // Devuelve el daño que realmente se recibió (0 = esquivó).
    public virtual int RecibirDanio(int danioBruto)
    {
        int danioReal = Math.Max(1, danioBruto - Defensa);
        Vida -= danioReal;
        return danioReal;
    }

    public void Curar(int cantidad) => Vida += cantidad;

    public void RestaurarVida() => Vida = VidaMaxima;

    // 🎭 abstract: las hijas DEBEN decir cómo se describen. Cada una lo hace distinto.
    public abstract string Descripcion { get; }
}
