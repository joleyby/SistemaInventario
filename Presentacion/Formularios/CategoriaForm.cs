using System.Diagnostics;
using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;

namespace Presentacion.Formularios;
public sealed partial class CategoriaForm : FormularioBase
{
    private readonly ICategoriaServicio _servicio;
    private readonly Categoria _categoria;
    public CategoriaForm(ICategoriaServicio servicio, Categoria? categoria)
        : base(categoria is null ? "Nueva categoría" : "Editar categoría")
    {
        _servicio = servicio;
        _categoria = categoria ?? new Categoria();

        DisenarFormulario();
        CargarDatos();
    }
    protected override void GuardarDatos()
    {
        _categoria.Nombre = _txtNombre.Text;
        _categoria.Descripcion = _txtDescripcion.Text;
        _servicio.Guardar(_categoria);
    }
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _picVistaPrevia.Image?.Dispose();
        base.OnFormClosed(e);
    }

    private void CargarDatos()
    {
        _txtNombre.Text = _categoria.Nombre;
        _txtDescripcion.Text = _categoria.Descripcion;
        foreach (var foto in _categoria.Fotos)
        {
            _lstFotos.Items.Add(foto);
        }
    }

    private void AgregarFotos()
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            Multiselect = true,
            Title = "Seleccione fotos (máx. 2 MB cada una)"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        foreach (var ruta in dlg.FileNames)
        {
            if (new FileInfo(ruta).Length > Validador.TamañoMaximoFotoBytes)
            {
                MessageBox.Show(this, $"'{Path.GetFileName(ruta)}' supera los 2 MB y no se agregó.",
                    "Foto no válida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                continue;
            }

            var foto = new Foto
            {
                NombreArchivo = Path.GetFileName(ruta),
                Extension = Path.GetExtension(ruta).ToLowerInvariant(),
                Contenido = File.ReadAllBytes(ruta)
            };
            _categoria.Fotos.Add(foto);
            _lstFotos.Items.Add(foto);
            _lstFotos.SelectedItem = foto;
        }
    }

    private void QuitarFoto()
    {
        if (_lstFotos.SelectedItem is not Foto foto)
        {
            return;
        }

        _categoria.Fotos.Remove(foto);
        _lstFotos.Items.Remove(foto);
        MostrarVistaPrevia();
    }
    private void MostrarVistaPrevia()
    {
        _picVistaPrevia.Image?.Dispose();
        _picVistaPrevia.Image = null;

        if (_lstFotos.SelectedItem is not Foto foto)
        {
            return;
        }

        var bytes = ObtenerBytes(foto);
        if (bytes.Length == 0)
        {
            return;
        }
        using var ms = new MemoryStream(bytes);
        using var original = Image.FromStream(ms);
        _picVistaPrevia.Image = new Bitmap(original);
    }

    private static byte[] ObtenerBytes(Foto foto)
    {
        if (foto.Contenido.Length > 0)
        {
            return foto.Contenido;   // Foto recién elegida, todavía no guardada.
        }

        if (string.IsNullOrEmpty(foto.Url))
        {
            return Array.Empty<byte>();
        }

        var ruta = new Uri(foto.Url).LocalPath;
        return File.Exists(ruta) ? File.ReadAllBytes(ruta) : Array.Empty<byte>();
    }

    private void AbrirFotoSeleccionada()
    {
        if (_lstFotos.SelectedItem is not Foto foto)
        {
            return;
        }

        if (string.IsNullOrEmpty(foto.Url))
        {
            MessageBox.Show(this, "Guarde la categoría para generar el enlace de la foto.", "Foto",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        Process.Start(new ProcessStartInfo(foto.Url) { UseShellExecute = true });
    }
}




