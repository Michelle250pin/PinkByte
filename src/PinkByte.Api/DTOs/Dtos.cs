using PinkByte.Api.Domain.Personajes;

namespace PinkByte.Api.DTOs;

// 📦 DTO = Data Transfer Object: la "forma" de los datos que viajan por HTTP (JSON).
// Separamos DTOs del dominio para no exponer la lógica interna ni romper el front
// cada vez que cambiamos una clase. `record` = clase inmutable, ideal para datos.

// --- Lo que ENTRA (requests) ---
public record CrearProtagonistaRequest(string Nombre, Estilo Estilo, string ColorPelo);
public record IniciarCombateRequest(Guid ProtagonistaId, string TipoVillano);
public record TurnoRequest(string Habilidad);

// --- Lo que SALE (responses) ---
public record HabilidadDto(string Nombre, string Descripcion, int CostoCafe);

public record LenguajeDto(string Nombre, string Icono);

public record ProtagonistaDto(
    Guid Id, string Nombre, Estilo Estilo, string ColorPelo, string Descripcion,
    int Nivel, int Experiencia, int ExperienciaParaSubir,
    int Vida, int VidaMaxima, int Ataque, int Defensa, int Cafe, int CafeMaximo,
    IEnumerable<LenguajeDto> Lenguajes, IEnumerable<HabilidadDto> Habilidades);

public record VillanoDto(
    string Tipo, string Nombre, string Descripcion, string Frase,
    int Vida, int VidaMaxima, int Ataque, int Defensa,
    int Experiencia, LenguajeDto? Ensena);

public record CombateDto(
    Guid Id, int Turno, string Estado,
    ProtagonistaDto Protagonista, VillanoDto Villano, IEnumerable<string> Registro);
