using System.Collections.Concurrent;
using PinkByte.Api.Domain.Comun;

namespace PinkByte.Api.Repositorios;

// Guarda todo en memoria (se pierde al reiniciar). En una fase futura lo cambiaremos
// por una base de datos real SIN tocar el resto del código, porque todos dependen
// de la interfaz IRepositorio, no de esta clase. (Eso es la "D" de SOLID).
public class RepositorioEnMemoria<T> : IRepositorio<T> where T : IEntidad
{
    // ConcurrentDictionary: seguro si llegan varias peticiones al mismo tiempo.
    private readonly ConcurrentDictionary<Guid, T> _datos = new();

    public void Guardar(T entidad) => _datos[entidad.Id] = entidad;

    public T? Buscar(Guid id) => _datos.GetValueOrDefault(id);
}
