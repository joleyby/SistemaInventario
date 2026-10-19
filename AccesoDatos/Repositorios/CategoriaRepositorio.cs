using System.Data;
using AccesoDatos.Contratos;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public sealed class CategoriaRepositorio : RepositorioBase<Categoria>, ICategoriaRepositorio
{
    protected override string Tabla => "Categoria";
    protected override string ConsultaBase =>
        "SELECT t.Id, t.Nombre, t.Descripcion, t.Activo, t.FechaCreacion, t.FechaModificacion FROM Categoria t";
    protected override string[] ColumnasEscritura => new[] { "Nombre", "Descripcion" };
    protected override Categoria CrearEntidad(IDataRecord registro) => new()
    {
        Nombre = (string)registro["Nombre"],
        Descripcion = (string)registro["Descripcion"]
    };
    protected override void AsignarParametros(SqlCommand comando, Categoria entidad)
    {
        Agregar(comando, "Nombre", entidad.Nombre);
        Agregar(comando, "Descripcion", entidad.Descripcion);
    }
    public bool ExisteNombre(string nombre, int? excluirId = null)
    {
        const string sql = "SELECT COUNT(1) FROM Categoria WHERE Nombre = @Nombre AND (@ExcluirId IS NULL OR Id <> @ExcluirId)";
        return Escalar(sql, cmd =>
        {
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@ExcluirId", (object?)excluirId ?? DBNull.Value);
        }) > 0;
    }
}




