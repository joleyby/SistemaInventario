using Entidades.Modelos;
using LogicaNegocio.Filtros;

namespace LogicaNegocio.Servicios.Contratos;
public interface IProductoServicio
{
    IReadOnlyList<Producto> Buscar(ProductoFiltro filtro);
    IReadOnlyList<Producto> ListarEliminados();
    Producto? ObtenerPorId(int id);
    void Guardar(Producto producto);
    void Eliminar(int id);
    void Restaurar(int id);
}




