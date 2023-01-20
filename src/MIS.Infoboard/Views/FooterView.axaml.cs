using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MIS.Infoboard.ViewModels;

namespace MIS.Infoboard.Views;

public partial class FooterView : UserControl
{
    public FooterView()
    {
        ViewModel = new FooterViewModel();
        DataContext = ViewModel;
        
        InitializeComponent();
        
        new DispatcherTimer(TimeSpan.FromSeconds(15), DispatcherPriority.Normal, Timer_OnTick).Start();
    }
    
    private FooterViewModel ViewModel { get; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void UserControl_OnInitialized(object? sender, EventArgs e)
    {
        ViewModel.DateTime = DateTime.Now;
    }
    
    private void Timer_OnTick(object? sender, EventArgs e)
    {
        ViewModel.DateTime = DateTime.Now;
    }
}
