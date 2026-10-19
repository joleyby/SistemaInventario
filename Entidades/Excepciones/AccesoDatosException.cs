namespace Entidades.Excepciones;
public class AccesoDatosException : Exception
{
    public AccesoDatosException(string mensaje, Exception? innerException)
        : base(mensaje, innerException)
    {
    }
}




