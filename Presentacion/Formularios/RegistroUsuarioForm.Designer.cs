using Entidades.Modelos;
using Presentacion.Diseño;

namespace Presentacion.Formularios;
public sealed partial class RegistroUsuarioForm
{
    private TextBox _txtUsuario = null!;
    private TextBox _txtNombre = null!;
    private ComboBox _cboRol = null!;
    private CheckBox _chkActiva = null!;
    private TextBox _txtContraseña = null!;
    private TextBox _txtConfirmar = null!;
    private void DisenarFormulario(bool esNuevo)
    {
        _txtUsuario = new TextBox { Width = 340, MaxLength = 50, PlaceholderText = "Ej: jperez" };
        _txtNombre = new TextBox { Width = 340, MaxLength = 150, PlaceholderText = "Nombre y apellido" };

        _cboRol = new ComboBox { Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
        _cboRol.Items.AddRange(RolesUsuario.Todos);

        _chkActiva = new CheckBox
        {
            Text = "Cuenta activa (puede iniciar sesión)",
            AutoSize = true,
            Checked = true,
            Font = Tema.FuenteNormal,
            ForeColor = Tema.TextoPrincipal,
            Margin = new Padding(0, 4, 0, 4)
        };

        _txtContraseña = new TextBox
        {
            Width = 340,
            MaxLength = 100,
            PasswordChar = '•',
            PlaceholderText = esNuevo ? "Mínimo 8 caracteres" : "Dejar vacío para no cambiarla"
        };
        _txtConfirmar = new TextBox { Width = 340, MaxLength = 100, PasswordChar = '•' };

        AgregarSeccion("Datos del usuario");
        AgregarCampo("NombreUsuario", "Usuario *", _txtUsuario);
        AgregarCampo("NombreCompleto", "Nombre completo *", _txtNombre);
        AgregarCampo("Rol", "Rol *", _cboRol);
        AgregarCampo("Activo", "Estado", _chkActiva);

        AgregarSeccion(esNuevo
            ? "Contraseña (mín. 8 caracteres, con letras y números)"
            : "Contraseña (opcional, déjela vacía para no cambiarla)");
        AgregarCampo("Contraseña", esNuevo ? "Contraseña *" : "Nueva contraseña", _txtContraseña);
        AgregarCampo("ConfirmarContraseña", "Confirmar contraseña", _txtConfirmar);
    }
}




