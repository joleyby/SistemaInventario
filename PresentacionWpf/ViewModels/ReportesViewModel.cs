using System.Globalization;
using LogicaNegocio.Reportes;
using LogicaNegocio.Servicios.Contratos;
using PresentacionWpf.MVVM;
using System.Windows.Input;

namespace PresentacionWpf.ViewModels;
public sealed class ReportesViewModel : ViewModelBase, IModulo
{
    private readonly IReporteServicio _reportes;
    private IReadOnlyList<ResumenCategoria> _productosPorCategoria = Array.Empty<ResumenCategoria>();
    private IReadOnlyList<TopSuplidor> _top5Suplidores = Array.Empty<TopSuplidor>();
    private IReadOnlyList<PromedioPrecioCategoria> _promedioPrecios = Array.Empty<PromedioPrecioCategoria>();
    private decimal _valorTotal;
    public ReportesViewModel(IReporteServicio reportes)
    {
        _reportes = reportes;
        ActualizarCommand = new RelayCommand(Refrescar);
        Refrescar();
    }
    public IReadOnlyList<ResumenCategoria> ProductosPorCategoria
    {
        get => _productosPorCategoria;
        private set => SetProperty(ref _productosPorCategoria, value);
    }
    public IReadOnlyList<TopSuplidor> Top5Suplidores
    {
        get => _top5Suplidores;
        private set => SetProperty(ref _top5Suplidores, value);
    }
    public IReadOnlyList<PromedioPrecioCategoria> PromedioPrecios
    {
        get => _promedioPrecios;
        private set => SetProperty(ref _promedioPrecios, value);
    }
    public decimal ValorTotal
    {
        get => _valorTotal;
        private set
        {
            if (SetProperty(ref _valorTotal, value))
            {
                OnPropertyChanged(nameof(ValorTotalTexto));
            }
        }
    }
    public string ValorTotalTexto =>
        $"Valor total del inventario activo: {ValorTotal.ToString("C2", CultureInfo.CurrentCulture)}";
    public ICommand ActualizarCommand { get; }
    public void Refrescar()
    {
        ProductosPorCategoria = _reportes.ProductosPorCategoria();
        Top5Suplidores = _reportes.Top5Suplidores();
        PromedioPrecios = _reportes.PromedioPreciosPorCategoria();
        ValorTotal = _reportes.ValorTotalInventario();
    }
}




