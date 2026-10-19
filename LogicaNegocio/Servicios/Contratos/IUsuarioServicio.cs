using Entidades.Modelos;

namespace LogicaNegocio.Servicios.Contratos;
public interface IUsuarioServicio
{
    IReadOnlyList<Usuario> Listar(string? texto = null);
    Usuario? ObtenerPorId(int id);
    void Guardar(Usuario usuario, string? Contraseña);
    void Eliminar(int id);
    void Restaurar(int id);
}




