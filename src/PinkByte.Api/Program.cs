using System.Text.Json.Serialization;
using PinkByte.Api.Middleware;
using PinkByte.Api.Repositorios;
using PinkByte.Api.Servicios;

var builder = WebApplication.CreateBuilder(args);

// ---------- 1. Registrar servicios (Inyección de dependencias) ----------
builder.Services.AddControllers()
    // Los enums viajan como texto ("Punk") en vez de número (1).
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton = una sola instancia para toda la app (si no, los datos en memoria se perderían).
builder.Services.AddSingleton(typeof(IRepositorio<>), typeof(RepositorioEnMemoria<>));
builder.Services.AddSingleton<IJuegoServicio, JuegoServicio>();

builder.Services.AddExceptionHandler<ManejadorDeErrores>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// ---------- 2. Pipeline: por dónde pasa cada petición ----------
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   // prueba la API en /swagger
}

app.UseDefaultFiles();    // sirve wwwroot/index.html en "/"
app.UseStaticFiles();     // sirve css, js, imágenes de wwwroot

app.MapControllers();

app.Run();
