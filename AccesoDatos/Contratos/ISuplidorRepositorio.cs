using Entidades.Modelos;

namespace AccesoDatos.Contratos;
public interface ISuplidorRepositorio : IRepositorio<Suplidor>
{
    bool ExisteNombreEmpresa(string nombre, int? excluirId = null);
}




