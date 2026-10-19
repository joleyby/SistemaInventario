using System.Globalization;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;

namespace Presentacion.Formularios;
public sealed partial class ProductoForm : FormularioBase
{
    private readonly IProductoServicio _servicio;
    private readonly Producto _producto;
    public ProductoForm(IProductoServicio servicio, IReadOnlyList<Categoria> categorias,
                        IReadOnlyList<Suplidor> suplidores, Producto? producto)
        : base(producto is null ? "Nuevo producto" : "Editar producto")
    {
        _servicio = servicio;
        _producto = producto ?? new Producto();

        DisenarFormulario();
        CargarCombos(categorias, suplidores);
        CargarDatos();
    }
    protected override void GuardarDatos()
    {
        if (!decimal.TryParse(_txtPrecio.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var precio))
        {
            throw new ValidacionException(new Dictionary<string, string>
            {
                ["Precio"] = "El precio debe ser un número válido."
            });
        }

        _producto.Nombre = _txtNombre.Text.Trim();
        _producto.Precio = precio;
        _producto.CategoriaId = _cboCategoria.SelectedValue is int idCategoria ? idCategoria : 0;
        _producto.SuplidorId = _cboSuplidor.SelectedValue is int idSuplidor ? idSuplidor : 0;
        _servicio.Guardar(_producto);
    }

    private void CargarCombos(IReadOnlyList<Categoria> categorias, IReadOnlyList<Suplidor> suplidores)
    {
        var listaCategorias = categorias.ToList();
        _cboCategoria.DataSource = listaCategorias;
        _cboCategoria.DisplayMember = nameof(Categoria.Nombre);
        _cboCategoria.ValueMember = nameof(Categoria.Id);
        _cboCategoria.SelectedIndex = listaCategorias.FindIndex(c => c.Id == _producto.CategoriaId);

        var listaSuplidores = suplidores.ToList();
        _cboSuplidor.DataSource = listaSuplidores;
        _cboSuplidor.DisplayMember = nameof(Suplidor.NombreEmpresa);
        _cboSuplidor.ValueMember = nameof(Suplidor.Id);
        _cboSuplidor.SelectedIndex = listaSuplidores.FindIndex(s => s.Id == _producto.SuplidorId);
    }

    private void CargarDatos()
    {
        _txtNombre.Text = _producto.Nombre;
        _txtPrecio.Text = _producto.Id == 0
            ? string.Empty
            : _producto.Precio.ToString("0.00", CultureInfo.CurrentCulture);
    }
}




