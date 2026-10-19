namespace Entidades.Modelos;
public class Usuario : EntidadBase
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string EstadoTexto => Activo ? "Activo" : "Inactivo";
    public override string ToString() => NombreCompleto;
}




