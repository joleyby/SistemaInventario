using Entidades.Excepciones;
using Entidades.Modelos;
using LogicaNegocio.Genericos;
using LogicaNegocio.Servicios.Contratos;

namespace Presentacion.Controles;
public sealed partial class BitacoraControl : UserControl, IRefrescable
{
    private readonly IBitacoraServicio _servicio;
    private IReadOnlyList<RegistroBitacora> _registros = Array.Empty<RegistroBitacora>();
    public BitacoraControl(IBitacoraServicio servicio)
    {
        _servicio = servicio;
        DisenarControl();

        _txtBuscar.TextChanged += (_, _) => Filtrar();
        _btnActualizar.Click += (_, _) => Refrescar();
        Load += (_, _) => Refrescar();
    }
    public void Refrescar()
    {
        try
        {
            _registros = _servicio.Listar();
            Filtrar();
        }
        catch (ValidacionException ex)
        {
            Mostrar(ex.Message);
        }
        catch (AccesoDatosException ex)
        {
            Mostrar(ex.Message);
        }
    }

    private void Filtrar()
    {
        var filtrados = Buscador.Filtrar(_registros, _txtBuscar.Text,
                r => r.UsuarioNombre, r => r.Accion, r => r.Entidad, r => r.Detalle)
            .ToList();
        _grid.DataSource = filtrados;
        _lblTotal.Text = $"Registros: {filtrados.Count}";
    }

    private void Mostrar(string texto) =>
        MessageBox.Show(this, texto, "Bitácora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}




