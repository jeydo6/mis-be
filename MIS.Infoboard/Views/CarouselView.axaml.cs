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

namespace MIS.Infoboard.Views;

public partial class CarouselView : UserControl
{
    private readonly IMediator _mediator;
    
    private readonly DispatcherTimer _timer;
    private readonly Func<BaseCarouselItemViewModel[]>[] _actions;

    private BaseCarouselItemViewModel[] _items;

    private int _actionIndex;
    private int _itemIndex;

    public CarouselView()
    {
        var serviceProvider = Avalonia.Application.Current.GetServiceProvider();
        _mediator = serviceProvider.GetRequiredService<IMediator>();

        ViewModel = new CarouselViewModel();
        DataContext = ViewModel;

        _timer = new DispatcherTimer(TimeSpan.Zero, DispatcherPriority.Normal, Timer_OnTick);
        _actions = new Func<BaseCarouselItemViewModel[]>[]
        {
            GetDepartments,
            GetSpecialties
        };
        _items = Array.Empty<BaseCarouselItemViewModel>();

        InitializeComponent();

        _timer.Start();
    }

    private CarouselViewModel ViewModel { get; }
    
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
    
    private BaseCarouselItemViewModel[] GetDepartments()
    {
        var departments = _mediator.Send(
            new DepartmentListItemsQuery()
        );

        if (departments.Length == 0)
            return Array.Empty<BaseCarouselItemViewModel>();
        
        var maxHeight = (int)Bounds.Height;
        const int itemHeight = 120;
        const int headerHeight = 110;
        
        var items = departments
            .GroupBy(maxHeight, itemHeight, headerHeight)
            .Select(g => new DepartmentsCarouselItemViewModel
            {
                Interval = TimeSpan.FromSeconds(12),
                Values = g.ToArray()
            })
            .ToArray();

        var result = new BaseCarouselItemViewModel[items.Length + 1];
        result[0] = new StringCarouselItemViewModel
        {
            Interval = TimeSpan.FromSeconds(3),
            Value = "Контакты"
        };
        
        Array.Copy(items, 0, result, 1, items.Length);

        return result;
    }

    private BaseCarouselItemViewModel[] GetSpecialties()
    {
        var specialties = _mediator.Send(
            new SpecialtyListItemsQuery(patient: null)
        );

        if (specialties.Length == 0)
            return Array.Empty<BaseCarouselItemViewModel>();
        
        var maxHeight = (int)Bounds.Height;
        const int itemHeight = 80;
        const int headerHeight = 110;
        
        var items = specialties
            .GroupBy(maxHeight, itemHeight, headerHeight)
            .Select(g => new SpecialtiesCarouselItemViewModel
            {
                Interval = TimeSpan.FromSeconds(12),
                Values = g.ToArray()
            })
            .ToArray();

        var result = new BaseCarouselItemViewModel[items.Length + 1];
        result[0] = new StringCarouselItemViewModel
        {
            Interval = TimeSpan.FromSeconds(3),
            Value = "Расписание приёма врачей"
        };
        
        Array.Copy(items, 0, result, 1, items.Length);

        return result;
    }
}
