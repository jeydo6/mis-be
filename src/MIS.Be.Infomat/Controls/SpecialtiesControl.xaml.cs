using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Application.Queries;
using MIS.Be.Application.ViewModels;
using MIS.Be.Infomat.Windows;
using MIS.Be.Mediator;

namespace MIS.Be.Infomat.Controls
{
	/// <summary>
	/// Логика взаимодействия для SpecialtiesControl.xaml
	/// </summary>
	public partial class SpecialtiesControl : UserControl
	{
		private readonly PatientViewModel _patient;

		private readonly MainWindow _mainWindow;

		private readonly IMediator _mediator;

		internal SpecialtiesControl()
		{
			throw new ArgumentNullException($"Field '{nameof(_patient)}' can't be empty!");
		}

		internal SpecialtiesControl(PatientViewModel patient)
		{
			_patient = patient;

			var app = System.Windows.Application.Current as App;

			_mainWindow = app.MainWindow as MainWindow;

			_mediator = app.ServiceProvider.GetService<IMediator>();

			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			list.ItemsSource = _mediator.Send(
				new SpecialtyListItemsQuery(_patient)
			);
		}

		private void ListItemButton_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is Button button && button.DataContext is SpecialtyViewModel specialtyItem)
			{
				_mainWindow.NextWorkflow(new ResourcesControl(_patient, specialtyItem));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (VisualTreeHelper.GetChild(list, 0) is ScrollViewer scrollViewer)
			{
				scrollViewer.LineUp();
			}
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (VisualTreeHelper.GetChild(list, 0) is ScrollViewer scrollViewer)
			{
				scrollViewer.LineDown();
			}
		}

		private void PrevButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.PrevWorkflow();
		}
	}
}
