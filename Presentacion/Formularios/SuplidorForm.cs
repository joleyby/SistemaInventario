using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;

namespace Presentacion.Formularios;
public sealed partial class SuplidorForm : FormularioBase
{
    private readonly ISuplidorServicio _servicio;
    private readonly Suplidor _suplidor;
    public SuplidorForm(ISuplidorServicio servicio, Suplidor? suplidor)
        : base(suplidor is null ? "Nuevo suplidor" : "Editar suplidor")
    {
        _servicio = servicio;
        _suplidor = suplidor ?? new Suplidor();

        DisenarFormulario();
        CargarDatos();
    }
    protected override void GuardarDatos()
    {
        _suplidor.NombreEmpresa = _txtEmpresa.Text;
        _suplidor.NombreContacto = _txtContacto.Text;
        _suplidor.Telefono = _txtTelefono.Text.Trim();
        _suplidor.Email = _txtEmail.Text.Trim();
        _suplidor.SitioWeb = string.IsNullOrWhiteSpace(_txtSitioWeb.Text) ? null : _txtSitioWeb.Text.Trim();
        _servicio.Guardar(_suplidor);
    }

    private void CargarDatos()
    {
        _txtEmpresa.Text = _suplidor.NombreEmpresa;
        _txtContacto.Text = _suplidor.NombreContacto;
        _txtTelefono.Text = _suplidor.Telefono;
        _txtEmail.Text = _suplidor.Email;
        _txtSitioWeb.Text = _suplidor.SitioWeb ?? string.Empty;
    }
}




