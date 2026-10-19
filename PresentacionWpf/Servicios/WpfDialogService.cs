using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;

namespace PresentacionWpf.Servicios;
public sealed class WpfDialogService : IDialogService
{
    public bool Confirmar(string mensaje) =>
        MessageBox.Show(mensaje, "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    public string[] SeleccionarImagenes()
    {
        var dialogo = new OpenFileDialog
        {
            Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            Multiselect = true,
            Title = "Seleccione fotos (máx. 2 MB cada una)"
        };
        return dialogo.ShowDialog() == true ? dialogo.FileNames : Array.Empty<string>();
    }
    public void AbrirEnlace(string url) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
}




