using System.Collections.ObjectModel;
using Entidades.Modelos;
using LogicaNegocio.Filtros;
using LogicaNegocio.Servicios.Contratos;
using PresentacionWpf.MVVM;
using PresentacionWpf.Servicios;

namespace PresentacionWpf.ViewModels;
public sealed class ProductosViewModel : CrudViewModelBase<Producto>
{
    private readonly IProductoServicio _productos;
    private readonly ICategoriaServicio _categorias;
    private readonly ISuplidorServicio _suplidores;
    private Producto _actual = new();
    private bool _recargandoListas;

    private string _nombre = string.Empty;
    private decimal _precio;
    private int? _categoriaId;
    private int? _suplidorId;

    private int? _filtroCategoriaId;
    private int? _filtroSuplidorId;
    private decimal? _precioMinimo;
    private decimal? _precioMaximo;
    public ProductosViewModel(IProductoServicio productos, ICategoriaServicio categorias,
                              ISuplidorServicio suplidores, IDialogService dialogo) : base(dialogo)
    {
        _productos = productos;
        _categorias = categorias;
        _suplidores = suplidores;
        RecargarListas();
        Refrescar();
    }
    public ObservableCollection<Categoria> CategoriasActivas { get; } = new();
    public ObservableCollection<Suplidor> SuplidoresActivos { get; } = new();
    public ObservableCollection<Categoria> CategoriasFiltro { get; } = new();
    public ObservableCollection<Suplidor> SuplidoresFiltro { get; } = new();
    public int? FiltroCategoriaId
    {
        get => _filtroCategoriaId;
        set { if (SetProperty(ref _filtroCategoriaId, value) && !_recargandoListas) Refrescar(); }
    }
    public int? FiltroSuplidorId
    {
        get => _filtroSuplidorId;
        set { if (SetProperty(ref _filtroSuplidorId, value) && !_recargandoListas) Refrescar(); }
    }
    public decimal? PrecioMinimo
    {
        get => _precioMinimo;
        set { if (SetProperty(ref _precioMinimo, value)) Refrescar(); }
    }
    public decimal? PrecioMaximo
    {
        get => _precioMaximo;
        set { if (SetProperty(ref _precioMaximo, value)) Refrescar(); }
    }
    public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }
    public decimal Precio { get => _precio; set => SetProperty(ref _precio, value); }
    public int? CategoriaId { get => _categoriaId; set => SetProperty(ref _categoriaId, value); }
    public int? SuplidorId { get => _suplidorId; set => SetProperty(ref _suplidorId, value); }
    public void RecargarListas()
    {
        _recargandoListas = true;
        try
        {
            var filtroCat = FiltroCategoriaId;
            var filtroSup = FiltroSuplidorId;
            var formCat = CategoriaId;
            var formSup = SuplidorId;

            var activas = _categorias.Listar();
            CategoriasActivas.Clear();
            foreach (var c in activas) CategoriasActivas.Add(c);
            CategoriasFiltro.Clear();
            CategoriasFiltro.Add(new Categoria { Id = 0, Nombre = "(Todas)" });
            foreach (var c in activas) CategoriasFiltro.Add(c);

            var suplidores = _suplidores.Listar();
            SuplidoresActivos.Clear();
            foreach (var s in suplidores) SuplidoresActivos.Add(s);
            SuplidoresFiltro.Clear();
            SuplidoresFiltro.Add(new Suplidor { Id = 0, NombreEmpresa = "(Todos)" });
            foreach (var s in suplidores) SuplidoresFiltro.Add(s);

            FiltroCategoriaId = filtroCat;
            FiltroSuplidorId = filtroSup;
            CategoriaId = formCat;
            SuplidorId = formSup;
        }
        finally
        {
            _recargandoListas = false;
        }
    }
    protected override IReadOnlyList<Producto> Cargar(string? texto) => _productos.Buscar(new ProductoFiltro
    {
        Texto = texto,
        CategoriaId = IdFiltro(FiltroCategoriaId),
        SuplidorId = IdFiltro(FiltroSuplidorId),
        PrecioMinimo = PrecioMinimo,
        PrecioMaximo = PrecioMaximo
    });
    protected override IReadOnlyList<Producto> CargarEliminados() => _productos.ListarEliminados();
    protected override void CargarFormulario(Producto entidad)
    {
        _actual = _productos.ObtenerPorId(entidad.Id) ?? entidad;
        Nombre = _actual.Nombre;
        Precio = _actual.Precio;
        CategoriaId = _actual.CategoriaId;
        SuplidorId = _actual.SuplidorId;
    }
    protected override void LimpiarFormulario()
    {
        _actual = new Producto();
        Nombre = string.Empty;
        Precio = 0;
        CategoriaId = null;
        SuplidorId = null;
    }
    protected override int GuardarEntidad()
    {
        _actual.Nombre = Nombre;
        _actual.Precio = Precio;
        _actual.CategoriaId = CategoriaId ?? 0;
        _actual.SuplidorId = SuplidorId ?? 0;
        _productos.Guardar(_actual);
        return _actual.Id;
    }
    protected override void EliminarPorId(int id) => _productos.Eliminar(id);
    protected override void RestaurarPorId(int id) => _productos.Restaurar(id);

    private static int? IdFiltro(int? valor) => valor is > 0 ? valor : null;
}




