using Presentacion.Diseño;

namespace Presentacion.Formularios;
public sealed partial class LoginForm
{
    private TextBox _txtUsuario = null!;
    private TextBox _txtContraseña = null!;
    private Label _lblError = null!;
    private Button _btnEntrar = null!;
    private void DisenarFormulario()
    {
        SuspendLayout();

        Text = "Iniciar sesión";
        ClientSize = new Size(860, 520);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterScreen;
        Font = Tema.FuenteNormal;
        BackColor = Tema.Superficie;

        var raiz = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        raiz.Controls.Add(CrearPanelMarca(), 0, 0);
        raiz.Controls.Add(CrearPanelAcceso(), 1, 0);
        Controls.Add(raiz);

        AcceptButton = _btnEntrar;
        Shown += (_, _) => _txtUsuario.Focus();

        ResumeLayout(false);
    }

    private static Panel CrearPanelMarca()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Sidebar };

        panel.Controls.Add(new Label
        {
            Text = "I",
            Size = new Size(60, 60),
            Location = new Point(56, 60),
            BackColor = Tema.Acento,
            ForeColor = Color.White,
            Font = Tema.FuenteHero,
            TextAlign = ContentAlignment.MiddleCenter
        });
        panel.Controls.Add(new Label
        {
            Text = "Sistema de Gestión\nde Inventario",
            AutoSize = true,
            Font = Tema.FuenteHero,
            ForeColor = Color.White,
            Location = new Point(56, 150)
        });
        panel.Controls.Add(new Label
        {
            Text = "Controle categorías, suplidores, productos y reportes desde un solo lugar.",
            Font = Tema.FuenteNormal,
            ForeColor = Tema.SidebarTexto,
            Location = new Point(56, 280),
            Size = new Size(320, 60)
        });
        panel.Controls.Add(new Label
        {
            Text = "© 2026 · Control de inventario",
            AutoSize = true,
            Font = Tema.FuenteAyuda,
            ForeColor = Tema.SidebarTexto,
            Location = new Point(56, 466)
        });
        return panel;
    }

    private Panel CrearPanelAcceso()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Tema.Superficie };

        panel.Controls.Add(new Label
        {
            Text = "Bienvenido",
            AutoSize = true,
            Font = Tema.FuenteBienvenida,
            ForeColor = Tema.TextoPrincipal,
            Location = new Point(48, 110)
        });
        panel.Controls.Add(new Label
        {
            Text = "Inicie sesión para continuar",
            AutoSize = true,
            Font = Tema.FuenteNormal,
            ForeColor = Tema.TextoSecundario,
            Location = new Point(50, 150)
        });
        panel.Controls.Add(new Label
        {
            Text = "Usuario",
            AutoSize = true,
            Font = Tema.FuenteEtiqueta,
            ForeColor = Tema.TextoPrincipal,
            Location = new Point(48, 200)
        });

        _txtUsuario = new TextBox
        {
            Location = new Point(48, 224),
            Size = new Size(334, 32),
            Font = Tema.FuenteNormal,
            BorderStyle = BorderStyle.FixedSingle,
            MaxLength = 50,
            PlaceholderText = "Ingrese su usuario"
        };
        panel.Controls.Add(_txtUsuario);

        panel.Controls.Add(new Label
        {
            Text = "Contraseña",
            AutoSize = true,
            Font = Tema.FuenteEtiqueta,
            ForeColor = Tema.TextoPrincipal,
            Location = new Point(48, 280)
        });

        _txtContraseña = new TextBox
        {
            Location = new Point(48, 304),
            Size = new Size(334, 32),
            Font = Tema.FuenteNormal,
            BorderStyle = BorderStyle.FixedSingle,
            MaxLength = 100,
            PasswordChar = '•',
            PlaceholderText = "Ingrese su contraseña"
        };
        panel.Controls.Add(_txtContraseña);

        _lblError = new Label
        {
            Location = new Point(48, 346),
            Size = new Size(334, 22),
            Font = Tema.FuenteAyuda,
            ForeColor = Tema.Error,
            Text = string.Empty
        };
        panel.Controls.Add(_lblError);

        _btnEntrar = new Button
        {
            Text = "Iniciar sesión",
            Location = new Point(48, 380),
            Size = new Size(334, 46),
            FlatStyle = FlatStyle.Flat,
            BackColor = Tema.Primario,
            ForeColor = Color.White,
            Font = Tema.FuenteBoton,
            Cursor = Cursors.Hand
        };
        _btnEntrar.FlatAppearance.BorderSize = 0;
        _btnEntrar.FlatAppearance.MouseOverBackColor = Tema.PrimarioHover;
        _btnEntrar.FlatAppearance.MouseDownBackColor = Tema.PrimarioHover;
        _btnEntrar.Click += (_, _) => IntentarIngresar();
        panel.Controls.Add(_btnEntrar);

        return panel;
    }
}




