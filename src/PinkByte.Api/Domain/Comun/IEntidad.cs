namespace PinkByte.Api.Domain.Comun;

// 📜 INTERFAZ: es un "contrato". Quien la implemente PROMETE tener un Id.
// Gracias a esto, un mismo repositorio genérico sirve para personajes, combates, etc.
public interface IEntidad
{
    Guid Id { get; }
}
