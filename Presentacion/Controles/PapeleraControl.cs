using Entidades.Excepciones;
using LogicaNegocio.Servicios.Contratos;
using Presentacion.Modelos;

namespace Presentacion.Controles;
public sealed class PapeleraControl : UserControl, IRefrescable
{
    private readonly ICategoriaServicio _categorias;
    private readonly ISuplidorServicio _suplidores;
    private readonly IProductoServicio _productos;

    private readonly ComboBox _cboTipo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
    private readonly Label _lblTotal = new() { AutoSize = true, Padding = new Padding(16, 8, 0, 0) };
    private readonly DataGridView _grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        RowHeadersVisible = false,
        BackgroundColor = Color.White,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
    public PapeleraControl(ICategoriaServicio categorias, ISuplidorServicio suplidores, IProductoServicio productos)
    {
        _categorias = categorias;
        _suplidores = suplidores;
        _productos = productos;
        Dock = DockStyle.Fill;

        _cboTipo.Items.AddRange(new object[] { "Todos", "Categoría", "Suplidor", "Producto" });
        _cboTipo.SelectedIndex = 0;
        _cboTipo.SelectedIndexChanged += (_, _) => Refrescar();

        var btnRestaurar = new Button { Text = "Restaurar seleccionado", AutoSize = true };
        btnRestaurar.Click += (_, _) => Restaurar();

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8) };
        barra.Controls.Add(new Label { Text = "Ver:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        barra.Controls.Add(_cboTipo);
        barra.Controls.Add(btnRestaurar);
        barra.Controls.Add(_lblTotal);

        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementoPapelera.Tipo), HeaderText = "Tipo" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementoPapelera.Nombre), HeaderText = "Nombre" });
        _grilla.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(ElementoPapelera.FechaModificacion),
            HeaderText = "Eliminado el",
            DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
        });

        Controls.Add(_grilla);
        Controls.Add(barra);
        Load += (_, _) => Refrescar();
    }
    public void Refrescar()
    {
        try
        {
            var elementos = new List<ElementoPapelera>();
            elementos.AddRange(_categorias.ListarEliminadas()
                .Select(c => new ElementoPapelera(c.Id, "Categoría", c.Nombre, c.FechaModificacion)));
            elementos.AddRange(_suplidores.ListarEliminados()
                .Select(s => new ElementoPapelera(s.Id, "Suplidor", s.NombreEmpresa, s.FechaModificacion)));
            elementos.AddRange(_productos.ListarEliminados()
                .Select(p => new ElementoPapelera(p.Id, "Producto", p.Nombre, p.FechaModificacion)));

            var tipo = _cboTipo.SelectedItem as string ?? "Todos";
            var visibles = elementos
                .Where(e => tipo == "Todos" || e.Tipo == tipo)
                .OrderByDescending(e => e.FechaModificacion)
                .ToList();

            _grilla.DataSource = visibles;
            _lblTotal.Text = $"En papelera: {visibles.Count}";
        }
        catch (AccesoDatosException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Restaurar()
    {
        if (_grilla.CurrentRow?.DataBoundItem is not ElementoPapelera elemento)
        {
            MessageBox.Show(this, "Seleccione un elemento para restaurar.", "Papelera",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            switch (elemento.Tipo)
            {
                case "Categoría": _categorias.Restaurar(elemento.Id); break;
                case "Suplidor": _suplidores.Restaurar(elemento.Id); break;
                default: _productos.Restaurar(elemento.Id); break;
            }
            Refrescar();
        }
        catch (ValidacionException ex)
        {
            MessageBox.Show(this, ex.Message, "No se pudo restaurar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (AccesoDatosException ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}




