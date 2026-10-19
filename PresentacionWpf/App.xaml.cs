using System.Windows;
using Entidades.Comun;
using LogicaNegocio.Fabrica;
using PresentacionWpf.Servicios;
using PresentacionWpf.ViewModels;

namespace PresentacionWpf;
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += (_, args) =>
        {
            Logger.RegistrarError("Excepción no controlada de la interfaz", args.Exception);
            MessageBox.Show("Ocurrió un error inesperado. Revise errores.log.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        try
        {
            var fabrica = new FabricaServicios();
            var main = new MainViewModel(fabrica.Categorias, fabrica.Suplidores, fabrica.Productos,
                fabrica.Reportes, new WpfDialogService());

            var ventana = new Views.MainWindow { DataContext = main };
            MainWindow = ventana;
            ventana.Show();
        }
        catch (Exception ex)
        {
            Logger.RegistrarError("Inicio de la aplicación WPF", ex);
            MessageBox.Show("No se pudo iniciar la aplicación. Revise errores.log.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }
}




