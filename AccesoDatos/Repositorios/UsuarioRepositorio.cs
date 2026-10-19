using System.Data;
using AccesoDatos.Contratos;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public sealed class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
{
    protected override string Tabla => "Usuario";
    protected override string ConsultaBase =>
        "SELECT t.Id, t.NombreUsuario, t.NombreCompleto, t.PasswordHash, t.Rol, t.Activo, t.FechaCreacion, t.FechaModificacion FROM Usuario t";
    protected override string[] ColumnasEscritura => new[] { "NombreUsuario", "NombreCompleto", "PasswordHash", "Rol" };
    protected override Usuario CrearEntidad(IDataRecord registro) => new()
    {
        NombreUsuario = (string)registro["NombreUsuario"],
        NombreCompleto = (string)registro["NombreCompleto"],
        PasswordHash = (string)registro["PasswordHash"],
        Rol = (string)registro["Rol"]
    };
    protected override void AsignarParametros(SqlCommand comando, Usuario entidad)
    {
        Agregar(comando, "NombreUsuario", entidad.NombreUsuario);
        Agregar(comando, "NombreCompleto", entidad.NombreCompleto);
        Agregar(comando, "PasswordHash", entidad.PasswordHash);
        Agregar(comando, "Rol", entidad.Rol);
    }
    public Usuario? ObtenerPorNombreUsuario(string nombreUsuario) =>
        Consultar(ConsultaBase + " WHERE t.NombreUsuario = @NombreUsuario AND t.Activo = 1",
            cmd => cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario)).FirstOrDefault();
    public int Contar() => Escalar("SELECT COUNT(1) FROM Usuario");
    public bool ExisteNombreUsuario(string nombre, int? excluirId = null)
    {
        const string sql = "SELECT COUNT(1) FROM Usuario WHERE NombreUsuario = @Nombre AND (@ExcluirId IS NULL OR Id <> @ExcluirId)";
        return Escalar(sql, cmd =>
        {
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@ExcluirId", (object?)excluirId ?? DBNull.Value);
        }) > 0;
    }
    public int ContarAdministradoresActivos() =>
        Escalar("SELECT COUNT(1) FROM Usuario WHERE Rol = N'Administrador' AND Activo = 1");
}




