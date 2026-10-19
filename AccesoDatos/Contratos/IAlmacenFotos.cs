namespace AccesoDatos.Contratos;
public interface IAlmacenFotos
{
    string Guardar(int categoriaId, byte[] contenido, string extension);
    void Eliminar(string enlace);
    string ObtenerUrl(string enlace);
}




