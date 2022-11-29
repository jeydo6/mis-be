using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MIS.Infoboard.ViewModels;

internal abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void RaiseAndSetIfChanged<T>(ref T oldValue, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (oldValue is not null && oldValue.Equals(newValue)) return;
        
        oldValue = newValue;
        RaisePropertyChanged(propertyName);
    }
    
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
