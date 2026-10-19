using Entidades.Excepciones;

namespace Presentacion.Formularios;
public abstract partial class FormularioBase : Form
{
    protected FormularioBase(string titulo)
    {
        InicializarComponentes();
        Text = titulo;
    }
    protected abstract void GuardarDatos();
    protected void MostrarErrores(IReadOnlyDictionary<string, string> errores)
    {
        _errorProvider.Clear();
        var generales = new List<string>();

        foreach (var (clave, mensaje) in errores)
        {
            if (_campos.TryGetValue(clave, out var control))
            {
                _errorProvider.SetError(control, mensaje);
            }
            else
            {
                generales.Add(mensaje);
            }
        }

        if (generales.Count > 0)
        {
            MessageBox.Show(this, string.Join(Environment.NewLine, generales), "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void GuardarConValidacion()
    {
        _errorProvider.Clear();
        try
        {
            GuardarDatos();
            DialogResult = DialogResult.OK;
        }
        catch (ValidacionException ex)
        {
            MostrarErrores(ex.Errores);
        }
        catch (AccesoDatosException ex)
        {
            MessageBox.Show(this, ex.Message, "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}




