using System.Text.RegularExpressions;

namespace LogicaNegocio.Validaciones;
public static class Validador
{
    public const long TamañoMaximoFotoBytes = 2 * 1024 * 1024;
    public const int MaximoFotosPorCategoria = 5;

    private static readonly string[] ExtensionesFoto = { ".jpg", ".jpeg", ".png" };
    private static readonly Regex RegexEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private static readonly Regex RegexTelefono = new(@"^(809|829|849)-\d{3}-\d{4}$", RegexOptions.Compiled);
    private static readonly Regex RegexNombreUsuario = new(@"^[A-Za-z0-9._-]{3,50}$", RegexOptions.Compiled);
    public static bool EsEmailValido(string? valor) => valor is not null && RegexEmail.IsMatch(valor);
    public static bool EsTelefonoValido(string? valor) => valor is not null && RegexTelefono.IsMatch(valor);
    public static bool EsUrlValida(string? valor) =>
        Uri.TryCreate(valor, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    public static bool EsPrecioValido(decimal precio) => precio > 0 && decimal.Round(precio, 2) == precio;
    public static bool LongitudValida(string? valor, int minimo, int maximo)
    {
        var longitud = (valor ?? string.Empty).Trim().Length;
        return longitud >= minimo && longitud <= maximo;
    }
    public static bool ExtensionFotoValida(string? extension) =>
        ExtensionesFoto.Contains((extension ?? string.Empty).ToLowerInvariant());
    public static bool TamañoFotoValido(long bytes) => bytes > 0 && bytes <= TamañoMaximoFotoBytes;
    public static bool EsNombreUsuarioValido(string? valor) => valor is not null && RegexNombreUsuario.IsMatch(valor);
    public static bool EsContraseñaSegura(string? valor) =>
        valor is not null && valor.Length >= 8 && valor.Any(c => char.IsLetter(c)) && valor.Any(c => char.IsDigit(c));
}




