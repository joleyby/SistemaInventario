using AccesoDatos.Contratos;
using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Seguridad;
using LogicaNegocio.Servicios.Contratos;

namespace LogicaNegocio.Servicios;
public sealed class BitacoraServicio : IBitacoraServicio
{
    private const int LongitudMaximaUsuario = 50;
    private const int LongitudMaximaDetalle = 500;

    private readonly IBitacoraRepositorio _repositorio;
    private readonly ISesionUsuario _sesion;
    public BitacoraServicio(IBitacoraRepositorio repositorio, ISesionUsuario sesion)
    {
        _repositorio = repositorio;
        _sesion = sesion;
    }
    public void Registrar(string accion, string entidad, int? entidadId, string detalle)
    {
        var actual = _sesion.UsuarioActual;
        Guardar(new RegistroBitacora
        {
            FechaHora = DateTime.Now,
            UsuarioId = actual?.Id,
            UsuarioNombre = actual?.NombreUsuario ?? "sistema",
            Accion = accion,
            Entidad = entidad,
            EntidadId = entidadId,
            Detalle = detalle
        });
    }
    public void RegistrarAnonimo(string nombreUsuario, string accion, string detalle) =>
        Guardar(new RegistroBitacora
        {
            FechaHora = DateTime.Now,
            UsuarioId = null,
            UsuarioNombre = nombreUsuario,
            Accion = accion,
            Entidad = "Usuario",
            EntidadId = null,
            Detalle = detalle
        });
    public IReadOnlyList<RegistroBitacora> Listar(int limite = 500)
    {
        if (!_sesion.EsAdministrador)
        {
            throw new ValidacionException("Solo los administradores pueden ver la bitácora.");
        }
        return _repositorio.Listar(limite);
    }

    private void Guardar(RegistroBitacora registro)
    {
        registro.UsuarioNombre = Recortar(registro.UsuarioNombre, LongitudMaximaUsuario);
        registro.Detalle = Recortar(registro.Detalle, LongitudMaximaDetalle);
        _repositorio.Insertar(registro);
    }

    private static string Recortar(string texto, int maximo) =>
        texto.Length <= maximo ? texto : texto[..maximo];
}




