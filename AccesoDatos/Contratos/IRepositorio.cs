using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface IRepositorio<T> where T : EntidadBase
{
    IReadOnlyList<T> Obtener();
    IReadOnlyList<T> ObtenerEliminados();
    T? ObtenerPorId(int id);
    int Insertar(T entidad);
    void Actualizar(T entidad);
    void EliminarLogico(int id);
    void Restaurar(int id);
}




