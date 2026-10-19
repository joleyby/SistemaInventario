using Presentacion.Diseño;

namespace Presentacion.Formularios;
public sealed partial class SuplidorForm
{
    private TextBox _txtEmpresa = null!;
    private TextBox _txtContacto = null!;
    private TextBox _txtTelefono = null!;
    private TextBox _txtEmail = null!;
    private TextBox _txtSitioWeb = null!;
    private void DisenarFormulario()
    {
        _txtEmpresa = new TextBox { Width = 340, MaxLength = 150, PlaceholderText = "Ej: Distribuidora Caribe SRL" };
        _txtContacto = new TextBox { Width = 340, MaxLength = 100, PlaceholderText = "Nombre y apellido" };
        _txtTelefono = new TextBox { Width = 340, PlaceholderText = "809-XXX-XXXX" };
        _txtEmail = new TextBox { Width = 340, MaxLength = 150, PlaceholderText = "correo@empresa.com" };
        _txtSitioWeb = new TextBox { Width = 340, PlaceholderText = "https://..." };

        AgregarSeccion("Datos de la empresa");
        AgregarCampo("NombreEmpresa", "Nombre de la empresa *", _txtEmpresa);
        AgregarCampo("Telefono", "Teléfono *", _txtTelefono);
        AgregarCampo("Email", "Correo electrónico *", _txtEmail);
        AgregarCampo("SitioWeb", "Sitio web", _txtSitioWeb);

        AgregarSeccion("Persona de contacto");
        AgregarCampo("NombreContacto", "Nombre del contacto *", _txtContacto);
    }
}




