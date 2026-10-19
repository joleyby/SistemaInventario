using AccesoDatos.Contratos;
using Entidades.Comun;
using Entidades.Excepciones;

namespace AccesoDatos.Almacenamiento;
public sealed class AlmacenFotosArchivo : IAlmacenFotos
{
    private const string VariableEntorno = "SISTEMA_INVENTARIO_FOTOS";
    private readonly string _raiz;
    public AlmacenFotosArchivo()
    {
        var configurada = Environment.GetEnvironmentVariable(VariableEntorno);
        _raiz = Path.GetFullPath(string.IsNullOrWhiteSpace(configurada)
            ? Path.Combine(AppContext.BaseDirectory, "Fotos")
            : configurada);
    }
    public string Guardar(int categoriaId, byte[] contenido, string extension)
    {
        var enlace = $"categorias/{categoriaId}/{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var ruta = RutaAbsoluta(enlace);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
            File.WriteAllBytes(ruta, contenido);
            return enlace;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Logger.RegistrarError("Guardar foto en disco", ex);
            throw new AccesoDatosException("No se pudo guardar la foto en el disco. Revise errores.log.", ex);
        }
    }
    public void Eliminar(string enlace)
    {
        try
        {
            var ruta = RutaAbsoluta(enlace);
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Logger.RegistrarError("Eliminar foto en disco", ex);
        }
    }
    public string ObtenerUrl(string enlace) => new Uri(RutaAbsoluta(enlace)).AbsoluteUri;
    private string RutaAbsoluta(string enlace)
    {
        var ruta = Path.GetFullPath(Path.Combine(_raiz, enlace.Replace('/', Path.DirectorySeparatorChar)));
        if (!ruta.StartsWith(_raiz, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("El enlace de la foto apunta fuera del almacén.", nameof(enlace));
        }
        return ruta;
    }
}




