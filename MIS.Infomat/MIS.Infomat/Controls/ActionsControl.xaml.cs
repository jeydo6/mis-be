using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
using MIS.Infomat.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infomat.Controls
{
    /// <summary>
    /// Логика взаимодействия для ActionsControl.xaml
    /// </summary>
    public partial class ActionsControl : UserControl
    {
        private readonly PatientViewModel _patient;

        private readonly MainWindow _mainWindow;

        private readonly IMediator _mediator;

        internal ActionsControl()
        {
            throw new ArgumentNullException($"Field '{nameof(_patient)}' can't be empty!");
        }

        internal ActionsControl(PatientViewModel patient)
        {
            _patient = patient;

            var app = System.Windows.Application.Current as App;

            _mainWindow = app.MainWindow as MainWindow;

            _mediator = app.ServiceProvider.GetService<IMediator>();

            InitializeComponent();
        }

        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            Boolean dispanserizationIsRequire = _mediator.Send(
                new DispanserizationIsRequireQuery(_patient)
            ).Result;

            dispanserizationButton.Visibility = dispanserizationIsRequire ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TimesButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.IsServiceTime)
            {
                _mainWindow.NextWorkflow(new SpecialtiesControl(_patient));
            }
        }

        private void VisitsButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.IsServiceTime)
            {
                _mainWindow.NextWorkflow(new VisitItemsControl(_patient));
            }
        }

        private void DispanserizationButton_Click(Object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.IsServiceTime)
            {
                _mainWindow.NextWorkflow(new DispanserizationControl(_patient));
            }
        }

        private void PrevButton_Click(Object sender, RoutedEventArgs e)
        {
            _mainWindow.MainWorkflow();
        }
    }
}