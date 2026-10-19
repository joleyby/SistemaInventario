using LogicaNegocio.Servicios.Contratos;
using PresentacionWpf.MVVM;
using PresentacionWpf.Servicios;

namespace PresentacionWpf.ViewModels;
public sealed class MainViewModel : ViewModelBase
{
    private const int IndiceProductos = 2;
    private readonly IModulo[] _modulos;
    private int _pestanaActiva;
    public MainViewModel(ICategoriaServicio categorias, ISuplidorServicio suplidores,
                         IProductoServicio productos, IReporteServicio reportes, IDialogService dialogo)
    {
        Categorias = new CategoriasViewModel(categorias, dialogo);
        Suplidores = new SuplidoresViewModel(suplidores, dialogo);
        Productos = new ProductosViewModel(productos, categorias, suplidores, dialogo);
        Reportes = new ReportesViewModel(reportes);
        _modulos = new IModulo[] { Categorias, Suplidores, Productos, Reportes };
    }
    public CategoriasViewModel Categorias { get; }
    public SuplidoresViewModel Suplidores { get; }
    public ProductosViewModel Productos { get; }
    public ReportesViewModel Reportes { get; }
    public int PestanaActiva
    {
        get => _pestanaActiva;
        set
        {
            if (!SetProperty(ref _pestanaActiva, value) || value < 0 || value >= _modulos.Length)
            {
                return;
            }

            if (value == IndiceProductos)
            {
                Productos.RecargarListas();
            }
            _modulos[value].Refrescar();
        }
    }
}




