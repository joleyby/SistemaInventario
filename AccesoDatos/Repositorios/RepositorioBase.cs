using System.Data;
using AccesoDatos.Conexion;
using AccesoDatos.Contratos;
using Entidades.Comun;
using Entidades.Excepciones;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public abstract class RepositorioBase<T> : IRepositorio<T> where T : EntidadBase
{
    protected abstract string Tabla { get; }
    protected abstract string ConsultaBase { get; }
    protected abstract string[] ColumnasEscritura { get; }
    protected abstract T CrearEntidad(IDataRecord registro);
    protected abstract void AsignarParametros(SqlCommand comando, T entidad);
    public IReadOnlyList<T> Obtener() =>
        Consultar(ConsultaBase + " WHERE t.Activo = 1 ORDER BY t.Id");
    public IReadOnlyList<T> ObtenerEliminados() =>
        Consultar(ConsultaBase + " WHERE t.Activo = 0 ORDER BY t.Id");
    public T? ObtenerPorId(int id) =>
        Consultar(ConsultaBase + " WHERE t.Id = @Id", cmd => cmd.Parameters.AddWithValue("@Id", id)).FirstOrDefault();
    public int Insertar(T entidad)
    {
        var columnas = string.Join(", ", ColumnasEscritura);
        var valores = string.Join(", ", ColumnasEscritura.Select(c => "@" + c));
        var sql = $"INSERT INTO {Tabla} ({columnas}, Activo, FechaCreacion, FechaModificacion) " +
                  $"VALUES ({valores}, 1, GETDATE(), GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            AsignarParametros(cmd, entidad);
            cn.Open();
            var id = Convert.ToInt32(cmd.ExecuteScalar());
            entidad.Id = id;
            return id;
        }
        catch (SqlException ex)
        {
            throw CrearError("No se pudo insertar el registro", ex);
        }
    }
    public void Actualizar(T entidad)
    {
        var asignaciones = string.Join(", ", ColumnasEscritura.Select(c => $"{c} = @{c}"));
        var sql = $"UPDATE {Tabla} SET {asignaciones}, FechaModificacion = GETDATE() WHERE Id = @Id";
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            AsignarParametros(cmd, entidad);
            cmd.Parameters.AddWithValue("@Id", entidad.Id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            throw CrearError("No se pudo actualizar el registro", ex);
        }
    }
    public void EliminarLogico(int id) => CambiarEstado(id, false);
    public void Restaurar(int id) => CambiarEstado(id, true);

    private void CambiarEstado(int id, bool activo)
    {
        var sql = $"UPDATE {Tabla} SET Activo = @Activo, FechaModificacion = GETDATE() WHERE Id = @Id";
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Activo", activo);
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            throw CrearError("No se pudo cambiar el estado del registro", ex);
        }
    }
    protected IReadOnlyList<T> Consultar(string sql, Action<SqlCommand>? configurar = null)
    {
        var lista = new List<T>();
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            configurar?.Invoke(cmd);
            cn.Open();
            using var lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(Leer(lector));
            }
        }
        catch (SqlException ex)
        {
            throw CrearError("Error al consultar datos", ex);
        }
        return lista;
    }
    protected int Escalar(string sql, Action<SqlCommand>? configurar = null)
    {
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            configurar?.Invoke(cmd);
            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (SqlException ex)
        {
            throw CrearError("Error al ejecutar consulta", ex);
        }
    }
    protected static void Agregar(SqlCommand cmd, string nombre, object? valor) =>
        cmd.Parameters.AddWithValue("@" + nombre, valor ?? DBNull.Value);

    private T Leer(IDataRecord registro)
    {
        var entidad = CrearEntidad(registro);
        entidad.Id = Convert.ToInt32(registro["Id"]);
        entidad.Activo = Convert.ToBoolean(registro["Activo"]);
        entidad.FechaCreacion = (DateTime)registro["FechaCreacion"];
        entidad.FechaModificacion = (DateTime)registro["FechaModificacion"];
        return entidad;
    }

    private AccesoDatosException CrearError(string contexto, SqlException ex)
    {
        Logger.RegistrarError($"{Tabla}: {contexto}", ex);
        return new AccesoDatosException($"{contexto}. Revise errores.log para más detalles.", ex);
    }
}




