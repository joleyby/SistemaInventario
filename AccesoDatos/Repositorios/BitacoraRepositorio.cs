using AccesoDatos.Conexion;
using AccesoDatos.Contratos;
using Entidades.Comun;
using Entidades.Excepciones;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;

namespace AccesoDatos.Repositorios;
public sealed class BitacoraRepositorio : IBitacoraRepositorio
{
    public void Insertar(RegistroBitacora registro)
    {
        const string sql = "INSERT INTO Bitacora (UsuarioId, UsuarioNombre, Accion, Entidad, EntidadId, Detalle) " +
                           "VALUES (@UsuarioId, @UsuarioNombre, @Accion, @Entidad, @EntidadId, @Detalle)";
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@UsuarioId", (object?)registro.UsuarioId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UsuarioNombre", registro.UsuarioNombre);
            cmd.Parameters.AddWithValue("@Accion", registro.Accion);
            cmd.Parameters.AddWithValue("@Entidad", registro.Entidad);
            cmd.Parameters.AddWithValue("@EntidadId", (object?)registro.EntidadId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Detalle", registro.Detalle);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Logger.RegistrarError("Bitácora: insertar", ex);
            throw new AccesoDatosException("No se pudo registrar el evento en la bitácora.", ex);
        }
    }
    public IReadOnlyList<RegistroBitacora> Listar(int limite)
    {
        const string sql = "SELECT TOP (@Limite) Id, FechaHora, UsuarioId, UsuarioNombre, Accion, Entidad, EntidadId, Detalle " +
                           "FROM Bitacora ORDER BY FechaHora DESC, Id DESC";
        var lista = new List<RegistroBitacora>();
        try
        {
            using var cn = ConexionSingleton.Instancia.CrearConexion();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Limite", limite);
            cn.Open();
            using var lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(new RegistroBitacora
                {
                    Id = lector.GetInt32(0),
                    FechaHora = lector.GetDateTime(1),
                    UsuarioId = lector.IsDBNull(2) ? (int?)null : lector.GetInt32(2),
                    UsuarioNombre = lector.GetString(3),
                    Accion = lector.GetString(4),
                    Entidad = lector.GetString(5),
                    EntidadId = lector.IsDBNull(6) ? (int?)null : lector.GetInt32(6),
                    Detalle = lector.GetString(7)
                });
            }
        }
        catch (SqlException ex)
        {
            Logger.RegistrarError("Bitácora: listar", ex);
            throw new AccesoDatosException("No se pudo leer la bitácora.", ex);
        }
        return lista;
    }
}




