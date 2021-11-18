#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
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
using MIS.Application.Extensions;
using MIS.Application.Queries;
using MIS.Domain.Providers;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MIS.Infoboard.Controls
{
	/// <summary>
	/// Логика взаимодействия для HeaderControl.xaml
	/// </summary>
	public partial class HeaderControl : UserControl
	{
		private readonly IMediator _mediator;
		private readonly IDateTimeProvider _dateTimeProvider;

		private readonly DispatcherTimer _timer;

		public HeaderControl()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();
			_dateTimeProvider = app.ServiceProvider.GetService<IDateTimeProvider>();

			_timer = new DispatcherTimer
			{
				IsEnabled = true,
				Interval = new TimeSpan(0, 0, 15)
			};
			_timer.Tick += Tick;

			InitializeComponent();

			_timer.Start();
		}

		private void UserControl_Initialized(Object sender, EventArgs e)
		{
			organizationName.Text = _mediator.SendSync(
				new OrganizationNameQuery()
			);
		}

		private void Tick(Object sender, EventArgs e)
		{
			time.Text = $"{_dateTimeProvider.Now:H:mm}";
			date.Text = $"{_dateTimeProvider.Now:dddd, d MMMM yyyy г.}";
		}
	}
}
