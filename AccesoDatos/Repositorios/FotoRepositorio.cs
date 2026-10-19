using System.Data;
using AccesoDatos.Contratos;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public sealed class FotoRepositorio : RepositorioBase<Foto>, IFotoRepositorio
{
    protected override string Tabla => "Foto";
    protected override string ConsultaBase =>
        "SELECT t.Id, t.CategoriaId, t.NombreArchivo, t.Extension, t.Enlace, t.Tamaño, t.Activo, t.FechaCreacion, t.FechaModificacion FROM Foto t";
    protected override string[] ColumnasEscritura => new[] { "CategoriaId", "NombreArchivo", "Extension", "Enlace", "Tamaño" };
    protected override Foto CrearEntidad(IDataRecord registro) => new()
    {
        CategoriaId = (int)registro["CategoriaId"],
        NombreArchivo = (string)registro["NombreArchivo"],
        Extension = (string)registro["Extension"],
        Enlace = (string)registro["Enlace"],
        Tamaño = (long)registro["Tamaño"]
    };
    protected override void AsignarParametros(SqlCommand comando, Foto entidad)
    {
        Agregar(comando, "CategoriaId", entidad.CategoriaId);
        Agregar(comando, "NombreArchivo", entidad.NombreArchivo);
        Agregar(comando, "Extension", entidad.Extension);
        Agregar(comando, "Enlace", entidad.Enlace);
        Agregar(comando, "Tamaño", entidad.Tamaño);
    }
    public IReadOnlyList<Foto> ObtenerPorCategoria(int categoriaId) =>
        Consultar(ConsultaBase + " WHERE t.CategoriaId = @CategoriaId AND t.Activo = 1 ORDER BY t.Id",
            cmd => cmd.Parameters.AddWithValue("@CategoriaId", categoriaId));
}




