using AccesoDatos.Contratos;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Seguridad;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;

namespace LogicaNegocio.Servicios;
public sealed class AutenticacionServicio : IAutenticacionServicio
{
    private const string UsuarioInicial = "admin";
    private const string ContraseñaInicial = "admin123";

    private readonly IUsuarioRepositorio _usuarios;
    private readonly ISesionUsuario _sesion;
    private readonly IBitacoraServicio _bitacora;
    public AutenticacionServicio(IUsuarioRepositorio usuarios, ISesionUsuario sesion, IBitacoraServicio bitacora)
    {
        _usuarios = usuarios;
        _sesion = sesion;
        _bitacora = bitacora;
    }
    public Usuario? Autenticar(string nombreUsuario, string Contraseña)
    {
        if (!Validador.LongitudValida(nombreUsuario, 3, 50) || string.IsNullOrEmpty(Contraseña))
        {
            throw new ValidacionException("Escriba su usuario y contraseña.");
        }

        var nombre = nombreUsuario.Trim();
        var usuario = _usuarios.ObtenerPorNombreUsuario(nombre);
        if (usuario is null || !HashContraseña.Verificar(Contraseña, usuario.PasswordHash))
        {
            _bitacora.RegistrarAnonimo(nombre, "LOGIN_FALLIDO", "Usuario o contraseña incorrectos.");
            return null;
        }

        _sesion.Iniciar(usuario);
        _bitacora.Registrar("INICIAR_SESION", "Usuario", usuario.Id, "Inicio de sesión correcto.");
        return usuario;
    }
    public void CerrarSesion()
    {
        var actual = _sesion.UsuarioActual;
        if (actual is null)
        {
            return;
        }

        _bitacora.Registrar("CERRAR_SESION", "Usuario", actual.Id, "Cierre de sesión.");
        _sesion.Cerrar();
    }
    public void CrearUsuarioInicialSiNoExiste()
    {
        if (_usuarios.Contar() > 0)
        {
            return;
        }

        _usuarios.Insertar(new Usuario
        {
            NombreUsuario = UsuarioInicial,
            NombreCompleto = "Administrador",
            Rol = RolesUsuario.Administrador,
            PasswordHash = HashContraseña.Crear(ContraseñaInicial)
        });
    }
}




