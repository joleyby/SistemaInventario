using Presentacion.Diseño;

namespace Presentacion.Formularios;
public abstract partial class FormularioBase
{
    private readonly Dictionary<string, Control> _campos = new();
    private ErrorProvider _errorProvider = null!;
    private TableLayoutPanel _raiz = null!;
    private Panel _encabezado = null!;
    private Label _lblTitulo = null!;
    private Label _lblSubtitulo = null!;
    private TableLayoutPanel _grid = null!;
    private Panel _pie = null!;
    private Button _btnGuardar = null!;
    private Button _btnCancelar = null!;
    private void InicializarComponentes()
    {
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        BackColor = Tema.Superficie;
        Font = Tema.FuenteNormal;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(540, 0);
        TextChanged += (_, _) => _lblTitulo.Text = Text;

        _errorProvider = new ErrorProvider
        {
            BlinkStyle = ErrorBlinkStyle.NeverBlink,
            ContainerControl = this
        };
        _lblTitulo = new Label
        {
            AutoSize = true,
            Font = Tema.FuenteTitulo,
            ForeColor = Color.White,
            Location = new Point(28, 14)
        };
        _lblSubtitulo = new Label
        {
            AutoSize = true,
            Font = Tema.FuenteSubtitulo,
            ForeColor = Tema.PrimarioTexto,
            Location = new Point(30, 46),
            Text = "Los campos marcados con * son obligatorios"
        };
        _encabezado = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Primario };
        _encabezado.Controls.Add(_lblTitulo);
        _encabezado.Controls.Add(_lblSubtitulo);
        _grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2,
            Padding = new Padding(28, 22, 28, 8),
            BackColor = Tema.Superficie,
            Margin = Padding.Empty
        };
        _grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _btnGuardar = new Button
        {
            Text = "Guardar",
            Width = 130,
            FlatStyle = FlatStyle.Flat,
            BackColor = Tema.Primario,
            ForeColor = Color.White,
            Font = Tema.FuenteBoton,
            Cursor = Cursors.Hand
        };
        _btnGuardar.FlatAppearance.BorderSize = 0;
        _btnGuardar.FlatAppearance.MouseOverBackColor = Tema.PrimarioHover;
        _btnGuardar.FlatAppearance.MouseDownBackColor = Tema.PrimarioHover;
        _btnGuardar.Click += (_, _) => GuardarConValidacion();

        _btnCancelar = new Button
        {
            Text = "Cancelar",
            Width = 120,
            DialogResult = DialogResult.Cancel,
            FlatStyle = FlatStyle.Flat,
            BackColor = Tema.Superficie,
            ForeColor = Tema.TextoPrincipal,
            Font = Tema.FuenteBoton,
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 10, 0)
        };
        _btnCancelar.FlatAppearance.BorderSize = 1;
        _btnCancelar.FlatAppearance.BorderColor = Tema.Borde;
        _btnCancelar.FlatAppearance.MouseOverBackColor = Tema.Fondo;
        _btnCancelar.FlatAppearance.MouseDownBackColor = Tema.Fondo;
        CancelButton = _btnCancelar;

        var botones = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 15, 0, 0)
        };
        botones.Controls.Add(_btnGuardar);
        botones.Controls.Add(_btnCancelar);

        var linea = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Tema.Borde };
        _pie = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Tema.PieFondo,
            Padding = new Padding(28, 0, 28, 0)
        };
        _pie.Controls.Add(botones);
        _pie.Controls.Add(linea);
        _raiz = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 3,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Tema.Superficie
        };
        _raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        _raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        _raiz.Controls.Add(_encabezado, 0, 0);
        _raiz.Controls.Add(_grid, 0, 1);
        _raiz.Controls.Add(_pie, 0, 2);
        Controls.Add(_raiz);

        ResumeLayout(false);
    }
    protected void AgregarSeccion(string titulo)
    {
        var fila = AgregarFila();
        var lbl = new Label
        {
            Text = titulo.ToUpperInvariant(),
            AutoSize = true,
            Font = Tema.FuenteSeccion,
            ForeColor = Tema.Primario,
            Margin = new Padding(0, fila == 0 ? 0 : 12, 0, 6)
        };
        _grid.Controls.Add(lbl, 0, fila);
        _grid.SetColumnSpan(lbl, 2);
    }
    protected void AgregarCampo(string clave, string etiqueta, Control control)
    {
        var fila = AgregarFila();
        var lbl = new Label
        {
            Text = etiqueta,
            AutoSize = true,
            Font = Tema.FuenteEtiqueta,
            ForeColor = Tema.TextoPrincipal,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 9, 18, 0)
        };

        control.Font = Tema.FuenteNormal;
        control.BackColor = Tema.Superficie;
        if (control is TextBoxBase texto)
        {
            texto.BorderStyle = BorderStyle.FixedSingle;
        }
        if (control is ComboBox combo)
        {
            combo.FlatStyle = FlatStyle.Standard;
        }
        control.Margin = new Padding(0, 4, 0, 12);

        _grid.Controls.Add(lbl, 0, fila);
        _grid.Controls.Add(control, 1, fila);
        _campos[clave] = control;
    }

    private int AgregarFila()
    {
        _grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        return _grid.RowStyles.Count - 1;
    }
}




