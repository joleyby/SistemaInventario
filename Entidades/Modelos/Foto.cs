namespace Entidades.Modelos;
public class Foto : EntidadBase
{
    public int CategoriaId { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string Enlace { get; set; } = string.Empty;
    public long Tamaño { get; set; }
    public byte[] Contenido { get; set; } = Array.Empty<byte>();
    public string Url { get; set; } = string.Empty;
    public override string ToString() => NombreArchivo;
}




