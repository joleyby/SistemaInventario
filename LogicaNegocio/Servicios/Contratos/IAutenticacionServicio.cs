using Entidades.Modelos;

namespace LogicaNegocio.Servicios.Contratos;
public interface IAutenticacionServicio
{
    Usuario? Autenticar(string nombreUsuario, string Contraseña);
    void CerrarSesion();
    void CrearUsuarioInicialSiNoExiste();
}




