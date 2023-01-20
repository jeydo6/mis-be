using System;
using System.Linq;
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
	/// Логика взаимодействия для TimeItemsControl.xaml
	/// </summary>
	public partial class TimeItemsControl : UserControl
	{
		private readonly PatientViewModel _patient;
		private readonly ResourceViewModel _resource;

		private readonly MainWindow _mainWindow;

		private readonly IMediator _mediator;

		internal TimeItemsControl()
		{
			throw new ArgumentNullException($"Fields '{nameof(_patient)}', '{nameof(_resource)}' can't be empty!");
		}

		internal TimeItemsControl(PatientViewModel patient, ResourceViewModel resource)
		{
			_patient = patient;
			_resource = resource;

			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();

			_mainWindow = app.MainWindow as MainWindow;

			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			datesHeader.Content = _mediator.Send(
				new DateHeaderQuery()
			);

			datesList.ItemsSource = _mediator.Send(
				new DateListItemsQuery(_resource)
			);
		}

		private void DateListItemButton_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is Button button && button.DataContext is DateItemViewModel dateItem)
			{
				if (dateItem.Times == null)
				{
					dateItem.Times = _mediator.Send(
						new TimeListItemsQuery(dateItem.Date, _resource.ResourceID)
					);

					dateItem.IsEnabled = dateItem.Times.Any(ti => ti.IsEnabled);
				}

				if (dateItem.IsEnabled)
				{
					timeHeader.Content = $"{dateItem.Date:dd MMMM yyyy г.}";
					timesList.ItemsSource = dateItem.Times;
				}
				else
				{
					button.IsEnabled = false;
					if (button.Content is TextBlock textBlock)
					{
						textBlock.Foreground = Brushes.DarkGray;
					}
				}
			}
		}

		private void TimeListItemButton_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is Button button && button.DataContext is TimeItemViewModel timeItem)
			{
				try
				{
					var visitItem = _mediator.Send(
						new VisitCreateCommand(timeItem.TimeItemID, _patient.ID, _patient.Code, _patient.Name)
					);

					_patient.VisitItems.Add(visitItem);

					_mediator.Send(
						new PrintFormPrintCommand(new VisitItemPrintForm(visitItem))
					);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "При записи на приём произошла ошибка");
				}

				_mainWindow.PrevWorkflow<ActionsControl>();
			}
		}

		private void PrevButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.PrevWorkflow();
		}
	}
}
