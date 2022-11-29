namespace MIS.Infoboard.ViewModels;

internal sealed class CarouselControlViewModel : BaseControlViewModel
{
    private BaseCarouselItemControlViewModel? _current;

    public BaseCarouselItemControlViewModel? Current
    {
        get => _current;
        set => RaiseAndSetIfChanged(ref _current, value);
    }
}
