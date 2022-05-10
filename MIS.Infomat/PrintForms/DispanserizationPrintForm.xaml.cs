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

using Microsoft.Extensions.DependencyInjection;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;
using System;
using System.Windows.Controls;

namespace MIS.Infomat.PrintForms
{
	/// <summary>
	/// Логика взаимодействия для DispanserizationPrintForm.xaml
	/// </summary>
	public partial class DispanserizationPrintForm : UserControl
	{
		internal DispanserizationPrintForm()
		{
			throw new ArgumentNullException($"Print model can't be empty!");
		}

		internal DispanserizationPrintForm(DispanserizationViewModel dispanserization)
		{
			var app = System.Windows.Application.Current as App;

			var dateTimeProvider = app.ServiceProvider.GetService<IDateTimeProvider>();

			InitializeComponent();

			DataContext = new DispanserizationPrintFormViewModel
			{
				Now = dateTimeProvider.Now,
				Dispanserization = dispanserization
			};
		}
	}
}
