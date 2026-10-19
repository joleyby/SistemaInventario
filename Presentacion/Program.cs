using Entidades.Comun;
using Entidades.Modelos;
using LogicaNegocio.Fabrica;
using Presentacion.Formularios;

namespace Presentacion;
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            var fabrica = new FabricaServicios();
            fabrica.Autenticacion.CrearUsuarioInicialSiNoExiste();

            while (true)
            {
                var usuario = MostrarLogin(fabrica);
                if (usuario is null)
                {
                    break;
                }

                using var principal = new MainForm(fabrica.Categorias, fabrica.Suplidores, fabrica.Productos,
                    fabrica.Reportes, fabrica.Usuarios, fabrica.Bitacora, fabrica.Sesion, usuario);
                Application.Run(principal);

                fabrica.Autenticacion.CerrarSesion();
                if (!principal.CerrarSesionSolicitado)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.RegistrarError("Inicio de la aplicación", ex);
            MessageBox.Show("No se pudo iniciar la aplicación. Revise errores.log.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static Usuario? MostrarLogin(FabricaServicios fabrica)
    {
        using var login = new LoginForm(fabrica.Autenticacion);
        return login.ShowDialog() == DialogResult.OK ? login.UsuarioAutenticado : null;
    }
}




