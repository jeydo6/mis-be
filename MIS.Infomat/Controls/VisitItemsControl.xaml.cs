#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Commands;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
using MIS.Infomat.PrintForms;
using MIS.Infomat.Windows;
using MIS.Mediator;
using Serilog;

namespace MIS.Infomat.Controls
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
