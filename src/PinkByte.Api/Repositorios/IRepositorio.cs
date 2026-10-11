using PinkByte.Api.Domain.Comun;

namespace PinkByte.Api.Repositorios;

// 🧪 GENÉRICOS: <T> es "cualquier tipo", con la condición (where) de que sea una IEntidad.
// Un solo contrato sirve para guardar Protagonistas, Combates o lo que venga.
public interface IRepositorio<T> where T : IEntidad
{
    void Guardar(T entidad);
    T? Buscar(Guid id);
}
