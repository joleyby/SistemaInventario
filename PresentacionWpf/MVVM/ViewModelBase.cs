using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PresentacionWpf.MVVM;
public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errores = new();
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _errores.Count > 0;
    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is not null && _errores.TryGetValue(propertyName, out var mensajes))
        {
            return mensajes;
        }
        return Array.Empty<string>();
    }
    protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string? nombre = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor))
        {
            return false;
        }
        campo = valor;
        OnPropertyChanged(nombre);
        return true;
    }
    protected void OnPropertyChanged([CallerMemberName] string? nombre = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    public void AplicarErrores(IReadOnlyDictionary<string, string> errores)
    {
        LimpiarErrores();
        foreach (var (campo, mensaje) in errores)
        {
            _errores[campo] = new List<string> { mensaje };
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(campo));
        }
        OnPropertyChanged(nameof(HasErrors));
    }
    public void LimpiarErrores()
    {
        var campos = _errores.Keys.ToList();
        _errores.Clear();
        foreach (var campo in campos)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(campo));
        }
        OnPropertyChanged(nameof(HasErrors));
    }
}




