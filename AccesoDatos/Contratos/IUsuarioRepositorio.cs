using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface IUsuarioRepositorio : IRepositorio<Usuario>
{
    Usuario? ObtenerPorNombreUsuario(string nombreUsuario);
    int Contar();
    bool ExisteNombreUsuario(string nombre, int? excluirId = null);
    int ContarAdministradoresActivos();
}




