using AccesoDatos.Contratos;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Genericos;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;

namespace LogicaNegocio.Servicios;
public sealed class SuplidorServicio : ISuplidorServicio
{
    private readonly ISuplidorRepositorio _suplidores;
    private readonly IProductoRepositorio _productos;
    private readonly IBitacoraServicio _bitacora;
    public SuplidorServicio(ISuplidorRepositorio suplidores, IProductoRepositorio productos, IBitacoraServicio bitacora)
    {
        _suplidores = suplidores;
        _productos = productos;
        _bitacora = bitacora;
    }
    public IReadOnlyList<Suplidor> Listar(string? texto = null) =>
        Buscador.Filtrar(_suplidores.Obtener(), texto, s => s.NombreEmpresa, s => s.NombreContacto, s => s.Email)
            .OrderBy(s => s.NombreEmpresa)
            .ToList();
    public IReadOnlyList<Suplidor> ListarEliminados() => _suplidores.ObtenerEliminados();
    public Suplidor? ObtenerPorId(int id) => _suplidores.ObtenerPorId(id);
    public void Guardar(Suplidor suplidor)
    {
        var errores = Validar(suplidor);
        if (errores.Count > 0)
        {
            throw new ValidacionException(errores);
        }

        var esNuevo = suplidor.Id == 0;
        if (esNuevo)
        {
            _suplidores.Insertar(suplidor);
        }
        else
        {
            _suplidores.Actualizar(suplidor);
        }
        _bitacora.Registrar(esNuevo ? "CREAR" : "MODIFICAR", "Suplidor", suplidor.Id, suplidor.NombreEmpresa);
    }
    public void Eliminar(int id)
    {
        var productosActivos = _productos.ContarActivosPorSuplidor(id);
        if (productosActivos > 0)
        {
            throw new ValidacionException(
                $"No se puede eliminar: el suplidor tiene {productosActivos} producto(s) activo(s).");
        }

        var nombre = _suplidores.ObtenerPorId(id)?.NombreEmpresa ?? id.ToString();
        _suplidores.EliminarLogico(id);
        _bitacora.Registrar("ELIMINAR", "Suplidor", id, nombre);
    }
    public void Restaurar(int id)
    {
        _suplidores.Restaurar(id);
        _bitacora.Registrar("RESTAURAR", "Suplidor", id, _suplidores.ObtenerPorId(id)?.NombreEmpresa ?? string.Empty);
    }

    private Dictionary<string, string> Validar(Suplidor suplidor)
    {
        var errores = new Dictionary<string, string>();

        if (!Validador.LongitudValida(suplidor.NombreEmpresa, 2, 150))
        {
            errores["NombreEmpresa"] = "El nombre de la empresa es obligatorio (2 a 150 caracteres).";
        }
        else if (_suplidores.ExisteNombreEmpresa(suplidor.NombreEmpresa, suplidor.Id == 0 ? (int?)null : suplidor.Id))
        {
            errores["NombreEmpresa"] = "Ya existe un suplidor con ese nombre de empresa.";
        }

        if (!Validador.LongitudValida(suplidor.NombreContacto, 2, 100))
        {
            errores["NombreContacto"] = "El nombre del contacto es obligatorio (2 a 100 caracteres).";
        }

        if (!Validador.EsTelefonoValido(suplidor.Telefono))
        {
            errores["Telefono"] = "Use el formato dominicano 809/829/849-XXX-XXXX.";
        }

        if (!Validador.EsEmailValido(suplidor.Email))
        {
            errores["Email"] = "El correo electrónico no tiene un formato válido.";
        }

        if (!string.IsNullOrWhiteSpace(suplidor.SitioWeb) && !Validador.EsUrlValida(suplidor.SitioWeb))
        {
            errores["SitioWeb"] = "El sitio web debe ser una URL válida (http:// o https://).";
        }

        return errores;
    }
}




