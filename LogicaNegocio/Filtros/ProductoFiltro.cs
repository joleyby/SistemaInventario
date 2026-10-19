namespace LogicaNegocio.Filtros;
public sealed class ProductoFiltro
{
    public string? Texto { get; init; }
    public int? CategoriaId { get; init; }
    public int? SuplidorId { get; init; }
    public decimal? PrecioMinimo { get; init; }
    public decimal? PrecioMaximo { get; init; }
}




