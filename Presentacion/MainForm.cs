using Entidades.Modelos;
using LogicaNegocio.Seguridad;
using LogicaNegocio.Servicios.Contratos;
using Presentacion.Controles;

namespace Presentacion;
public sealed partial class MainForm : Form
{
    public MainForm(ICategoriaServicio categorias, ISuplidorServicio suplidores, IProductoServicio productos,
                    IReporteServicio reportes, IUsuarioServicio usuarios, IBitacoraServicio bitacora,
                    ISesionUsuario sesion, Usuario usuario)
    {
        var paneles = new List<(string Titulo, Control Contenido)>
        {
            ("Categorías", new CategoriasControl(categorias)),
            ("Suplidores", new SuplidoresControl(suplidores)),
            ("Productos", new ProductosControl(productos, categorias, suplidores)),
            ("Papelera", new PapeleraControl(categorias, suplidores, productos)),
            ("Reportes", new ReportesControl(reportes))
        };
        if (sesion.EsAdministrador)
        {
            paneles.Add(("Usuarios", new UsuariosControl(usuarios)));
            paneles.Add(("Bitácora", new BitacoraControl(bitacora)));
        }

        DisenarFormulario(paneles, usuario);
        _pestanas.SelectedIndexChanged += (_, _) => RefrescarPestanaActiva();
    }
    public bool CerrarSesionSolicitado { get; private set; }

    private void SolicitarCierreSesion()
    {
        CerrarSesionSolicitado = true;
        Close();
    }

    private void RefrescarPestanaActiva()
    {
        var contenido = _pestanas.SelectedTab?.Controls.Count > 0 ? _pestanas.SelectedTab.Controls[0] : null;
        if (contenido is IRefrescable refrescable)
        {
            refrescable.Refrescar();
        }
    }
}




