using System;
using MIS.Application.ViewModels;

namespace MIS.Infoboard.ViewModels;

internal abstract class BaseCarouselItemControlViewModel : BaseControlViewModel
{
    public TimeSpan Interval { get; set; }
}

internal class StringCarouselControlItemViewModel : BaseCarouselItemControlViewModel
{
    private string _value = string.Empty;

    public string Value
    {
        get => _value;
        set => RaiseAndSetIfChanged(ref _value, value);
    }
}

internal class DepartmentsCarouselControlItemViewModel : BaseCarouselItemControlViewModel
{
    private DepartmentViewModel[] _values = Array.Empty<DepartmentViewModel>();
    
    public DepartmentViewModel[] Values
    {
        get => _values;
        set => RaiseAndSetIfChanged(ref _values, value);
    }
}

internal class SpecialtiesCarouselControlItemViewModel : BaseCarouselItemControlViewModel
{
    private SpecialtyViewModel[] _values = Array.Empty<SpecialtyViewModel>();
    
    public SpecialtyViewModel[] Values
    {
        get => _values;
        set => RaiseAndSetIfChanged(ref _values, value);
    }
}
