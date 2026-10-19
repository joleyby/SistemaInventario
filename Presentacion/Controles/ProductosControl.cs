using System.Globalization;
using Entidades.Modelos;
using LogicaNegocio.Filtros;
using LogicaNegocio.Servicios.Contratos;
using Presentacion.Formularios;

namespace Presentacion.Controles;
public sealed class ProductosControl : ControlCrudBase<Producto>
{
    private readonly IProductoServicio _productos;
    private readonly ICategoriaServicio _categorias;
    private readonly ISuplidorServicio _suplidores;

    private readonly ComboBox _cboCategoria = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 190 };
    private readonly ComboBox _cboSuplidor = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 190 };
    private readonly TextBox _txtPrecioMinimo = new() { Width = 90, PlaceholderText = "Mín." };
    private readonly TextBox _txtPrecioMaximo = new() { Width = 90, PlaceholderText = "Máx." };
    private bool _cargandoCombos;
    public ProductosControl(IProductoServicio productos, ICategoriaServicio categorias, ISuplidorServicio suplidores)
    {
        _productos = productos;
        _categorias = categorias;
        _suplidores = suplidores;

        var btnLimpiar = new Button { Text = "Limpiar filtros", AutoSize = true };
        btnLimpiar.Click += (_, _) =>
        {
            _txtPrecioMinimo.Clear();
            _txtPrecioMaximo.Clear();
            _cboCategoria.SelectedIndex = 0;
            _cboSuplidor.SelectedIndex = 0;
        };

        PanelFiltros.Controls.Add(Etiqueta("Categoría:"));
        PanelFiltros.Controls.Add(_cboCategoria);
        PanelFiltros.Controls.Add(Etiqueta("Suplidor:"));
        PanelFiltros.Controls.Add(_cboSuplidor);
        PanelFiltros.Controls.Add(Etiqueta("Precio:"));
        PanelFiltros.Controls.Add(_txtPrecioMinimo);
        PanelFiltros.Controls.Add(Etiqueta("a"));
        PanelFiltros.Controls.Add(_txtPrecioMaximo);
        PanelFiltros.Controls.Add(btnLimpiar);

        _cboCategoria.SelectedIndexChanged += (_, _) => { if (!_cargandoCombos) Refrescar(); };
        _cboSuplidor.SelectedIndexChanged += (_, _) => { if (!_cargandoCombos) Refrescar(); };
        _txtPrecioMinimo.TextChanged += (_, _) => Refrescar();
        _txtPrecioMaximo.TextChanged += (_, _) => Refrescar();
    }
    protected override IReadOnlyList<Producto> Cargar(string? texto) => _productos.Buscar(new ProductoFiltro
    {
        Texto = texto,
        CategoriaId = IdSeleccionado(_cboCategoria),
        SuplidorId = IdSeleccionado(_cboSuplidor),
        PrecioMinimo = ParsePrecio(_txtPrecioMinimo.Text),
        PrecioMaximo = ParsePrecio(_txtPrecioMaximo.Text)
    });
    public override void Refrescar()
    {
        CargarCombos();
        base.Refrescar();
    }
    protected override ColumnaGrid[] Columnas => new[]
    {
        new ColumnaGrid(nameof(Producto.Nombre), "Producto"),
        new ColumnaGrid(nameof(Producto.Precio), "Precio", "C2"),
        new ColumnaGrid(nameof(Producto.NombreCategoria), "Categoría"),
        new ColumnaGrid(nameof(Producto.NombreSuplidor), "Suplidor"),
        new ColumnaGrid(nameof(Producto.FechaModificacion), "Modificado", "dd/MM/yyyy HH:mm")
    };
    protected override bool MostrarFormulario(Producto? entidad)
    {
        var completo = entidad is null ? null : _productos.ObtenerPorId(entidad.Id);
        using var formulario = new ProductoForm(_productos, _categorias.Listar(), _suplidores.Listar(), completo);
        return formulario.ShowDialog(this) == DialogResult.OK;
    }
    protected override void EliminarPorId(int id) => _productos.Eliminar(id);

    private void CargarCombos()
    {
        _cargandoCombos = true;
        try
        {
            var catSeleccionada = _cboCategoria.SelectedValue is int idCat ? idCat : 0;
            var supSeleccionado = _cboSuplidor.SelectedValue is int idSup ? idSup : 0;

            var categorias = new List<Categoria> { new() { Id = 0, Nombre = "(Todas)" } };
            categorias.AddRange(_categorias.Listar());
            _cboCategoria.DataSource = categorias;
            _cboCategoria.DisplayMember = nameof(Categoria.Nombre);
            _cboCategoria.ValueMember = nameof(Categoria.Id);
            _cboCategoria.SelectedIndex = Math.Max(0, categorias.FindIndex(c => c.Id == catSeleccionada));

            var suplidores = new List<Suplidor> { new() { Id = 0, NombreEmpresa = "(Todos)" } };
            suplidores.AddRange(_suplidores.Listar());
            _cboSuplidor.DataSource = suplidores;
            _cboSuplidor.DisplayMember = nameof(Suplidor.NombreEmpresa);
            _cboSuplidor.ValueMember = nameof(Suplidor.Id);
            _cboSuplidor.SelectedIndex = Math.Max(0, suplidores.FindIndex(s => s.Id == supSeleccionado));
        }
        finally
        {
            _cargandoCombos = false;
        }
    }

    private static int? IdSeleccionado(ComboBox combo) =>
        combo.SelectedValue is int id && id > 0 ? (int?)id : null;

    private static decimal? ParsePrecio(string texto) =>
        decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out var valor) ? (decimal?)valor : null;

    private static Label Etiqueta(string texto) => new() { Text = texto, AutoSize = true, Padding = new Padding(8, 8, 0, 0) };
}




