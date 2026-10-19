using Microsoft.Data.SqlClient;

namespace AccesoDatos.Conexion;
public sealed class ConexionSingleton
{
    private const string CadenaPorDefecto =
        "Server=localhost;Database=SistemaInventario;Trusted_Connection=True;TrustServerCertificate=True;";

    private static readonly Lazy<ConexionSingleton> _instancia = new(() => new ConexionSingleton());

    private readonly string _cadenaConexion;

    private ConexionSingleton()
    {
        _cadenaConexion = Environment.GetEnvironmentVariable("SISTEMA_INVENTARIO_CN") ?? CadenaPorDefecto;
    }
    public static ConexionSingleton Instancia => _instancia.Value;
    public SqlConnection CrearConexion() => new(_cadenaConexion);
}




