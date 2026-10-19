using AccesoDatos.Contratos;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Genericos;
using LogicaNegocio.Seguridad;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;

namespace LogicaNegocio.Servicios;
public sealed class UsuarioServicio : IUsuarioServicio
{
    private readonly IUsuarioRepositorio _usuarios;
    private readonly ISesionUsuario _sesion;
    private readonly IBitacoraServicio _bitacora;
    public UsuarioServicio(IUsuarioRepositorio usuarios, ISesionUsuario sesion, IBitacoraServicio bitacora)
    {
        _usuarios = usuarios;
        _sesion = sesion;
        _bitacora = bitacora;
    }
    public IReadOnlyList<Usuario> Listar(string? texto = null)
    {
        ExigirAdministrador();
        var todos = _usuarios.Obtener().Concat(_usuarios.ObtenerEliminados());
        return Buscador.Filtrar(todos, texto, u => u.NombreUsuario, u => u.NombreCompleto, u => u.Rol)
            .OrderBy(u => u.NombreUsuario)
            .ToList();
    }
    public Usuario? ObtenerPorId(int id)
    {
        ExigirAdministrador();
        return _usuarios.ObtenerPorId(id);
    }
    public void Guardar(Usuario usuario, string? Contraseña)
    {
        ExigirAdministrador();
        var errores = Validar(usuario, Contraseña);
        if (errores.Count > 0)
        {
            throw new ValidacionException(errores);
        }

        if (usuario.Id == 0)
        {
            Crear(usuario, Contraseña!);
        }
        else
        {
            Modificar(usuario, Contraseña);
        }
    }
    public void Eliminar(int id)
    {
        ExigirAdministrador();
        var usuario = _usuarios.ObtenerPorId(id) ?? throw new ValidacionException("El usuario no existe.");

        if (usuario.Id == _sesion.UsuarioActual?.Id)
        {
            throw new ValidacionException("No puede desactivar su propia cuenta.");
        }
        if (EsAdministradorActivo(usuario) && _usuarios.ContarAdministradoresActivos() <= 1)
        {
            throw new ValidacionException("Debe quedar al menos un administrador activo.");
        }

        _usuarios.EliminarLogico(id);
        _bitacora.Registrar("DESACTIVAR", "Usuario", id, usuario.NombreUsuario);
    }
    public void Restaurar(int id)
    {
        ExigirAdministrador();
        _usuarios.Restaurar(id);
        _bitacora.Registrar("ACTIVAR", "Usuario", id, _usuarios.ObtenerPorId(id)?.NombreUsuario ?? string.Empty);
    }

    private void Crear(Usuario usuario, string Contraseña)
    {
        usuario.PasswordHash = HashContraseña.Crear(Contraseña);
        _usuarios.Insertar(usuario);
        _bitacora.Registrar("CREAR", "Usuario", usuario.Id, $"{usuario.NombreUsuario} ({usuario.Rol})");
    }

    private void Modificar(Usuario usuario, string? Contraseña)
    {
        var original = _usuarios.ObtenerPorId(usuario.Id) ?? throw new ValidacionException("El usuario no existe.");
        var esPropia = usuario.Id == _sesion.UsuarioActual?.Id;
        var cambioRol = original.Rol != usuario.Rol;
        var cambioEstado = original.Activo != usuario.Activo;

        if (esPropia && cambioRol)
        {
            throw new ValidacionException("No puede cambiar su propio rol.");
        }
        if (esPropia && cambioEstado && !usuario.Activo)
        {
            throw new ValidacionException("No puede desactivar su propia cuenta.");
        }
        if (EsAdministradorActivo(original) && !(usuario.Rol == RolesUsuario.Administrador && usuario.Activo)
            && _usuarios.ContarAdministradoresActivos() <= 1)
        {
            throw new ValidacionException("Debe quedar al menos un administrador activo.");
        }

        usuario.PasswordHash = string.IsNullOrEmpty(Contraseña)
            ? original.PasswordHash
            : HashContraseña.Crear(Contraseña);
        _usuarios.Actualizar(usuario);

        if (cambioEstado)
        {
            if (usuario.Activo)
            {
                _usuarios.Restaurar(usuario.Id);
            }
            else
            {
                _usuarios.EliminarLogico(usuario.Id);
            }
        }
        if (cambioRol)
        {
            _bitacora.Registrar("CAMBIO_ROL", "Usuario", usuario.Id,
                $"{usuario.NombreUsuario}: {original.Rol} → {usuario.Rol}");
        }
        if (cambioEstado)
        {
            _bitacora.Registrar(usuario.Activo ? "ACTIVAR" : "DESACTIVAR", "Usuario", usuario.Id, usuario.NombreUsuario);
        }
        if (!string.IsNullOrEmpty(Contraseña))
        {
            _bitacora.Registrar("RESTABLECER_Contraseña", "Usuario", usuario.Id, usuario.NombreUsuario);
        }
        if (original.NombreUsuario != usuario.NombreUsuario || original.NombreCompleto != usuario.NombreCompleto)
        {
            _bitacora.Registrar("MODIFICAR", "Usuario", usuario.Id,
                $"{original.NombreUsuario} → {usuario.NombreUsuario}; nombre: {usuario.NombreCompleto}");
        }
    }

    private Dictionary<string, string> Validar(Usuario usuario, string? Contraseña)
    {
        var errores = new Dictionary<string, string>();

        if (!Validador.EsNombreUsuarioValido(usuario.NombreUsuario))
        {
            errores["NombreUsuario"] = "Use de 3 a 50 caracteres: letras, números, punto, guion o guion bajo.";
        }
        else if (_usuarios.ExisteNombreUsuario(usuario.NombreUsuario, usuario.Id == 0 ? (int?)null : usuario.Id))
        {
            errores["NombreUsuario"] = "Ya existe un usuario con ese nombre.";
        }

        if (!Validador.LongitudValida(usuario.NombreCompleto, 3, 150))
        {
            errores["NombreCompleto"] = "El nombre es obligatorio (3 a 150 caracteres).";
        }

        if (!RolesUsuario.Todos.Contains(usuario.Rol))
        {
            errores["Rol"] = "Seleccione un rol válido.";
        }

        if (usuario.Id == 0 && string.IsNullOrEmpty(Contraseña))
        {
            errores["Contraseña"] = "La contraseña es obligatoria.";
        }
        else if (!string.IsNullOrEmpty(Contraseña) && !Validador.EsContraseñaSegura(Contraseña))
        {
            errores["Contraseña"] = "Mínimo 8 caracteres, con letras y números.";
        }

        return errores;
    }

    private static bool EsAdministradorActivo(Usuario usuario) =>
        usuario.Rol == RolesUsuario.Administrador && usuario.Activo;

    private void ExigirAdministrador()
    {
        if (!_sesion.EsAdministrador)
        {
            throw new ValidacionException("Solo los administradores pueden gestionar usuarios.");
        }
    }
}




