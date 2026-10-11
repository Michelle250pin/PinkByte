using PinkByte.Api.Domain.Comun;

namespace PinkByte.Api.Domain.Villanos;

// 🏭 PATRÓN FACTORY: un solo lugar sabe cómo crear cada villano a partir de un texto.
// Si mañana agregas un villano nuevo, solo tocas este diccionario.
public static class FabricaDeVillanos
{
    // El orden del diccionario = el orden de la historia.
    private static readonly Dictionary<string, Func<Villano>> Creadores = new()
    {
        [nameof(BugFantasma)] = () => new BugFantasma(),
        [nameof(SpaghettiCode)] = () => new SpaghettiCode(),
        [nameof(LaDeadline)] = () => new LaDeadline(),
        [nameof(SindromeDelImpostor)] = () => new SindromeDelImpostor(),
    };

    public static Villano Crear(string tipo) =>
        Creadores.TryGetValue(tipo, out var crear)
            ? crear()
            : throw new NoEncontradoException($"No existe el villano '{tipo}'.");

    public static IEnumerable<Villano> Catalogo() => Creadores.Values.Select(crear => crear());
}
