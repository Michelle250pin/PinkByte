using PinkByte.Api.Domain.Personajes;

namespace PinkByte.Api.Domain.Habilidades;

// 📜 INTERFAZ: todas las habilidades cumplen este contrato.
// El combate NO sabe qué habilidad es; solo llama a Ejecutar(). Eso es 🎭 POLIMORFISMO.
public interface IHabilidad
{
    string Nombre { get; }
    string Descripcion { get; }
    int CostoCafe { get; }

    /// <returns>El texto que se mostrará en el registro del combate.</returns>
    string Ejecutar(Protagonista lanzadora, Personaje objetivo);
}
