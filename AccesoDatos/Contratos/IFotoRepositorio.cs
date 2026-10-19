using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface IFotoRepositorio : IRepositorio<Foto>
{
    IReadOnlyList<Foto> ObtenerPorCategoria(int categoriaId);
}




