using AccesoDatos.Contratos;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Filtros;
using LogicaNegocio.Genericos;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;

namespace LogicaNegocio.Servicios;
public sealed class ProductoServicio : IProductoServicio
{
    private readonly IProductoRepositorio _productos;
    private readonly ICategoriaRepositorio _categorias;
    private readonly ISuplidorRepositorio _suplidores;
    private readonly IBitacoraServicio _bitacora;
    public ProductoServicio(IProductoRepositorio productos, ICategoriaRepositorio categorias,
                            ISuplidorRepositorio suplidores, IBitacoraServicio bitacora)
    {
        _productos = productos;
        _categorias = categorias;
        _suplidores = suplidores;
        _bitacora = bitacora;
    }
    public IReadOnlyList<Producto> Buscar(ProductoFiltro filtro)
    {
        IEnumerable<Producto> consulta = _productos.Obtener();

        if (filtro.CategoriaId is int categoriaId)
        {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId);
        }
        if (filtro.SuplidorId is int suplidorId)
        {
            consulta = consulta.Where(p => p.SuplidorId == suplidorId);
        }
        if (filtro.PrecioMinimo is decimal precioMinimo)
        {
            consulta = consulta.Where(p => p.Precio >= precioMinimo);
        }
        if (filtro.PrecioMaximo is decimal precioMaximo)
        {
            consulta = consulta.Where(p => p.Precio <= precioMaximo);
        }

        return Buscador.Filtrar(consulta, filtro.Texto, p => p.Nombre, p => p.NombreCategoria, p => p.NombreSuplidor)
            .OrderBy(p => p.NombreCategoria)
            .ThenBy(p => p.Nombre)
            .ToList();
    }
    public IReadOnlyList<Producto> ListarEliminados() => _productos.ObtenerEliminados();
    public Producto? ObtenerPorId(int id) => _productos.ObtenerPorId(id);
    public void Guardar(Producto producto)
    {
        var errores = Validar(producto);
        if (errores.Count > 0)
        {
            throw new ValidacionException(errores);
        }

        var esNuevo = producto.Id == 0;
        if (esNuevo)
        {
            _productos.Insertar(producto);
        }
        else
        {
            _productos.Actualizar(producto);
        }
        _bitacora.Registrar(esNuevo ? "CREAR" : "MODIFICAR", "Producto", producto.Id,
            $"{producto.Nombre} · {producto.Precio.ToString("0.00")}");
    }
    public void Eliminar(int id)
    {
        var nombre = _productos.ObtenerPorId(id)?.Nombre ?? id.ToString();
        _productos.EliminarLogico(id);
        _bitacora.Registrar("ELIMINAR", "Producto", id, nombre);
    }
    public void Restaurar(int id)
    {
        var producto = _productos.ObtenerPorId(id) ?? throw new ValidacionException("El producto no existe.");

        var categoria = _categorias.ObtenerPorId(producto.CategoriaId);
        if (categoria is null || !categoria.Activo)
        {
            throw new ValidacionException("No se puede restaurar: la categoría asociada está eliminada.");
        }

        var suplidor = _suplidores.ObtenerPorId(producto.SuplidorId);
        if (suplidor is null || !suplidor.Activo)
        {
            throw new ValidacionException("No se puede restaurar: el suplidor asociado está eliminado.");
        }

        _productos.Restaurar(id);
        _bitacora.Registrar("RESTAURAR", "Producto", id, producto.Nombre);
    }

    private Dictionary<string, string> Validar(Producto producto)
    {
        var errores = new Dictionary<string, string>();

        if (!Validador.LongitudValida(producto.Nombre, 2, 150))
        {
            errores["Nombre"] = "El nombre es obligatorio (2 a 150 caracteres).";
        }
        if (!Validador.EsPrecioValido(producto.Precio))
        {
            errores["Precio"] = "El precio debe ser mayor a 0 y tener máximo 2 decimales.";
        }

        var categoria = _categorias.ObtenerPorId(producto.CategoriaId);
        if (categoria is null || !categoria.Activo)
        {
            errores["CategoriaId"] = "Seleccione una categoría activa.";
        }

        var suplidor = _suplidores.ObtenerPorId(producto.SuplidorId);
        if (suplidor is null || !suplidor.Activo)
        {
            errores["SuplidorId"] = "Seleccione un suplidor activo.";
        }

        return errores;
    }
}




