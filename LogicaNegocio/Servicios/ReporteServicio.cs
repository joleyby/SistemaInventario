using AccesoDatos.Contratos;
using LogicaNegocio.Reportes;
using LogicaNegocio.Servicios.Contratos;

namespace LogicaNegocio.Servicios;
public sealed class ReporteServicio : IReporteServicio
{
    private readonly IProductoRepositorio _productos;
    public ReporteServicio(IProductoRepositorio productos)
    {
        _productos = productos;
    }
    public IReadOnlyList<ResumenCategoria> ProductosPorCategoria() =>
        _productos.Obtener()
            .GroupBy(p => new { p.CategoriaId, p.NombreCategoria })
            .Select(g => new ResumenCategoria(g.Key.NombreCategoria, g.Count()))
            .OrderByDescending(r => r.CantidadProductos)
            .ThenBy(r => r.Categoria)
            .ToList();
    public IReadOnlyList<TopSuplidor> Top5Suplidores() =>
        _productos.Obtener()
            .GroupBy(p => p.NombreSuplidor)
            .Select(g => new TopSuplidor(g.Key, g.Count()))
            .OrderByDescending(t => t.CantidadProductos)
            .ThenBy(t => t.Suplidor)
            .Take(5)
            .ToList();
    public IReadOnlyList<PromedioPrecioCategoria> PromedioPreciosPorCategoria() =>
        _productos.Obtener()
            .GroupBy(p => p.NombreCategoria)
            .Select(g => new PromedioPrecioCategoria(g.Key, Math.Round(g.Average(p => p.Precio), 2)))
            .OrderBy(x => x.Categoria)
            .ToList();
    public decimal ValorTotalInventario() => _productos.Obtener().Sum(p => p.Precio);
}




