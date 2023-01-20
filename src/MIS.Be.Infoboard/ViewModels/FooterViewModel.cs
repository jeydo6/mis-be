using System;

namespace MIS.Be.Infoboard.ViewModels;

internal sealed class FooterViewModel : BaseViewModel
{
    private DateTime? _dateTime;
    
    public DateTime? DateTime
    {
        get => _dateTime;
        set => RaiseAndSetIfChanged(ref _dateTime, value);
    }
}
