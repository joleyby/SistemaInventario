namespace PresentacionWpf.Servicios;
public interface IDialogService
{
    bool Confirmar(string mensaje);
    string[] SeleccionarImagenes();
    void AbrirEnlace(string url);
}




