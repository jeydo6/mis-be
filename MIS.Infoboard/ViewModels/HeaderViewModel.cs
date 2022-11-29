using System;

namespace MIS.Infoboard.ViewModels;

internal sealed class HeaderViewModel : BaseViewModel
{
    private string? _organizationName;
    private DateTime? _dateTime;
    
    public string? OrganizationName
    {
        get => _organizationName;
        set => RaiseAndSetIfChanged(ref _organizationName, value);
    }

    public DateTime? DateTime
    {
        get => _dateTime;
        set => RaiseAndSetIfChanged(ref _dateTime, value);
    }
}
