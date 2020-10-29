using MIS.Infomat.Windows;
using Serilog;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MIS.Infomat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            ServiceProvider = Program.Run();
            if (ServiceProvider == null)
            {
                Shutdown();
            }
        }

        public IServiceProvider ServiceProvider { get; }

        private void App_DispatcherUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, $"Unhandled exception of type '{e.Exception.GetType()}' was thrown.");
            e.Handled = true;

            MainWindow mainWindow = Current.MainWindow as MainWindow;
            mainWindow.MainWorkflow();
        }
    }
}
