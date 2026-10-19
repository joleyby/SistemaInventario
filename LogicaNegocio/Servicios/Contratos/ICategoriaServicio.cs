using Entidades.Modelos;

namespace LogicaNegocio.Servicios.Contratos;
public interface ICategoriaServicio
{
    IReadOnlyList<Categoria> Listar(string? texto = null);
    IReadOnlyList<Categoria> ListarEliminadas();
    Categoria? ObtenerPorId(int id);
    void Guardar(Categoria categoria);
    void Eliminar(int id);
    void Restaurar(int id);
}




