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
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MIS.Application.Configs;
using MIS.Application.ViewModels;

namespace MIS.Infomat.PrintForms
{
	/// <summary>
	/// Логика взаимодействия для VisitItemPrintForm.xaml
	/// </summary>
	public partial class VisitItemPrintForm : UserControl
	{
		internal VisitItemPrintForm()
		{
			throw new ArgumentNullException($"Print model can't be empty!");
		}

		internal VisitItemPrintForm(VisitItemViewModel visitItem)
		{
			var app = System.Windows.Application.Current as App;
			var settingsConfigOptions = app.ServiceProvider.GetService<IOptionsSnapshot<SettingsConfig>>();
			var settingsConfig = settingsConfigOptions.Value;

			InitializeComponent();

			DataContext = new VisitItemPrintFormViewModel
			{
				OrganizationName = settingsConfig.OrganizationName,
				VisitItem = visitItem
			};
		}
	}
}
