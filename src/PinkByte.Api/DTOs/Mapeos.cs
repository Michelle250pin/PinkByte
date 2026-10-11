using PinkByte.Api.Domain.Combates;
using PinkByte.Api.Domain.Habilidades;
using PinkByte.Api.Domain.Personajes;
using PinkByte.Api.Domain.Villanos;

namespace PinkByte.Api.DTOs;

// 🪄 MÉTODOS DE EXTENSIÓN: el `this` en el primer parámetro permite escribir
// `protagonista.ADto()` como si el método fuera parte de la clase Protagonista.
public static class Mapeos
{
    public static LenguajeDto ADto(this Lenguaje l) => new(l.Nombre, l.Icono);

    public static HabilidadDto ADto(this IHabilidad h) => new(h.Nombre, h.Descripcion, h.CostoCafe);

    public static ProtagonistaDto ADto(this Protagonista p) => new(
        p.Id, p.Nombre, p.Estilo, p.ColorPelo, p.Descripcion,
        p.Nivel, p.Experiencia, p.ExperienciaParaSubir,
        p.Vida, p.VidaMaxima, p.Ataque, p.Defensa, p.Cafe, p.CafeMaximo,
        p.Lenguajes.Select(l => l.ADto()),
        p.Habilidades.Select(h => h.ADto()));

    public static VillanoDto ADto(this Villano v) => new(
        v.Tipo, v.Nombre, v.Descripcion, v.Frase,
        v.Vida, v.VidaMaxima, v.Ataque, v.Defensa,
        v.ExperienciaQueOtorga, v.LenguajeQueEnsena?.ADto());

    public static CombateDto ADto(this Combate c) => new(
        c.Id, c.Turno, c.Estado.ToString(),
        c.Protagonista.ADto(), c.Villano.ADto(), c.Registro);
}
