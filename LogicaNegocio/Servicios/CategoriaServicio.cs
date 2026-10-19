using AccesoDatos.Contratos;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Genericos;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;

namespace LogicaNegocio.Servicios;
public sealed class CategoriaServicio : ICategoriaServicio
{
    private readonly ICategoriaRepositorio _categorias;
    private readonly IFotoRepositorio _fotos;
    private readonly IProductoRepositorio _productos;
    private readonly IAlmacenFotos _almacen;
    private readonly IBitacoraServicio _bitacora;
    public CategoriaServicio(ICategoriaRepositorio categorias, IFotoRepositorio fotos,
                             IProductoRepositorio productos, IAlmacenFotos almacen, IBitacoraServicio bitacora)
    {
        _categorias = categorias;
        _fotos = fotos;
        _productos = productos;
        _almacen = almacen;
        _bitacora = bitacora;
    }
    public IReadOnlyList<Categoria> Listar(string? texto = null) =>
        Buscador.Filtrar(_categorias.Obtener(), texto, c => c.Nombre, c => c.Descripcion)
            .OrderBy(c => c.Nombre)
            .ToList();
    public IReadOnlyList<Categoria> ListarEliminadas() => _categorias.ObtenerEliminados();
    public Categoria? ObtenerPorId(int id)
    {
        var categoria = _categorias.ObtenerPorId(id);
        if (categoria is null)
        {
            return null;
        }

        foreach (var foto in _fotos.ObtenerPorCategoria(id))
        {
            foto.Url = _almacen.ObtenerUrl(foto.Enlace);
            categoria.Fotos.Add(foto);
        }
        return categoria;
    }
    public void Guardar(Categoria categoria)
    {
        var errores = Validar(categoria);
        if (errores.Count > 0)
        {
            throw new ValidacionException(errores);
        }

        var esNueva = categoria.Id == 0;
        if (esNueva)
        {
            _categorias.Insertar(categoria);
        }
        else
        {
            _categorias.Actualizar(categoria);
        }

        var (agregadas, quitadas) = SincronizarFotos(categoria);
        var detalle = categoria.Nombre + (agregadas + quitadas > 0 ? $" · fotos +{agregadas} -{quitadas}" : string.Empty);
        _bitacora.Registrar(esNueva ? "CREAR" : "MODIFICAR", "Categoria", categoria.Id, detalle);
    }
    public void Eliminar(int id)
    {
        var productosActivos = _productos.ContarActivosPorCategoria(id);
        if (productosActivos > 0)
        {
            throw new ValidacionException(
                $"No se puede eliminar: la categoría tiene {productosActivos} producto(s) activo(s).");
        }

        var nombre = _categorias.ObtenerPorId(id)?.Nombre ?? id.ToString();
        _categorias.EliminarLogico(id);
        _bitacora.Registrar("ELIMINAR", "Categoria", id, nombre);
    }
    public void Restaurar(int id)
    {
        _categorias.Restaurar(id);
        _bitacora.Registrar("RESTAURAR", "Categoria", id, _categorias.ObtenerPorId(id)?.Nombre ?? string.Empty);
    }
    private (int agregadas, int quitadas) SincronizarFotos(Categoria categoria)
    {
        var quitadas = 0;
        foreach (var existente in _fotos.ObtenerPorCategoria(categoria.Id)
                     .Where(f => categoria.Fotos.All(n => n.Id != f.Id)))
        {
            _fotos.EliminarLogico(existente.Id);
            quitadas++;
        }

        var agregadas = 0;
        foreach (var nueva in categoria.Fotos.Where(f => f.Id == 0))
        {
            var enlace = _almacen.Guardar(categoria.Id, nueva.Contenido, nueva.Extension);
            nueva.CategoriaId = categoria.Id;
            nueva.Enlace = enlace;
            nueva.Tamaño = nueva.Contenido.LongLength;

            try
            {
                _fotos.Insertar(nueva);
            }
            catch (AccesoDatosException)
            {
                _almacen.Eliminar(enlace);
                throw;
            }

            nueva.Url = _almacen.ObtenerUrl(enlace);
            nueva.Contenido = Array.Empty<byte>();
            agregadas++;
        }

        return (agregadas, quitadas);
    }

    private Dictionary<string, string> Validar(Categoria categoria)
    {
        var errores = new Dictionary<string, string>();

        if (!Validador.LongitudValida(categoria.Nombre, 3, 100))
        {
            errores["Nombre"] = "El nombre es obligatorio y debe tener entre 3 y 100 caracteres.";
        }
        else if (_categorias.ExisteNombre(categoria.Nombre, categoria.Id == 0 ? (int?)null : categoria.Id))
        {
            errores["Nombre"] = "Ya existe una categoría con ese nombre.";
        }

        if (!Validador.LongitudValida(categoria.Descripcion, 1, 500))
        {
            errores["Descripcion"] = "La descripción es obligatoria (máximo 500 caracteres).";
        }

        if (categoria.Fotos.Count > Validador.MaximoFotosPorCategoria)
        {
            errores["Fotos"] = $"Una categoría admite máximo {Validador.MaximoFotosPorCategoria} fotos.";
        }

        foreach (var foto in categoria.Fotos.Where(f => f.Id == 0))
        {
            if (!Validador.ExtensionFotoValida(foto.Extension))
            {
                errores["Fotos"] = "Solo se permiten imágenes .jpg, .jpeg o .png.";
            }
            else if (!Validador.TamañoFotoValido(foto.Contenido.LongLength))
            {
                errores["Fotos"] = "Cada foto no puede superar 2 MB.";
            }
        }

        return errores;
    }
}




