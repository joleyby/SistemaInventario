using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface IBitacoraRepositorio
{
    void Insertar(RegistroBitacora registro);
    IReadOnlyList<RegistroBitacora> Listar(int limite);
}




