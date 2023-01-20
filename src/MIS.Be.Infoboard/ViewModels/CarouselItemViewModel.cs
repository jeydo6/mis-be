using System;
using MIS.Be.Application.ViewModels;

namespace MIS.Be.Infoboard.ViewModels;

internal abstract class BaseCarouselItemViewModel : BaseViewModel
{
    public TimeSpan Interval { get; set; }
}

internal class StringCarouselItemViewModel : BaseCarouselItemViewModel
{
    private string _value = string.Empty;

    public string Value
    {
        get => _value;
        set => RaiseAndSetIfChanged(ref _value, value);
    }
}

internal class DepartmentsCarouselItemViewModel : BaseCarouselItemViewModel
{
    private DepartmentViewModel[] _values = Array.Empty<DepartmentViewModel>();
    
    public DepartmentViewModel[] Values
    {
        get => _values;
        set => RaiseAndSetIfChanged(ref _values, value);
    }
}

internal class SpecialtiesCarouselItemViewModel : BaseCarouselItemViewModel
{
    private SpecialtyViewModel[] _values = Array.Empty<SpecialtyViewModel>();
    
    public SpecialtyViewModel[] Values
    {
        get => _values;
        set => RaiseAndSetIfChanged(ref _values, value);
    }
}
