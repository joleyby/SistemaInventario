namespace Entidades.Modelos;
public class Producto : EntidadBase
{
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public int SuplidorId { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string NombreSuplidor { get; set; } = string.Empty;
    public override string ToString() => Nombre;
}




