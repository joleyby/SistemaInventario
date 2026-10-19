using Presentacion.Diseño;

namespace Presentacion.Formularios;
public sealed partial class CategoriaForm
{
    private TextBox _txtNombre = null!;
    private TextBox _txtDescripcion = null!;
    private ListBox _lstFotos = null!;
    private Button _btnAgregarFoto = null!;
    private Button _btnQuitarFoto = null!;
    private Label _lblAyudaFotos = null!;
    private PictureBox _picVistaPrevia = null!;
    private void DisenarFormulario()
    {
        _txtNombre = new TextBox { Width = 340, MaxLength = 100 };
        _txtDescripcion = new TextBox
        {
            Width = 340,
            Height = 84,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            MaxLength = 500
        };

        _lstFotos = new ListBox
        {
            Width = 300,
            Height = 112,
            IntegralHeight = false,
            BorderStyle = BorderStyle.FixedSingle,
            Font = Tema.FuenteNormal,
            ItemHeight = 22
        };

        _btnAgregarFoto = new Button { Text = "Agregar...", Width = 120, Height = 36, Cursor = Cursors.Hand };
        _btnQuitarFoto = new Button
        {
            Text = "Quitar",
            Width = 120,
            Height = 36,
            Margin = new Padding(0, 8, 0, 0),
            Cursor = Cursors.Hand
        };
        EstilizarBotonSecundario(_btnAgregarFoto);
        EstilizarBotonSecundario(_btnQuitarFoto);

        _picVistaPrevia = new PictureBox
        {
            Size = new Size(300, 190),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Tema.Fondo,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 10, 0, 0)
        };

        _btnAgregarFoto.Click += (_, _) => AgregarFotos();
        _btnQuitarFoto.Click += (_, _) => QuitarFoto();
        _lstFotos.SelectedIndexChanged += (_, _) => MostrarVistaPrevia();
        _lstFotos.DoubleClick += (_, _) => AbrirFotoSeleccionada();

        var botonesFotos = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            Margin = new Padding(12, 0, 0, 0)
        };
        botonesFotos.Controls.Add(_btnAgregarFoto);
        botonesFotos.Controls.Add(_btnQuitarFoto);

        _lblAyudaFotos = new Label
        {
            Text = "Máx. 5 fotos · .jpg / .png · 2 MB c/u · Seleccione una foto para verla · Doble clic para abrirla",
            AutoSize = true,
            Font = Tema.FuenteAyuda,
            ForeColor = Tema.TextoSecundario,
            Margin = new Padding(0, 6, 0, 0)
        };

        var panelFotos = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 3,
            Margin = Padding.Empty
        };
        panelFotos.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panelFotos.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        panelFotos.Controls.Add(_lstFotos, 0, 0);
        panelFotos.Controls.Add(botonesFotos, 1, 0);
        panelFotos.Controls.Add(_lblAyudaFotos, 0, 1);
        panelFotos.Controls.Add(_picVistaPrevia, 0, 2);
        panelFotos.SetColumnSpan(_lblAyudaFotos, 2);
        panelFotos.SetColumnSpan(_picVistaPrevia, 2);

        AgregarSeccion("Información de la categoría");
        AgregarCampo("Nombre", "Nombre *", _txtNombre);
        AgregarCampo("Descripcion", "Descripción *", _txtDescripcion);

        AgregarSeccion("Fotos");
        AgregarCampo("Fotos", "Imágenes", panelFotos);
    }
    private static void EstilizarBotonSecundario(Button boton)
    {
        boton.FlatStyle = FlatStyle.Flat;
        boton.BackColor = Tema.Superficie;
        boton.ForeColor = Tema.TextoPrincipal;
        boton.Font = Tema.FuenteBoton;
        boton.FlatAppearance.BorderSize = 1;
        boton.FlatAppearance.BorderColor = Tema.Borde;
        boton.FlatAppearance.MouseOverBackColor = Tema.Fondo;
        boton.FlatAppearance.MouseDownBackColor = Tema.Fondo;
    }
}




