using Entidades.Modelos;
using Presentacion.Diseño;
using System.Drawing.Drawing2D;

namespace Presentacion;
public sealed partial class MainForm
{
    private static readonly string[] Iconos = { "▦", "◉", "▤", "▣", "◔" };

    private readonly List<Button> _botonesMenu = new();
    private readonly List<Panel> _indicadores = new();
    private string[] _titulos = Array.Empty<string>();
    private string _iniciales = "U";

    private TableLayoutPanel _raiz = null!;
    private Label _lblPagina = null!;
    private Label _lblSubtitulo = null!;
    private Label _lblRol = null!;
    private Panel _avatar = null!;
    private TabControl _pestanas = null!;
    private Button _btnCerrarSesion = null!;
    private void DisenarFormulario(IReadOnlyList<(string Titulo, Control Contenido)> paneles, Usuario usuario)
    {
        SuspendLayout();

        _titulos = paneles.Select(p => p.Titulo).ToArray();
        var partes = usuario.NombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        _iniciales = partes.Length == 0
            ? "U"
            : string.Concat(partes.Take(2).Select(p => char.ToUpperInvariant(p[0])));

        Text = "Sistema de Gestión de Inventario";
        Font = Tema.FuenteNormal;
        BackColor = Tema.Fondo;
        MinimumSize = new Size(1120, 720);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;

        _raiz = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Tema.Fondo
        };
        _raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 252));
        _raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _raiz.Controls.Add(CrearBarraLateral(paneles, usuario), 0, 0);
        _raiz.Controls.Add(CrearAreaPrincipal(paneles, usuario), 1, 0);
        Controls.Add(_raiz);

        ActualizarNavegacion(0);
        ResumeLayout(false);
    }

    private Control CrearBarraLateral(IReadOnlyList<(string Titulo, Control Contenido)> paneles, Usuario usuario)
    {
        var barra = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            BackColor = Tema.Sidebar,
            Margin = Padding.Empty,
            Padding = new Padding(14, 0, 14, 18)
        };
        barra.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        barra.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
        barra.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        barra.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        barra.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        barra.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        barra.Controls.Add(CrearLogo(), 0, 0);
        barra.Controls.Add(CrearEtiquetaSeccion("MENÚ PRINCIPAL"), 0, 1);
        barra.Controls.Add(CrearMenu(paneles), 0, 2);
        barra.Controls.Add(CrearPerfil(usuario), 0, 3);
        barra.Controls.Add(CrearBotonCerrarSesion(), 0, 4);
        return barra;
    }

    private static Control CrearLogo()
    {
        var fila = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Tema.Sidebar,
            Margin = Padding.Empty,
            Padding = new Padding(4, 22, 0, 0)
        };

        var insignia = new Label
        {
            Text = "I",
            Size = new Size(36, 36),
            BackColor = Tema.Acento,
            ForeColor = Color.White,
            Font = Tema.FuenteLogo,
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(0, 0, 12, 0)
        };

        var textos = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink, // Agregado
            BackColor = Tema.Sidebar,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        textos.Controls.Add(new Label
        {
            Text = "Inventario.",
            AutoSize = true,
            Font = Tema.FuenteLogo,
            ForeColor = Color.White,
            Margin = new Padding(0, 0, 0, 2)
        });
        textos.Controls.Add(new Label
        {
            Text = "Gestión de stock",
            AutoSize = true,
            Font = Tema.FuenteAyuda,
            ForeColor = Tema.SidebarTexto,
            Margin = Padding.Empty
        });

        fila.Controls.Add(insignia);
        fila.Controls.Add(textos);
        return fila;
    }

    private static Control CrearEtiquetaSeccion(string texto) => new Label
    {
        Text = texto,
        Dock = DockStyle.Fill,
        Font = Tema.FuenteSeccion,
        ForeColor = Tema.SidebarTexto,
        TextAlign = ContentAlignment.BottomLeft,
        Padding = new Padding(10, 0, 0, 8)
    };

    private FlowLayoutPanel CrearMenu(IReadOnlyList<(string Titulo, Control Contenido)> paneles)
    {
        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            BackColor = Tema.Sidebar,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };

        for (var i = 0; i < paneles.Count; i++)
        {
            var indice = i;
            var icono = i < Iconos.Length ? Iconos[i] : "•";

            var item = new Panel
            {
                Width = 224,
                Height = 46,
                Margin = new Padding(0, 0, 0, 4),
                BackColor = Tema.Sidebar
            };

            var boton = new Button
            {
                Text = $"  {icono}   {paneles[i].Titulo}",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = Tema.FuenteMenu,
                ForeColor = Tema.SidebarTexto,
                BackColor = Tema.Sidebar
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = Tema.SidebarActivo;
            boton.FlatAppearance.MouseDownBackColor = Tema.SidebarActivo;
            boton.Click += (_, _) => _pestanas.SelectedIndex = indice;

            var indicador = new Panel
            {
                Dock = DockStyle.Left,
                Width = 4,
                BackColor = Tema.Acento,
                Visible = false
            };

            item.Controls.Add(boton);
            item.Controls.Add(indicador);

            _botonesMenu.Add(boton);
            _indicadores.Add(indicador);
            menu.Controls.Add(item);
        }

        return menu;
    }

    private Control CrearPerfil(Usuario usuario)
    {
        var tarjeta = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Tema.SidebarActivo,
            Margin = new Padding(0, 6, 0, 8),
            Padding = new Padding(14, 0, 10, 0)
        };
        tarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tarjeta.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        _avatar = new Panel
        {
            Size = new Size(44, 44),
            BackColor = Tema.SidebarActivo,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 0, 12, 0)
        };
        _avatar.Paint += DibujarAvatar;

        var textos = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = Tema.SidebarActivo,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 0, 0, 0),
            Padding = Padding.Empty
        };
        textos.Controls.Add(new Label
        {
            Text = usuario.NombreCompleto,
            AutoSize = true, // Cambiado
            MaximumSize = new Size(140, 0), // Agregado
            Font = Tema.FuenteUsuario,
            ForeColor = Color.White,
            Margin = new Padding(0, 0, 0, 2) // Ajustado
        });
        _lblRol = new Label
        {
            Text = usuario.Rol,
            AutoSize = true,
            Font = Tema.FuenteAyuda,
            ForeColor = Tema.SidebarTexto,
            Margin = Padding.Empty
        };
        textos.Controls.Add(_lblRol);

        tarjeta.Controls.Add(_avatar, 0, 0);
        tarjeta.Controls.Add(textos, 1, 0);
        return tarjeta;
    }

    private void DibujarAvatar(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var pincel = new SolidBrush(Tema.Acento);
        e.Graphics.FillEllipse(pincel, 0, 0, _avatar.Width - 1, _avatar.Height - 1);
        TextRenderer.DrawText(e.Graphics, _iniciales, Tema.FuenteUsuario, _avatar.ClientRectangle, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private Control CrearBotonCerrarSesion()
    {
        _btnCerrarSesion = new Button
        {
            Text = "Cerrar sesión",
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Tema.SidebarTexto,
            BackColor = Tema.Sidebar,
            Font = Tema.FuenteMenu,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 8, 0, 0)
        };
        _btnCerrarSesion.FlatAppearance.BorderSize = 0;
        _btnCerrarSesion.FlatAppearance.MouseOverBackColor = Tema.SidebarActivo;
        _btnCerrarSesion.FlatAppearance.MouseDownBackColor = Tema.SidebarActivo;
        _btnCerrarSesion.Click += (_, _) => SolicitarCierreSesion();
        return _btnCerrarSesion;
    }

    private Control CrearAreaPrincipal(IReadOnlyList<(string Titulo, Control Contenido)> paneles, Usuario usuario)
    {
        var area = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Tema.Fondo,
            Margin = Padding.Empty,
            Padding = new Padding(28, 22, 28, 22)
        };
        area.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        area.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Cambiado de Absolute a AutoSize
        area.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        area.Controls.Add(CrearEncabezado(usuario), 0, 0);
        area.Controls.Add(CrearTarjetaContenido(paneles), 0, 1);
        return area;
    }

    private Control CrearEncabezado(Usuario usuario)
    {
        _lblPagina = new Label
        {
            Text = _titulos.Length > 0 ? _titulos[0] : string.Empty,
            AutoSize = true,
            Font = Tema.FuenteTituloPagina,
            ForeColor = Tema.TextoPrincipal,
            Margin = Padding.Empty
        };
        _lblSubtitulo = new Label
        {
            Text = $"Hoy · {DateTime.Now:dddd, dd 'de' MMMM 'de' yyyy}",
            AutoSize = true,
            Font = Tema.FuenteSubtitulo,
            ForeColor = Tema.TextoSecundario,
            Margin = new Padding(0, 4, 0, 0) // Agregado margen superior
        };
        var textos = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink, // Agregado
            BackColor = Tema.Fondo,
            Anchor = AnchorStyles.Left | AnchorStyles.Top, // Cambiado
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        textos.Controls.Add(_lblPagina);
        textos.Controls.Add(_lblSubtitulo);

        var chip = new Label
        {
            Text = usuario.Rol,
            AutoSize = true,
            Font = Tema.FuenteEtiqueta,
            ForeColor = Tema.Acento,
            BackColor = Tema.AcentoSuave,
            Padding = new Padding(12, 7, 12, 7),
            Anchor = AnchorStyles.Right,
            Margin = Padding.Empty
        };

        var encabezado = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Tema.Fondo,
            Margin = Padding.Empty,
            Padding = new Padding(0, 0, 0, 10) // Agregado margen inferior
        };
        encabezado.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        encabezado.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        encabezado.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Cambiado de Percent a AutoSize
        encabezado.Controls.Add(textos, 0, 0);
        encabezado.Controls.Add(chip, 1, 0);
        return encabezado;
    }

    private Control CrearTarjetaContenido(IReadOnlyList<(string Titulo, Control Contenido)> paneles)
    {
        var tarjeta = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Tema.Superficie,
            Margin = new Padding(0, 4, 0, 0)
        };
        tarjeta.Paint += (_, e) =>
        {
            using var borde = new Pen(Tema.Borde);
            e.Graphics.DrawRectangle(borde, 0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
        };
        _pestanas = new TabControl
        {
            Dock = DockStyle.Fill,
            DrawMode = TabDrawMode.OwnerDrawFixed,
            SizeMode = TabSizeMode.Fixed,
            ItemSize = new Size(1, 1),
            BackColor = Tema.Superficie
        };
        _pestanas.DrawItem += (_, _) => { };

        foreach (var (titulo, contenido) in paneles)
        {
            var pagina = new TabPage(titulo)
            {
                BackColor = Tema.Superficie,
                Padding = new Padding(26, 22, 26, 22),
                UseVisualStyleBackColor = false
            };
            ConfigurarTablasInternas(contenido);

            contenido.Dock = DockStyle.Fill;
            pagina.Controls.Add(contenido);
            _pestanas.TabPages.Add(pagina);
        }

        _pestanas.SelectedIndexChanged += (_, _) => ActualizarNavegacion(_pestanas.SelectedIndex);

        tarjeta.Controls.Add(_pestanas);
        return tarjeta;
    }
    private void ActualizarNavegacion(int indice)
    {
        for (var i = 0; i < _botonesMenu.Count; i++)
        {
            var activo = i == indice;
            _botonesMenu[i].BackColor = activo ? Tema.SidebarActivo : Tema.Sidebar;
            _botonesMenu[i].ForeColor = activo ? Color.White : Tema.SidebarTexto;
            _botonesMenu[i].Font = activo ? Tema.FuenteMenuActivo : Tema.FuenteMenu;
            _indicadores[i].Visible = activo;
        }

        if (indice >= 0 && indice < _titulos.Length)
        {
            _lblPagina.Text = _titulos[indice];
        }
    }
    private void ConfigurarTablasInternas(Control contenedor)
    {
        foreach (Control control in contenedor.Controls)
        {
            if (control is DataGridView dgv)
            {
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else if (control.HasChildren)
            {
                ConfigurarTablasInternas(control);
            }
        }
    }
}



