using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;
using Presentacion.Formularios;

namespace Presentacion.Controles;
public sealed class CategoriasControl : ControlCrudBase<Categoria>
{
    private readonly ICategoriaServicio _servicio;
    public CategoriasControl(ICategoriaServicio servicio)
    {
        _servicio = servicio;
    }
    protected override IReadOnlyList<Categoria> Cargar(string? texto) => _servicio.Listar(texto);
    protected override ColumnaGrid[] Columnas => new[]
    {
        new ColumnaGrid(nameof(Categoria.Nombre), "Nombre"),
        new ColumnaGrid(nameof(Categoria.Descripcion), "Descripción"),
        new ColumnaGrid(nameof(Categoria.FechaCreacion), "Creada", "dd/MM/yyyy"),
        new ColumnaGrid(nameof(Categoria.FechaModificacion), "Modificada", "dd/MM/yyyy HH:mm")
    };
    protected override bool MostrarFormulario(Categoria? entidad)
    {
        var completa = entidad is null ? null : _servicio.ObtenerPorId(entidad.Id);
        using var formulario = new CategoriaForm(_servicio, completa);
        return formulario.ShowDialog(this) == DialogResult.OK;
    }
    protected override void EliminarPorId(int id) => _servicio.Eliminar(id);
}




