using Microsoft.AspNetCore.Mvc;
using PinkByte.Api.Domain.Villanos;
using PinkByte.Api.DTOs;

namespace PinkByte.Api.Controllers;

[ApiController]
[Route("api/villanos")]
public class VillanosController : ControllerBase
{
    // GET api/villanos  → el mapa de enemigos en orden de historia
    [HttpGet]
    public IEnumerable<VillanoDto> Catalogo() => FabricaDeVillanos.Catalogo().Select(v => v.ADto());
}
