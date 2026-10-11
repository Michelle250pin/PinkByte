namespace PinkByte.Api.Domain.Comun;

// 🧬 HERENCIA aplicada a errores: nuestras excepciones "son" Exception,
// pero con un nombre que explica QUÉ salió mal en el juego.

/// <summary>Se lanza cuando la jugadora intenta algo que las reglas no permiten.</summary>
public class ReglaDelJuegoException(string mensaje) : Exception(mensaje);

/// <summary>Se lanza cuando se busca algo (personaje, combate) que no existe.</summary>
public class NoEncontradoException(string mensaje) : Exception(mensaje);
