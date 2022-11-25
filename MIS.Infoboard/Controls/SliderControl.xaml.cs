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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Extensions;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
using MIS.Mediator;

namespace MIS.Infoboard.Controls
{
	/// <summary>
	/// Логика взаимодействия для SliderControl.xaml
	/// </summary>
	public partial class SliderControl : UserControl
	{
		private readonly IMediator _mediator;

		private readonly Action[] _actions;

		private int _index;

		public SliderControl()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();
			_actions = new Action[]
			{
				LoadSpecialtiesPages,
				LoadDepartmentPages
			};

			_index = -1;

			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			MoveNext(sender, e);
		}

		private void Pages_Done(object sender, RoutedEventArgs e)
		{
			MoveNext(sender, e);
		}

		private void MoveNext(object sender, EventArgs e)
		{
			if (_actions == null || _actions.Length == 0)
			{
				return;
			}

			if (_index >= _actions.Length - 1)
			{
				_index = -1;
			}

			_actions[++_index]();
		}

		private void LoadSpecialtiesPages()
		{
			var specialties = _mediator.Send(
				new SpecialtyListItemsQuery(patient: null)
			);

			var actualHeight = (int)ActualHeight;
			var itemHeight = 80;
			var headerHeight = 90 + 20;

			pages.Content = "Расписание приёма врачей";
			pages.Items = specialties
				.GroupBy(actualHeight, itemHeight, headerHeight)
				.Select(items => new PageViewModel
				{
					Objects = items
				})
				.ToArray();
		}

		private void LoadDepartmentPages()
		{
			var departments = _mediator.Send(
				new DepartmentListItemsQuery()
			);

			var actualHeight = (int)ActualHeight;
			var itemHeight = 120;
			var headerHeight = 90 + 20;

			pages.Content = "Контакты";
			pages.Items = departments
				.GroupBy(actualHeight, itemHeight, headerHeight)
				.Select(items => new PageViewModel
				{
					Objects = items
				})
				.ToArray();
		}
	}
}
