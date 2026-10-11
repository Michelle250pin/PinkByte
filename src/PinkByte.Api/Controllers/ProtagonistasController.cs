using Microsoft.AspNetCore.Mvc;
using PinkByte.Api.DTOs;
using PinkByte.Api.Servicios;

namespace PinkByte.Api.Controllers;

// 🎮 CONTROLADOR: recibe peticiones HTTP, llama al servicio y responde JSON.
// No tiene lógica del juego: solo "traduce" entre HTTP y el dominio.
[ApiController]
[Route("api/protagonistas")]
public class ProtagonistasController(IJuegoServicio juego) : ControllerBase
{
    // POST api/protagonistas  → crea tu personaje
    [HttpPost]
    public ActionResult<ProtagonistaDto> Crear(CrearProtagonistaRequest request)
    {
        var protagonista = juego.CrearProtagonista(request.Nombre, request.Estilo, request.ColorPelo);
        return CreatedAtAction(nameof(Obtener), new { id = protagonista.Id }, protagonista.ADto());
    }

    // GET api/protagonistas/{id}
    [HttpGet("{id:guid}")]
    public ActionResult<ProtagonistaDto> Obtener(Guid id) => juego.ObtenerProtagonista(id).ADto();
}
