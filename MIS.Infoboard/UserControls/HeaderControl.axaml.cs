using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MIS.Infoboard.ViewModels;

namespace MIS.Infoboard.UserControls;

public partial class HeaderControl : UserControl
{
    public HeaderControl()
    {
        ViewModel = new HeaderControlControlViewModel();
        DataContext = ViewModel;

        InitializeComponent();

        new DispatcherTimer(TimeSpan.FromSeconds(15), DispatcherPriority.Normal, Timer_OnTick).Start();
    }

    private HeaderControlControlViewModel ViewModel { get; }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void HeaderControl_OnInitialized(object? sender, EventArgs e)
    {
        ViewModel.OrganizationName = "Поликлиника";
        ViewModel.DateTime = DateTime.Now;
    }

    private void Timer_OnTick(object? sender, EventArgs e)
    {
        ViewModel.DateTime = DateTime.Now;
    }
}
