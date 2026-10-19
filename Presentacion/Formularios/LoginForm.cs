using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;

namespace Presentacion.Formularios;
public sealed partial class LoginForm : Form
{
    private readonly IAutenticacionServicio _autenticacion;
    public LoginForm(IAutenticacionServicio autenticacion)
    {
        _autenticacion = autenticacion;
        DisenarFormulario();
    }
    public Usuario? UsuarioAutenticado { get; private set; }

    private void IntentarIngresar()
    {
        _lblError.Text = string.Empty;
        try
        {
            var usuario = _autenticacion.Autenticar(_txtUsuario.Text, _txtContraseña.Text);
            if (usuario is null)
            {
                _lblError.Text = "Usuario o contraseña incorrectos.";
                _txtContraseña.Clear();
                _txtContraseña.Focus();
                return;
            }

            UsuarioAutenticado = usuario;
            DialogResult = DialogResult.OK;
        }
        catch (ValidacionException ex)
        {
            _lblError.Text = ex.Message;
        }
        catch (AccesoDatosException ex)
        {
            _lblError.Text = ex.Message;
        }
    }
}




