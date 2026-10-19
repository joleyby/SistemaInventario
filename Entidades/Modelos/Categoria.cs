namespace Entidades.Modelos;
public class Categoria : EntidadBase
{
    private string _nombre = string.Empty;
    private string _descripcion = string.Empty;
    public string Nombre
    {
        get => _nombre;
        set => _nombre = (value ?? string.Empty).Trim();
    }
    public string Descripcion
    {
        get => _descripcion;
        set => _descripcion = (value ?? string.Empty).Trim();
    }
    public List<Foto> Fotos { get; } = new();
    public override string ToString() => Nombre;
}




