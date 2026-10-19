using System.Security.Cryptography;

namespace LogicaNegocio.Seguridad;
public static class HashContraseña
{
    private const int Iteraciones = 210_000;
    private const int TamañoSal = 16;
    private const int TamañoHash = 32;
    public static string Crear(string Contraseña)
    {
        var sal = RandomNumberGenerator.GetBytes(TamañoSal);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Contraseña, sal, Iteraciones, HashAlgorithmName.SHA256, TamañoHash);
        return $"PBKDF2${Iteraciones}${Convert.ToBase64String(sal)}${Convert.ToBase64String(hash)}";
    }
    public static bool Verificar(string Contraseña, string almacenado)
    {
        var partes = almacenado.Split('$');
        if (partes.Length != 4 || partes[0] != "PBKDF2" || !int.TryParse(partes[1], out var iteraciones))
        {
            return false;
        }

        try
        {
            var sal = Convert.FromBase64String(partes[2]);
            var esperado = Convert.FromBase64String(partes[3]);
            var calculado = Rfc2898DeriveBytes.Pbkdf2(Contraseña, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);
            return CryptographicOperations.FixedTimeEquals(calculado, esperado);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}




