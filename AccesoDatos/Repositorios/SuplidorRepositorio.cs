using System.Data;
using AccesoDatos.Contratos;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public sealed class SuplidorRepositorio : RepositorioBase<Suplidor>, ISuplidorRepositorio
{
    protected override string Tabla => "Suplidor";
    protected override string ConsultaBase =>
        "SELECT t.Id, t.NombreEmpresa, t.NombreContacto, t.Telefono, t.Email, t.SitioWeb, t.Activo, t.FechaCreacion, t.FechaModificacion FROM Suplidor t";
    protected override string[] ColumnasEscritura => new[] { "NombreEmpresa", "NombreContacto", "Telefono", "Email", "SitioWeb" };
    protected override Suplidor CrearEntidad(IDataRecord registro) => new()
    {
        NombreEmpresa = (string)registro["NombreEmpresa"],
        NombreContacto = (string)registro["NombreContacto"],
        Telefono = (string)registro["Telefono"],
        Email = (string)registro["Email"],
        SitioWeb = registro["SitioWeb"] as string
    };
    protected override void AsignarParametros(SqlCommand comando, Suplidor entidad)
    {
        Agregar(comando, "NombreEmpresa", entidad.NombreEmpresa);
        Agregar(comando, "NombreContacto", entidad.NombreContacto);
        Agregar(comando, "Telefono", entidad.Telefono);
        Agregar(comando, "Email", entidad.Email);
        Agregar(comando, "SitioWeb", entidad.SitioWeb);
    }
    public bool ExisteNombreEmpresa(string nombre, int? excluirId = null)
    {
        const string sql = "SELECT COUNT(1) FROM Suplidor WHERE NombreEmpresa = @Nombre AND (@ExcluirId IS NULL OR Id <> @ExcluirId)";
        return Escalar(sql, cmd =>
        {
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@ExcluirId", (object?)excluirId ?? DBNull.Value);
        }) > 0;
    }
}




