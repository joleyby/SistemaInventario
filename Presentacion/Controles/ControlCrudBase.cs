using Entidades.Excepciones;
using Entidades.Modelos;

namespace Presentacion.Controles;
public abstract class ControlCrudBase<T> : UserControl, IRefrescable where T : EntidadBase
{
    protected readonly record struct ColumnaGrid(string Propiedad, string Encabezado, string? Formato = null);

    private readonly TextBox _txtBuscar = new() { PlaceholderText = "Escriba para buscar...", Width = 300 };
    private readonly Button _btnNuevo = new() { Text = "Nuevo", AutoSize = true };
    private readonly Button _btnEditar = new() { Text = "Editar", AutoSize = true };
    private readonly Button _btnEliminar = new() { Text = "Eliminar", AutoSize = true };
    private readonly Label _lblTotal = new() { AutoSize = true, Padding = new Padding(16, 8, 0, 0) };
    protected readonly FlowLayoutPanel PanelFiltros = new() { AutoSize = true, Padding = new Padding(8, 0, 8, 0) };
    protected readonly DataGridView Grilla = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AutoGenerateColumns = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        RowHeadersVisible = false,
        BackgroundColor = Color.White,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
    protected ControlCrudBase()
    {
        Dock = DockStyle.Fill;

        _btnNuevo.Click += (_, _) => Nuevo();
        _btnEditar.Click += (_, _) => Editar();
        _btnEliminar.Click += (_, _) => EliminarSeleccionado();
        _txtBuscar.TextChanged += (_, _) => Refrescar();
        Grilla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Editar(); };
        Load += (_, _) => Refrescar();

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(CrearBarra(), 0, 0);
        layout.Controls.Add(PanelFiltros, 0, 1);
        layout.Controls.Add(Grilla, 0, 2);
        Controls.Add(layout);
    }
    protected abstract IReadOnlyList<T> Cargar(string? texto);
    protected abstract ColumnaGrid[] Columnas { get; }
    protected abstract bool MostrarFormulario(T? entidad);
    protected abstract void EliminarPorId(int id);
    public virtual void Refrescar()
    {
        try
        {
            var datos = Cargar(_txtBuscar.Text);
            if (Grilla.Columns.Count == 0)
            {
                ConfigurarColumnas();
            }
            Grilla.DataSource = datos.ToList();
            _lblTotal.Text = $"Registros: {datos.Count}";
        }
        catch (AccesoDatosException ex)
        {
            Mostrar(ex.Message, MessageBoxIcon.Error);
        }
    }

    private Control CrearBarra()
    {
        var barra = new FlowLayoutPanel { AutoSize = true, Padding = new Padding(8) };
        barra.Controls.Add(new Label { Text = "Buscar:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        barra.Controls.Add(_txtBuscar);
        barra.Controls.Add(_btnNuevo);
        barra.Controls.Add(_btnEditar);
        barra.Controls.Add(_btnEliminar);
        barra.Controls.Add(_lblTotal);
        return barra;
    }

    private void ConfigurarColumnas()
    {
        foreach (var columna in Columnas)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = columna.Propiedad,
                HeaderText = columna.Encabezado
            };
            if (columna.Formato is not null)
            {
                col.DefaultCellStyle.Format = columna.Formato;
            }
            Grilla.Columns.Add(col);
        }
    }

    private void Nuevo()
    {
        if (MostrarFormulario(null))
        {
            Refrescar();
        }
    }

    private void Editar()
    {
        if (Grilla.CurrentRow?.DataBoundItem is not T seleccionado)
        {
            Mostrar("Seleccione un registro.", MessageBoxIcon.Information);
            return;
        }

        if (MostrarFormulario(seleccionado))
        {
            Refrescar();
        }
    }

    private void EliminarSeleccionado()
    {
        if (Grilla.CurrentRow?.DataBoundItem is not T seleccionado)
        {
            Mostrar("Seleccione un registro.", MessageBoxIcon.Information);
            return;
        }

        var confirmar = MessageBox.Show(this, $"¿Enviar '{seleccionado}' a la papelera?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes)
        {
            return;
        }

        try
        {
            EliminarPorId(seleccionado.Id);
            Refrescar();
        }
        catch (ValidacionException ex)
        {
            Mostrar(ex.Message, MessageBoxIcon.Warning);
        }
        catch (AccesoDatosException ex)
        {
            Mostrar(ex.Message, MessageBoxIcon.Error);
        }
    }
    protected void Mostrar(string texto, MessageBoxIcon icono) =>
        MessageBox.Show(this, texto, "Sistema de Inventario", MessageBoxButtons.OK, icono);
}




