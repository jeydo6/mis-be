using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MIS.Infoboard.ViewModels;

namespace MIS.Infoboard.UserControls;

public partial class FooterControl : UserControl
{
    public FooterControl()
    {
        ViewModel = new FooterControlControlViewModel();
        DataContext = ViewModel;
        
        InitializeComponent();
        
        new DispatcherTimer(TimeSpan.FromSeconds(15), DispatcherPriority.Normal, Timer_OnTick).Start();
    }
    
    private FooterControlControlViewModel ViewModel { get; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void FooterControl_OnInitialized(object? sender, EventArgs e)
    {
        ViewModel.DateTime = DateTime.Now;
    }
    
    private void Timer_OnTick(object? sender, EventArgs e)
    {
        ViewModel.DateTime = DateTime.Now;
    }
}
