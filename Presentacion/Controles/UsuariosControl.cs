using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;
using Presentacion.Formularios;

namespace Presentacion.Controles;
public sealed class UsuariosControl : ControlCrudBase<Usuario>
{
    private readonly IUsuarioServicio _servicio;
    public UsuariosControl(IUsuarioServicio servicio)
    {
        _servicio = servicio;
    }
    protected override IReadOnlyList<Usuario> Cargar(string? texto) => _servicio.Listar(texto);
    protected override ColumnaGrid[] Columnas => new[]
    {
        new ColumnaGrid(nameof(Usuario.NombreUsuario), "Usuario"),
        new ColumnaGrid(nameof(Usuario.NombreCompleto), "Nombre"),
        new ColumnaGrid(nameof(Usuario.Rol), "Rol"),
        new ColumnaGrid(nameof(Usuario.EstadoTexto), "Estado"),
        new ColumnaGrid(nameof(Usuario.FechaCreacion), "Creado", "dd/MM/yyyy")
    };
    protected override bool MostrarFormulario(Usuario? entidad)
    {
        var completo = entidad is null ? null : _servicio.ObtenerPorId(entidad.Id);
        using var formulario = new RegistroUsuarioForm(_servicio, completo);
        return formulario.ShowDialog(this) == DialogResult.OK;
    }
    protected override void EliminarPorId(int id) => _servicio.Eliminar(id);
}




