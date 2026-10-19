using Presentacion.Diseño;

namespace Presentacion.Formularios;
public sealed partial class ProductoForm
{
    private TextBox _txtNombre = null!;
    private TextBox _txtPrecio = null!;
    private ComboBox _cboCategoria = null!;
    private ComboBox _cboSuplidor = null!;
    private void DisenarFormulario()
    {
        _txtNombre = new TextBox { Width = 340, MaxLength = 150, PlaceholderText = "Ej: Audífonos Bluetooth" };
        _txtPrecio = new TextBox
        {
            Width = 340,
            PlaceholderText = "0.00",
            TextAlign = HorizontalAlignment.Right
        };
        _cboCategoria = new ComboBox { Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
        _cboSuplidor = new ComboBox { Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

        AgregarSeccion("Información del producto");
        AgregarCampo("Nombre", "Nombre *", _txtNombre);
        AgregarCampo("Precio", "Precio unitario (RD$) *", _txtPrecio);

        AgregarSeccion("Clasificación");
        AgregarCampo("CategoriaId", "Categoría *", _cboCategoria);
        AgregarCampo("SuplidorId", "Suplidor *", _cboSuplidor);
    }
}




