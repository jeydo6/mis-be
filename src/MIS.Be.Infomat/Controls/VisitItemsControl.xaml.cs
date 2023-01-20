using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Application.Commands;
using MIS.Be.Application.Queries;
using MIS.Be.Application.ViewModels;
using MIS.Be.Infomat.PrintForms;
using MIS.Be.Infomat.Windows;
using MIS.Be.Mediator;
using Serilog;

namespace MIS.Be.Infomat.Controls
{
	/// <summary>
	/// Логика взаимодействия для VisitsControl.xaml
	/// </summary>
	public partial class VisitItemsControl : UserControl
	{
		private readonly PatientViewModel _patient;

		private readonly MainWindow _mainWindow;

		private readonly IMediator _mediator;

		internal VisitItemsControl()
		{
			throw new ArgumentNullException($"Field '{nameof(_patient)}' can't be empty!");
		}

		internal VisitItemsControl(PatientViewModel patient)
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
				new VisitListItemsQuery(_patient)
			);

			dispanserizationButton.DataContext = _mediator.Send(
				new DispanserizationLastQuery(_patient)
			);

			//if (dispanserizationButton.DataContext != null)
			//{
			//    dispanserizationButton.Visibility = Visibility.Visible;
			//}
		}

		private void VisitItemButton_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is Button button && button.DataContext is VisitItemViewModel visitItem)
			{
				try
				{
					_mediator.Send(
						new PrintFormPrintCommand(new VisitItemPrintForm(visitItem))
					);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "При печати записи на приём произошла ошибка");
				}

				visitItem.IsEnabled = false;
				button.Visibility = Visibility.Collapsed;
			}
		}

		private void DispanserizationButton_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is Button button && button.DataContext is DispanserizationViewModel dispanserization)
			{
				try
				{
					_mediator.Send(
						new PrintFormPrintCommand(new DispanserizationPrintForm(dispanserization))
					);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "При печати диспансеризации произошла ошибка");
				}

				dispanserization.IsEnabled = false;
				button.Visibility = Visibility.Collapsed;
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
