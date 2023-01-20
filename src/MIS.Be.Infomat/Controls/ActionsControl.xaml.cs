using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Application.Queries;
using MIS.Be.Application.ViewModels;
using MIS.Be.Infomat.Windows;
using MIS.Be.Mediator;

namespace MIS.Be.Infomat.Controls
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

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var dispanserizationIsRequired = _mediator.Send(
				new DispanserizationIsRequiredQuery(_patient)
			);

			dispanserizationButton.Visibility = dispanserizationIsRequired ? Visibility.Visible : Visibility.Collapsed;
		}

		private void TimesButton_Click(object sender, RoutedEventArgs e)
		{
			if (!_mainWindow.IsServiceTime)
			{
				_mainWindow.NextWorkflow(new SpecialtiesControl(_patient));
			}
		}

		private void VisitsButton_Click(object sender, RoutedEventArgs e)
		{
			if (!_mainWindow.IsServiceTime)
			{
				_mainWindow.NextWorkflow(new VisitItemsControl(_patient));
			}
		}

		private void DispanserizationButton_Click(object sender, RoutedEventArgs e)
		{
			if (!_mainWindow.IsServiceTime)
			{
				_mainWindow.NextWorkflow(new DispanserizationControl(_patient));
			}
		}

		private void PrevButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.MainWorkflow();
		}
	}
}
