using Microsoft.AspNetCore.Mvc;
using PinkByte.Api.DTOs;
using PinkByte.Api.Servicios;

namespace PinkByte.Api.Controllers;

[ApiController]
[Route("api/combates")]
public class CombatesController(IJuegoServicio juego) : ControllerBase
{
    // POST api/combates  → empieza una pelea
    [HttpPost]
    public ActionResult<CombateDto> Iniciar(IniciarCombateRequest request) =>
        juego.IniciarCombate(request.ProtagonistaId, request.TipoVillano).ADto();

    // POST api/combates/{id}/turnos  → usa una habilidad
    [HttpPost("{id:guid}/turnos")]
    public ActionResult<CombateDto> JugarTurno(Guid id, TurnoRequest request) =>
        juego.JugarTurno(id, request.Habilidad).ADto();
}
