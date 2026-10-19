using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface ICategoriaRepositorio : IRepositorio<Categoria>
{
    bool ExisteNombre(string nombre, int? excluirId = null);
}




