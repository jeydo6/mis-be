namespace MIS.Infoboard.ViewModels;

internal sealed class CarouselViewModel : BaseViewModel
{
    private BaseCarouselItemViewModel? _current;

    public BaseCarouselItemViewModel? Current
    {
        get => _current;
        set => RaiseAndSetIfChanged(ref _current, value);
    }
}
