using System.Collections.ObjectModel;
using System.Windows.Input;
using Entidades.Excepciones;
using Entidades.Modelos;
using PresentacionWpf.Servicios;

namespace PresentacionWpf.MVVM;
public abstract class CrudViewModelBase<T> : ViewModelBase, IModulo where T : EntidadBase
{
    private readonly IDialogService _dialogo;
    private string _texto = string.Empty;
    private bool _mostrarEliminados;
    private T? _seleccionado;
    private string _errorGeneral = string.Empty;
    protected CrudViewModelBase(IDialogService dialogo)
    {
        _dialogo = dialogo;
        NuevoCommand = new RelayCommand(Nuevo);
        GuardarCommand = new RelayCommand(Guardar);
        EliminarCommand = new RelayCommand(Eliminar, () => Seleccionado is not null && !MostrarEliminados);
        RestaurarCommand = new RelayCommand(Restaurar, () => Seleccionado is not null && MostrarEliminados);
    }
    public ObservableCollection<T> Items { get; } = new();
    public string Texto
    {
        get => _texto;
        set { if (SetProperty(ref _texto, value)) Refrescar(); }
    }
    public bool MostrarEliminados
    {
        get => _mostrarEliminados;
        set
        {
            if (SetProperty(ref _mostrarEliminados, value))
            {
                Seleccionado = null;
                Refrescar();
            }
        }
    }
    public T? Seleccionado
    {
        get => _seleccionado;
        set
        {
            if (SetProperty(ref _seleccionado, value) && value is not null)
            {
                LimpiarErrores();
                ErrorGeneral = string.Empty;
                CargarFormulario(value);
            }
        }
    }
    public string ErrorGeneral
    {
        get => _errorGeneral;
        protected set => SetProperty(ref _errorGeneral, value);
    }
    public ICommand NuevoCommand { get; }
    public ICommand GuardarCommand { get; }
    public ICommand EliminarCommand { get; }
    public ICommand RestaurarCommand { get; }
    public virtual void Refrescar()
    {
        try
        {
            var datos = MostrarEliminados ? CargarEliminados() : Cargar(Texto);
            Items.Clear();
            foreach (var item in datos)
            {
                Items.Add(item);
            }
        }
        catch (AccesoDatosException ex)
        {
            ErrorGeneral = ex.Message;
        }
    }
    protected abstract IReadOnlyList<T> Cargar(string? texto);
    protected abstract IReadOnlyList<T> CargarEliminados();
    protected abstract void CargarFormulario(T entidad);
    protected abstract void LimpiarFormulario();
    protected abstract int GuardarEntidad();
    protected abstract void EliminarPorId(int id);
    protected abstract void RestaurarPorId(int id);

    private void Nuevo()
    {
        LimpiarErrores();
        ErrorGeneral = string.Empty;
        SetProperty(ref _seleccionado, null, nameof(Seleccionado));
        LimpiarFormulario();
    }

    private void Guardar()
    {
        LimpiarErrores();
        ErrorGeneral = string.Empty;
        try
        {
            var id = GuardarEntidad();
            Refrescar();
            SetProperty(ref _seleccionado, Items.FirstOrDefault(e => e.Id == id), nameof(Seleccionado));
            ErrorGeneral = "Guardado correctamente.";
        }
        catch (ValidacionException ex)
        {
            AplicarErrores(ex.Errores);
            ErrorGeneral = ex.Message;
        }
        catch (AccesoDatosException ex)
        {
            ErrorGeneral = ex.Message;
        }
    }

    private void Eliminar()
    {
        if (Seleccionado is null || !_dialogo.Confirmar($"¿Enviar '{Seleccionado}' a la papelera?"))
        {
            return;
        }

        try
        {
            EliminarPorId(Seleccionado.Id);
            Nuevo();
            Refrescar();
        }
        catch (ValidacionException ex)
        {
            ErrorGeneral = ex.Message;
        }
        catch (AccesoDatosException ex)
        {
            ErrorGeneral = ex.Message;
        }
    }

    private void Restaurar()
    {
        if (Seleccionado is null)
        {
            return;
        }

        try
        {
            RestaurarPorId(Seleccionado.Id);
            Nuevo();
            Refrescar();
        }
        catch (ValidacionException ex)
        {
            ErrorGeneral = ex.Message;
        }
        catch (AccesoDatosException ex)
        {
            ErrorGeneral = ex.Message;
        }
    }
}




