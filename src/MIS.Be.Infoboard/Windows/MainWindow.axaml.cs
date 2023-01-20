using Avalonia.Controls;
using Avalonia.Input;

namespace MIS.Be.Infoboard.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.F11)
            {
                if (WindowState is WindowState.FullScreen)
                {
                    Cursor = Cursor.Default;
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    Cursor = new Cursor(StandardCursorType.None);
                    WindowState = WindowState.FullScreen;
                }
            }
        }
    }
}
