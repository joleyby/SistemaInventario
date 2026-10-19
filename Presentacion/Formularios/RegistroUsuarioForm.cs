using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;

namespace Presentacion.Formularios;
public sealed partial class RegistroUsuarioForm : FormularioBase
{
    private readonly IUsuarioServicio _servicio;
    private readonly Usuario _usuario;
    private readonly bool _esNuevo;
    public RegistroUsuarioForm(IUsuarioServicio servicio, Usuario? usuario)
        : base(usuario is null ? "Registrar usuario" : "Editar usuario")
    {
        _servicio = servicio;
        _esNuevo = usuario is null;
        _usuario = usuario ?? new Usuario { Rol = RolesUsuario.Operador, Activo = true };

        DisenarFormulario(_esNuevo);
        CargarDatos();
    }
    protected override void GuardarDatos()
    {
        if (_txtContraseña.Text != _txtConfirmar.Text)
        {
            throw new ValidacionException(new Dictionary<string, string>
            {
                ["ConfirmarContraseña"] = "Las contraseñas no coinciden."
            });
        }

        _usuario.NombreUsuario = _txtUsuario.Text.Trim();
        _usuario.NombreCompleto = _txtNombre.Text.Trim();
        _usuario.Rol = _cboRol.SelectedItem as string ?? string.Empty;
        _usuario.Activo = _chkActiva.Checked;

        var Contraseña = string.IsNullOrEmpty(_txtContraseña.Text) ? null : _txtContraseña.Text;
        _servicio.Guardar(_usuario, Contraseña);
    }

    private void CargarDatos()
    {
        _txtUsuario.Text = _usuario.NombreUsuario;
        _txtNombre.Text = _usuario.NombreCompleto;
        _cboRol.SelectedItem = RolesUsuario.Todos.Contains(_usuario.Rol) ? _usuario.Rol : RolesUsuario.Operador;
        _chkActiva.Checked = _usuario.Activo;
    }
}




