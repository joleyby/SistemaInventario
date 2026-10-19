using Entidades.Modelos;

namespace LogicaNegocio.Seguridad;
public interface ISesionUsuario
{
    Usuario? UsuarioActual { get; }
    bool EsAdministrador { get; }
    void Iniciar(Usuario usuario);
    void Cerrar();
}




