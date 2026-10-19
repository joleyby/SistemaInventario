namespace Entidades.Modelos;
public class RegistroBitacora
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int? UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public int? EntidadId { get; set; }
    public string Detalle { get; set; } = string.Empty;
}




