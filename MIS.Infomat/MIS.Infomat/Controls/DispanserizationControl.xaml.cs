#region Copyright © 2020 Vladimir Deryagin. All rights reserved
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
				}
				catch (Exception ex)
				{
					Log.Error(ex, "При записи на диспансеризацию произошла ошибка");
				}

				_mainWindow.PrevWorkflow<ActionsControl>();
			}
		}

		private void PrevButton_Click(Object sender, RoutedEventArgs e)
		{
			_mainWindow.PrevWorkflow();
		}
	}
}
