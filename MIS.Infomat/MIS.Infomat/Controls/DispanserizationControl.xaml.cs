using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Commands;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
using MIS.Domain.Services;
using MIS.Infomat.PrintForms;
using MIS.Infomat.Windows;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MIS.Infomat.Controls
{
    /// <summary>
    /// Логика взаимодействия для DispanserizationControl.xaml
    /// </summary>
    public partial class DispanserizationControl : UserControl
    {
        private readonly PatientViewModel _patient;

        private readonly IMediator _mediator;
        private readonly IPrintService _printService;

        private readonly MainWindow _mainWindow;

        internal DispanserizationControl()
        {
            throw new ArgumentNullException($"Field '{nameof(_patient)}' can't be empty!");
        }

        internal DispanserizationControl(PatientViewModel patient)
        {
            _patient = patient;

            var app = System.Windows.Application.Current as App;

            _mainWindow = app.MainWindow as MainWindow;

            _mediator = app.ServiceProvider.GetService<IMediator>();
            _printService = app.ServiceProvider.GetService<IPrintService>();

            InitializeComponent();
        }

        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            datesHeader.Content = _mediator.Send(
                new DateHeaderQuery()
            ).Result;

            datesList.ItemsSource = _mediator.Send(
                new DispanserizationListItemsQuery()
            ).Result;
        }

        private void DateListItemButton_Click(Object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.DataContext is DispanserizationViewModel dispanserizationItem)
            {
                try
                {
                    DispanserizationViewModel dispanserization = _mediator.Send(
                        new DispanserizationCreateCommand(dispanserizationItem.BeginDate, _patient.ID, _patient.Code, _patient.DisplayName)
                    ).Result;

                    _patient.Dispanserizations.Add(dispanserization);

                    _printService.Print(
                        new DispanserizationPrintForm(dispanserization)
                    );

                    _mainWindow.PrevWorkflow<ActionsControl>();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "При записи на диспансеризацию произошла ошибка");

                    button.IsEnabled = false;
                    if (button.Content is TextBlock textBlock)
                    {
                        textBlock.Foreground = Brushes.DarkGray;
                    }
                }
            }
        }

        private void PrevButton_Click(Object sender, RoutedEventArgs e)
        {
            _mainWindow.PrevWorkflow();
        }
    }
}
