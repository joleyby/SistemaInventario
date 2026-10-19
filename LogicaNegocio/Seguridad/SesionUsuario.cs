using Entidades.Modelos;

namespace LogicaNegocio.Seguridad;
public sealed class SesionUsuario : ISesionUsuario
{
    public Usuario? UsuarioActual { get; private set; }
    public bool EsAdministrador => UsuarioActual?.Rol == RolesUsuario.Administrador;
    public void Iniciar(Usuario usuario) => UsuarioActual = usuario;
    public void Cerrar() => UsuarioActual = null;
}




