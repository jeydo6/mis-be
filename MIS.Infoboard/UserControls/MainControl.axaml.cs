using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MIS.Infoboard.UserControls;

public partial class MainControl : UserControl
{
    public MainControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}