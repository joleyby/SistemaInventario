using Entidades.Modelos;
using Presentacion.Diseño;

namespace Presentacion.Controles;
public sealed partial class BitacoraControl
{
    private TextBox _txtBuscar = null!;
    private Button _btnActualizar = null!;
    private Label _lblTotal = null!;
    private DataGridView _grid = null!;
    private void DisenarControl()
    {
        Dock = DockStyle.Fill;
        BackColor = Tema.Superficie;
        Font = Tema.FuenteNormal;

        _txtBuscar = new TextBox
        {
            Width = 300,
            PlaceholderText = "Buscar por usuario, acción o detalle..."
        };

        _btnActualizar = new Button
        {
            Text = "Actualizar",
            AutoSize = true,
            Padding = new Padding(10, 4, 10, 4),
            FlatStyle = FlatStyle.Flat,
            BackColor = Tema.Superficie,
            ForeColor = Tema.TextoPrincipal,
            Font = Tema.FuenteBoton,
            Cursor = Cursors.Hand
        };
        _btnActualizar.FlatAppearance.BorderSize = 1;
        _btnActualizar.FlatAppearance.BorderColor = Tema.Borde;
        _btnActualizar.FlatAppearance.MouseOverBackColor = Tema.Fondo;
        _btnActualizar.FlatAppearance.MouseDownBackColor = Tema.Fondo;

        _lblTotal = new Label
        {
            AutoSize = true,
            Font = Tema.FuenteEtiqueta,
            ForeColor = Tema.TextoSecundario,
            Padding = new Padding(16, 8, 0, 0)
        };

        var barra = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(0, 0, 0, 12)
        };
        barra.Controls.Add(new Label
        {
            Text = "Buscar:",
            AutoSize = true,
            Font = Tema.FuenteEtiqueta,
            ForeColor = Tema.TextoPrincipal,
            Padding = new Padding(0, 8, 6, 0)
        });
        barra.Controls.Add(_txtBuscar);
        barra.Controls.Add(_btnActualizar);
        barra.Controls.Add(_lblTotal);

        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoGenerateColumns = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Tema.Superficie,
            BorderStyle = BorderStyle.None,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            Font = Tema.FuenteNormal
        };
        _grid.Columns.AddRange(new DataGridViewColumn[]
        {
            Columna(nameof(RegistroBitacora.FechaHora), "Fecha", "dd/MM/yyyy HH:mm:ss", 1.4f),
            Columna(nameof(RegistroBitacora.UsuarioNombre), "Usuario", null, 1.2f),
            Columna(nameof(RegistroBitacora.Accion), "Acción", null, 1.4f),
            Columna(nameof(RegistroBitacora.Entidad), "Entidad", null, 1f),
            Columna(nameof(RegistroBitacora.EntidadId), "Id", null, 0.5f),
            Columna(nameof(RegistroBitacora.Detalle), "Detalle", null, 3f)
        });

        Controls.Add(_grid);
        Controls.Add(barra);
    }

    private static DataGridViewTextBoxColumn Columna(string propiedad, string encabezado, string? formato, float peso)
    {
        var columna = new DataGridViewTextBoxColumn
        {
            DataPropertyName = propiedad,
            HeaderText = encabezado,
            FillWeight = peso
        };
        if (formato is not null)
        {
            columna.DefaultCellStyle.Format = formato;
        }
        return columna;
    }
}




