using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PinkByte.Api.Domain.Comun;

namespace PinkByte.Api.Middleware;

// Atrapa las excepciones del dominio y las convierte en respuestas HTTP bonitas.
// Así los controladores quedan limpios: sin try/catch en cada método.
public class ManejadorDeErrores : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception error, CancellationToken ct)
    {
        // `switch` con patrones de tipo: según QUÉ excepción es, elegimos el código HTTP.
        int? codigo = error switch
        {
            ReglaDelJuegoException => StatusCodes.Status400BadRequest,
            NoEncontradoException => StatusCodes.Status404NotFound,
            _ => null
        };

        if (codigo is null) return false;   // otros errores: que los maneje ASP.NET (500)

        http.Response.StatusCode = codigo.Value;
        await http.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = codigo,
            Title = error.Message
        }, ct);
        return true;
    }
}
