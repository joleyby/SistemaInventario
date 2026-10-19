using LogicaNegocio.Reportes;

namespace LogicaNegocio.Servicios.Contratos;
public interface IReporteServicio
{
    IReadOnlyList<ResumenCategoria> ProductosPorCategoria();
    IReadOnlyList<TopSuplidor> Top5Suplidores();
    IReadOnlyList<PromedioPrecioCategoria> PromedioPreciosPorCategoria();
    decimal ValorTotalInventario();
}




