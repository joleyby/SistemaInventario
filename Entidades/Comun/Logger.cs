namespace Entidades.Comun;
public static class Logger
{
    private static readonly object _bloqueo = new();
    private static readonly string _ruta = Path.Combine(AppContext.BaseDirectory, "errores.log");
    public static string RutaArchivo => _ruta;
    public static void RegistrarError(string contexto, Exception ex)
    {
        var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {contexto} | {ex.GetType().Name}: {ex.Message}{Environment.NewLine}" +
                    $"{ex.StackTrace}{Environment.NewLine}{new string('-', 80)}{Environment.NewLine}";
        try
        {
            lock (_bloqueo)
            {
                File.AppendAllText(_ruta, linea);
            }
        }
        catch (IOException)
        {
        }
    }
}




