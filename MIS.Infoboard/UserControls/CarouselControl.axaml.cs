using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Extensions;
using MIS.Application.Queries;
using MIS.Infoboard.ViewModels;
using MIS.Mediator;

namespace MIS.Infoboard.UserControls;

public partial class CarouselControl : UserControl
{
    private readonly IMediator _mediator;
    
    private readonly DispatcherTimer _timer;
    private readonly Func<BaseCarouselItemControlViewModel[]>[] _actions;

    private BaseCarouselItemControlViewModel[] _items;

    private int _actionIndex;
    private int _itemIndex;

    public CarouselControl()
    {
        var serviceProvider = Avalonia.Application.Current.GetServiceProvider();
        _mediator = serviceProvider.GetRequiredService<IMediator>();

        ViewModel = new CarouselControlViewModel();
        DataContext = ViewModel;

        _timer = new DispatcherTimer(TimeSpan.Zero, DispatcherPriority.Normal, Timer_OnTick);
        _actions = new Func<BaseCarouselItemControlViewModel[]>[]
        {
            GetDepartments,
            GetSpecialties
        };
        _items = Array.Empty<BaseCarouselItemControlViewModel>();

        InitializeComponent();

        _timer.Start();
    }

    private CarouselControlViewModel ViewModel { get; }
    
    private void UserControl_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Left)
            Next();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Timer_OnTick(object? sender, EventArgs e) => Next();

    private void Next()
    {
        if (_actions.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_actions), "The list of actions must be greater than 0.");

        if (_itemIndex > _items.Length - 1)
        {
            if (_actionIndex > _actions.Length - 1)
                _actionIndex = 0;

            _items = _actions[_actionIndex++]();
            _itemIndex = 0;
        }

        ViewModel.Current = _items[_itemIndex++];
        _timer.Interval = ViewModel.Current.Interval;
    }
    
    private BaseCarouselItemControlViewModel[] GetDepartments()
    {
        var departments = _mediator.Send(
            new DepartmentListItemsQuery()
        );

        if (departments.Length == 0)
            return Array.Empty<BaseCarouselItemControlViewModel>();
        
        var maxHeight = (int)Bounds.Height;
        const int itemHeight = 120;
        const int headerHeight = 110;
        
        var items = departments
            .GroupBy(maxHeight, itemHeight, headerHeight)
            .Select(g => new DepartmentsCarouselControlItemViewModel
            {
                Interval = TimeSpan.FromSeconds(12),
                Values = g.ToArray()
            })
            .ToArray();

        var result = new BaseCarouselItemControlViewModel[items.Length + 1];
        result[0] = new StringCarouselControlItemViewModel
        {
            Interval = TimeSpan.FromSeconds(3),
            Value = "Контакты"
        };
        
        Array.Copy(items, 0, result, 1, items.Length);

        return result;
    }

    private BaseCarouselItemControlViewModel[] GetSpecialties()
    {
        var specialties = _mediator.Send(
            new SpecialtyListItemsQuery(patient: null)
        );

        if (specialties.Length == 0)
            return Array.Empty<BaseCarouselItemControlViewModel>();
        
        var maxHeight = (int)Bounds.Height;
        const int itemHeight = 80;
        const int headerHeight = 110;
        
        var items = specialties
            .GroupBy(maxHeight, itemHeight, headerHeight)
            .Select(g => new SpecialtiesCarouselControlItemViewModel
            {
                Interval = TimeSpan.FromSeconds(12),
                Values = g.ToArray()
            })
            .ToArray();

        var result = new BaseCarouselItemControlViewModel[items.Length + 1];
        result[0] = new StringCarouselControlItemViewModel
        {
            Interval = TimeSpan.FromSeconds(3),
            Value = "Расписание приёма врачей"
        };
        
        Array.Copy(items, 0, result, 1, items.Length);

        return result;
    }
}
