using System;

namespace MIS.Infoboard.ViewModels;

internal sealed class FooterControlControlViewModel : BaseControlViewModel
{
    private DateTime? _dateTime;
    
    public DateTime? DateTime
    {
        get => _dateTime;
        set => RaiseAndSetIfChanged(ref _dateTime, value);
    }
}
