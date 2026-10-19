using Entidades.Modelos;

namespace LogicaNegocio.Servicios.Contratos;
public interface ISuplidorServicio
{
    IReadOnlyList<Suplidor> Listar(string? texto = null);
    IReadOnlyList<Suplidor> ListarEliminados();
    Suplidor? ObtenerPorId(int id);
    void Guardar(Suplidor suplidor);
    void Eliminar(int id);
    void Restaurar(int id);
}




