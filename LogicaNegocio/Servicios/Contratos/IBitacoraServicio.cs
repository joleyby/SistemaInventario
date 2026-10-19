using Entidades.Modelos;

namespace LogicaNegocio.Servicios.Contratos;
public interface IBitacoraServicio
{
    void Registrar(string accion, string entidad, int? entidadId, string detalle);
    void RegistrarAnonimo(string nombreUsuario, string accion, string detalle);
    IReadOnlyList<RegistroBitacora> Listar(int limite = 500);
}




