using Entidades.Excepciones;
using LogicaNegocio.Genericos;
using LogicaNegocio.Servicios.Contratos;

namespace Presentacion.Controles;
public sealed class ReportesControl : UserControl, IRefrescable
{
    private readonly IReporteServicio _reportes;
    private readonly ComboBox _cboReporte = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 300 };
    private readonly Label _lblValor = new() { AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Padding = new Padding(8, 6, 0, 0) };
    private readonly DataGridView _grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        RowHeadersVisible = false,
        BackgroundColor = Color.White
    };
    public ReportesControl(IReporteServicio reportes)
    {
        _reportes = reportes;
        Dock = DockStyle.Fill;

        _cboReporte.Items.AddRange(new object[]
        {
            "Productos por categoría",
            "Top 5 suplidores con más productos",
            "Promedio de precios por categoría"
        });
        _cboReporte.SelectedIndex = 0;
        _cboReporte.SelectedIndexChanged += (_, _) => Refrescar();

        var btnExportar = new Button { Text = "Exportar a CSV", AutoSize = true };
        btnExportar.Click += (_, _) => Exportar();

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8) };
        barra.Controls.Add(new Label { Text = "Reporte:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        barra.Controls.Add(_cboReporte);
        barra.Controls.Add(btnExportar);
        barra.Controls.Add(_lblValor);

        Controls.Add(_grilla);
        Controls.Add(barra);
        Load += (_, _) => Refrescar();
    }
    public void Refrescar()
    {
        try
        {
            object datos;
            if (_cboReporte.SelectedIndex == 0)
            {
                datos = _reportes.ProductosPorCategoria().ToList();
            }
            else if (_cboReporte.SelectedIndex == 1)
            {
                datos = _reportes.Top5Suplidores().ToList();
            }
            else
            {
                datos = _reportes.PromedioPreciosPorCategoria().ToList();
            }

            _grilla.DataSource = datos;
            _lblValor.Text = $"Valor total del inventario activo: {_reportes.ValorTotalInventario():C2}";
        }
        catch (AccesoDatosException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Exportar()
    {
        using var dlg = new SaveFileDialog { Filter = "Archivo CSV (*.csv)|*.csv", FileName = "reporte.csv" };
        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            switch (_cboReporte.SelectedIndex)
            {
                case 0: ExportadorCsv.Exportar(_reportes.ProductosPorCategoria(), dlg.FileName); break;
                case 1: ExportadorCsv.Exportar(_reportes.Top5Suplidores(), dlg.FileName); break;
                default: ExportadorCsv.Exportar(_reportes.PromedioPreciosPorCategoria(), dlg.FileName); break;
            }
            MessageBox.Show(this, "Reporte exportado correctamente.", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (IOException ex)
        {
            Entidades.Comun.Logger.RegistrarError("Exportar reporte", ex);
            MessageBox.Show(this, "No se pudo escribir el archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}




