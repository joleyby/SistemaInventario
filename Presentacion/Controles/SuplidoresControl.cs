using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;
using Presentacion.Formularios;

namespace Presentacion.Controles;
public sealed class SuplidoresControl : ControlCrudBase<Suplidor>
{
    private readonly ISuplidorServicio _servicio;
    public SuplidoresControl(ISuplidorServicio servicio)
    {
        _servicio = servicio;
    }
    protected override IReadOnlyList<Suplidor> Cargar(string? texto) => _servicio.Listar(texto);
    protected override ColumnaGrid[] Columnas => new[]
    {
        new ColumnaGrid(nameof(Suplidor.NombreEmpresa), "Empresa"),
        new ColumnaGrid(nameof(Suplidor.NombreContacto), "Contacto"),
        new ColumnaGrid(nameof(Suplidor.Telefono), "Teléfono"),
        new ColumnaGrid(nameof(Suplidor.Email), "Correo"),
        new ColumnaGrid(nameof(Suplidor.SitioWeb), "Sitio web"),
        new ColumnaGrid(nameof(Suplidor.FechaModificacion), "Modificado", "dd/MM/yyyy HH:mm")
    };
    protected override bool MostrarFormulario(Suplidor? entidad)
    {
        var completo = entidad is null ? null : _servicio.ObtenerPorId(entidad.Id);
        using var formulario = new SuplidorForm(_servicio, completo);
        return formulario.ShowDialog(this) == DialogResult.OK;
    }
    protected override void EliminarPorId(int id) => _servicio.Eliminar(id);
}




