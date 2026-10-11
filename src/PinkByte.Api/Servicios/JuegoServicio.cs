using PinkByte.Api.Domain.Combates;
using PinkByte.Api.Domain.Comun;
using PinkByte.Api.Domain.Personajes;
using PinkByte.Api.Domain.Villanos;
using PinkByte.Api.Repositorios;

namespace PinkByte.Api.Servicios;

public interface IJuegoServicio
{
    Protagonista CrearProtagonista(string nombre, Estilo estilo, string colorPelo);
    Protagonista ObtenerProtagonista(Guid id);
    Combate IniciarCombate(Guid protagonistaId, string tipoVillano);
    Combate JugarTurno(Guid combateId, string habilidad);
}

// El servicio ORQUESTA: busca, crea, guarda. Las REGLAS viven en el dominio (Combate, Protagonista).
// 💉 INYECCIÓN DE DEPENDENCIAS: no hacemos `new RepositorioEnMemoria()`, nos lo "inyectan"
// por el constructor. Así es fácil cambiarlo o probarlo.
public class JuegoServicio(
    IRepositorio<Protagonista> protagonistas,
    IRepositorio<Combate> combates) : IJuegoServicio
{
    public Protagonista CrearProtagonista(string nombre, Estilo estilo, string colorPelo)
    {
        var protagonista = new Protagonista(nombre, estilo, colorPelo);
        protagonistas.Guardar(protagonista);
        return protagonista;
    }

    public Protagonista ObtenerProtagonista(Guid id) =>
        protagonistas.Buscar(id) ?? throw new NoEncontradoException("Esa protagonista no existe (¿reiniciaste el servidor?).");

    public Combate IniciarCombate(Guid protagonistaId, string tipoVillano)
    {
        var combate = new Combate(ObtenerProtagonista(protagonistaId), FabricaDeVillanos.Crear(tipoVillano));
        combates.Guardar(combate);
        return combate;
    }

    public Combate JugarTurno(Guid combateId, string habilidad)
    {
        var combate = combates.Buscar(combateId) ?? throw new NoEncontradoException("Ese combate no existe.");
        combate.JugarTurno(habilidad);
        return combate;
    }
}
