using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;
using PresentacionWpf.MVVM;
using PresentacionWpf.Servicios;

namespace PresentacionWpf.ViewModels;
public sealed class SuplidoresViewModel : CrudViewModelBase<Suplidor>
{
    private readonly ISuplidorServicio _servicio;
    private Suplidor _actual = new();
    private string _nombreEmpresa = string.Empty;
    private string _nombreContacto = string.Empty;
    private string _telefono = string.Empty;
    private string _email = string.Empty;
    private string? _sitioWeb;
    public SuplidoresViewModel(ISuplidorServicio servicio, IDialogService dialogo) : base(dialogo)
    {
        _servicio = servicio;
        Refrescar();
    }
    public string NombreEmpresa { get => _nombreEmpresa; set => SetProperty(ref _nombreEmpresa, value); }
    public string NombreContacto { get => _nombreContacto; set => SetProperty(ref _nombreContacto, value); }
    public string Telefono { get => _telefono; set => SetProperty(ref _telefono, value); }
    public string Email { get => _email; set => SetProperty(ref _email, value); }
    public string? SitioWeb { get => _sitioWeb; set => SetProperty(ref _sitioWeb, value); }
    protected override IReadOnlyList<Suplidor> Cargar(string? texto) => _servicio.Listar(texto);
    protected override IReadOnlyList<Suplidor> CargarEliminados() => _servicio.ListarEliminados();
    protected override void CargarFormulario(Suplidor entidad)
    {
        _actual = _servicio.ObtenerPorId(entidad.Id) ?? entidad;
        NombreEmpresa = _actual.NombreEmpresa;
        NombreContacto = _actual.NombreContacto;
        Telefono = _actual.Telefono;
        Email = _actual.Email;
        SitioWeb = _actual.SitioWeb;
    }
    protected override void LimpiarFormulario()
    {
        _actual = new Suplidor();
        NombreEmpresa = string.Empty;
        NombreContacto = string.Empty;
        Telefono = string.Empty;
        Email = string.Empty;
        SitioWeb = null;
    }
    protected override int GuardarEntidad()
    {
        _actual.NombreEmpresa = NombreEmpresa;
        _actual.NombreContacto = NombreContacto;
        _actual.Telefono = Telefono.Trim();
        _actual.Email = Email.Trim();
        _actual.SitioWeb = string.IsNullOrWhiteSpace(SitioWeb) ? null : SitioWeb.Trim();
        _servicio.Guardar(_actual);
        return _actual.Id;
    }
    protected override void EliminarPorId(int id) => _servicio.Eliminar(id);
    protected override void RestaurarPorId(int id) => _servicio.Restaurar(id);
}




