namespace LogicaNegocio.Genericos;
public static class Buscador
{
    public static IEnumerable<T> Filtrar<T>(IEnumerable<T> origen, string? texto, params Func<T, string?>[] campos)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return origen;
        }

        var termino = texto.Trim();
        return origen.Where(item =>
            campos.Any(campo => campo(item)?.Contains(termino, StringComparison.OrdinalIgnoreCase) == true));
    }
}




