namespace Entidades.Modelos;
public class Suplidor : EntidadBase
{
    public string NombreEmpresa { get; set; } = string.Empty;
    public string NombreContacto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? SitioWeb { get; set; }
    public override string ToString() => NombreEmpresa;
}




