using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface IProductoRepositorio : IRepositorio<Producto>
{
    int ContarActivosPorCategoria(int categoriaId);
    int ContarActivosPorSuplidor(int suplidorId);
}




