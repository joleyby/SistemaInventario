using System.Globalization;
using System.Reflection;
using System.Text;

namespace LogicaNegocio.Genericos;
public static class ExportadorCsv
{
    public static void Exportar<T>(IEnumerable<T> datos, string ruta)
    {
        var propiedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToArray();

        var sb = new StringBuilder();
        sb.AppendLine(string.Join(";", propiedades.Select(p => Escapar(p.Name))));

        foreach (var fila in datos)
        {
            var valores = propiedades.Select(p => Escapar(Convert.ToString(p.GetValue(fila), CultureInfo.CurrentCulture)));
            sb.AppendLine(string.Join(";", valores));
        }

        File.WriteAllText(ruta, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    }

    private static string Escapar(string? valor)
    {
        var texto = valor ?? string.Empty;
        return texto.IndexOfAny(new[] { ';', '"', '\n', '\r' }) >= 0
            ? $"\"{texto.Replace("\"", "\"\"")}\""
            : texto;
    }
}




