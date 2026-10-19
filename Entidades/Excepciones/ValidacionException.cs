namespace Entidades.Excepciones;
public class ValidacionException : Exception
{
    public const string ClaveGeneral = "General";
    public IReadOnlyDictionary<string, string> Errores { get; }
    public ValidacionException(IDictionary<string, string> errores)
        : base(string.Join(Environment.NewLine, errores.Values))
    {
        Errores = new Dictionary<string, string>(errores);
    }
    public ValidacionException(string mensaje)
        : this(new Dictionary<string, string> { [ClaveGeneral] = mensaje })
    {
    }
}




