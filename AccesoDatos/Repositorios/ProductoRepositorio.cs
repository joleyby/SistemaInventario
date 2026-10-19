using System.Data;
using AccesoDatos.Contratos;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public sealed class ProductoRepositorio : RepositorioBase<Producto>, IProductoRepositorio
{
    protected override string Tabla => "Producto";
    protected override string ConsultaBase =>
        "SELECT t.Id, t.Nombre, t.Precio, t.CategoriaId, t.SuplidorId, t.Activo, t.FechaCreacion, t.FechaModificacion, " +
        "c.Nombre AS NombreCategoria, s.NombreEmpresa AS NombreSuplidor " +
        "FROM Producto t " +
        "INNER JOIN Categoria c ON c.Id = t.CategoriaId " +
        "INNER JOIN Suplidor s ON s.Id = t.SuplidorId";
    protected override string[] ColumnasEscritura => new[] { "Nombre", "Precio", "CategoriaId", "SuplidorId" };
    protected override Producto CrearEntidad(IDataRecord registro) => new()
    {
        Nombre = (string)registro["Nombre"],
        Precio = (decimal)registro["Precio"],
        CategoriaId = (int)registro["CategoriaId"],
        SuplidorId = (int)registro["SuplidorId"],
        NombreCategoria = (string)registro["NombreCategoria"],
        NombreSuplidor = (string)registro["NombreSuplidor"]
    };
    protected override void AsignarParametros(SqlCommand comando, Producto entidad)
    {
        Agregar(comando, "Nombre", entidad.Nombre);
        Agregar(comando, "Precio", entidad.Precio);
        Agregar(comando, "CategoriaId", entidad.CategoriaId);
        Agregar(comando, "SuplidorId", entidad.SuplidorId);
    }
    public int ContarActivosPorCategoria(int categoriaId) =>
        Escalar("SELECT COUNT(1) FROM Producto WHERE CategoriaId = @Id AND Activo = 1",
            cmd => cmd.Parameters.AddWithValue("@Id", categoriaId));
    public int ContarActivosPorSuplidor(int suplidorId) =>
        Escalar("SELECT COUNT(1) FROM Producto WHERE SuplidorId = @Id AND Activo = 1",
            cmd => cmd.Parameters.AddWithValue("@Id", suplidorId));
}




