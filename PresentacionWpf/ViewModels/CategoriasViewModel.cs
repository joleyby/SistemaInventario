using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Entidades.Modelos;
using LogicaNegocio.Servicios.Contratos;
using LogicaNegocio.Validaciones;
using PresentacionWpf.MVVM;
using PresentacionWpf.Servicios;

namespace PresentacionWpf.ViewModels;
public sealed class CategoriasViewModel : CrudViewModelBase<Categoria>
{
    private readonly ICategoriaServicio _servicio;
    private readonly IDialogService _dialogos;
    private Categoria _actual = new();
    private string _nombre = string.Empty;
    private string _descripcion = string.Empty;
    private Foto? _fotoSeleccionada;
    public CategoriasViewModel(ICategoriaServicio servicio, IDialogService dialogos) : base(dialogos)
    {
        _servicio = servicio;
        _dialogos = dialogos;
        AgregarFotosCommand = new RelayCommand(AgregarFotos, () => Fotos.Count < Validador.MaximoFotosPorCategoria);
        QuitarFotoCommand = new RelayCommand(QuitarFoto, () => FotoSeleccionada is not null);
        AbrirFotoCommand = new RelayCommand(AbrirFoto, () => FotoSeleccionada is not null);
        Refrescar();
    }
    public ObservableCollection<Foto> Fotos { get; } = new();
    public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }
    public string Descripcion { get => _descripcion; set => SetProperty(ref _descripcion, value); }
    public Foto? FotoSeleccionada
    {
        get => _fotoSeleccionada;
        set => SetProperty(ref _fotoSeleccionada, value);
    }
    public ICommand AgregarFotosCommand { get; }
    public ICommand QuitarFotoCommand { get; }
    public ICommand AbrirFotoCommand { get; }
    protected override IReadOnlyList<Categoria> Cargar(string? texto) => _servicio.Listar(texto);
    protected override IReadOnlyList<Categoria> CargarEliminados() => _servicio.ListarEliminadas();
    protected override void CargarFormulario(Categoria entidad)
    {
        _actual = _servicio.ObtenerPorId(entidad.Id) ?? entidad;
        Nombre = _actual.Nombre;
        Descripcion = _actual.Descripcion;
        Fotos.Clear();
        foreach (var foto in _actual.Fotos)
        {
            Fotos.Add(foto);
        }
    }
    protected override void LimpiarFormulario()
    {
        _actual = new Categoria();
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Fotos.Clear();
    }
    protected override int GuardarEntidad()
    {
        _actual.Nombre = Nombre;
        _actual.Descripcion = Descripcion;
        _actual.Fotos.Clear();
        _actual.Fotos.AddRange(Fotos);
        _servicio.Guardar(_actual);
        return _actual.Id;
    }
    protected override void EliminarPorId(int id) => _servicio.Eliminar(id);
    protected override void RestaurarPorId(int id) => _servicio.Restaurar(id);

    private void AgregarFotos()
    {
        foreach (var ruta in _dialogos.SeleccionarImagenes())
        {
            if (new FileInfo(ruta).Length > Validador.TamañoMaximoFotoBytes)
            {
                ErrorGeneral = $"'{Path.GetFileName(ruta)}' supera los 2 MB y no se agregó.";
                continue;
            }

            Fotos.Add(new Foto
            {
                NombreArchivo = Path.GetFileName(ruta),
                Extension = Path.GetExtension(ruta).ToLowerInvariant(),
                Contenido = File.ReadAllBytes(ruta)
            });
        }
    }

    private void QuitarFoto()
    {
        if (FotoSeleccionada is not null)
        {
            Fotos.Remove(FotoSeleccionada);
            FotoSeleccionada = null;
        }
    }

    private void AbrirFoto()
    {
        if (FotoSeleccionada is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(FotoSeleccionada.Url))
        {
            ErrorGeneral = "Guarde la categoría para generar el enlace de la foto.";
            return;
        }

        _dialogos.AbrirEnlace(FotoSeleccionada.Url);
    }
}




