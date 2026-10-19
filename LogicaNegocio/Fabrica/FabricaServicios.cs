using AccesoDatos.Almacenamiento;
using AccesoDatos.Repositorios;
using LogicaNegocio.Seguridad;
using LogicaNegocio.Servicios;
using LogicaNegocio.Servicios.Contratos;

namespace LogicaNegocio.Fabrica;
public sealed class FabricaServicios
{
    public ISesionUsuario Sesion { get; }
    public IBitacoraServicio Bitacora { get; }
    public ICategoriaServicio Categorias { get; }
    public ISuplidorServicio Suplidores { get; }
    public IProductoServicio Productos { get; }
    public IReporteServicio Reportes { get; }
    public IUsuarioServicio Usuarios { get; }
    public IAutenticacionServicio Autenticacion { get; }
    public FabricaServicios()
    {
        var repoCategorias = new CategoriaRepositorio();
        var repoSuplidores = new SuplidorRepositorio();
        var repoProductos = new ProductoRepositorio();
        var repoFotos = new FotoRepositorio();
        var repoUsuarios = new UsuarioRepositorio();
        var almacenFotos = new AlmacenFotosArchivo();

        Sesion = new SesionUsuario();
        Bitacora = new BitacoraServicio(new BitacoraRepositorio(), Sesion);

        Categorias = new CategoriaServicio(repoCategorias, repoFotos, repoProductos, almacenFotos, Bitacora);
        Suplidores = new SuplidorServicio(repoSuplidores, repoProductos, Bitacora);
        Productos = new ProductoServicio(repoProductos, repoCategorias, repoSuplidores, Bitacora);
        Reportes = new ReporteServicio(repoProductos);
        Usuarios = new UsuarioServicio(repoUsuarios, Sesion, Bitacora);
        Autenticacion = new AutenticacionServicio(repoUsuarios, Sesion, Bitacora);
    }
}




