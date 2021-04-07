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
using MIS.Application.Queries;
using MIS.Infomat.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MIS.Infomat.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IMediator _mediator;

		private readonly DispatcherTimer _timer;

		private readonly Stack<UserControl> _path;

		private Boolean _serviceMode = false;

		public MainWindow()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();

			_path = new Stack<UserControl>();

			InitializeComponent();

			_timer = new DispatcherTimer()
			{
				Interval = new TimeSpan(0, 0, 60)
			};
			_timer.Tick += TimerTick;

			TimerTick(this, null);
		}

		public Boolean IsServiceTime { get; private set; }

		public void ResetTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer.Start();
			}
		}

		public void PrevWorkflow()
		{
			if (_path.Count > 0)
			{
				_path.Pop();
			}

			UserControl userControl = null;
			if (_path.Count > 0)
			{
				userControl = _path.Peek();
			}

			NextWorkflow(userControl ?? new MainControl(), isRemember: false);
		}

		public void PrevWorkflow<T>() where T : UserControl
		{
			UserControl userControl = null;
			while (_path.Count > 0)
			{
				if (_path.Peek() is T item)
				{
					userControl = item;
					break;
				}
				else
				{
					_path.Pop();
				}
			}

			NextWorkflow(userControl ?? new MainControl(), isRemember: false);
		}

		public void NextWorkflow(UserControl userControl, Boolean isRemember = true)
		{
			if (userControl != null)
			{
				if (isRemember)
				{
					_path.Push(userControl);
				}

				workflow.Children.Clear();
				workflow.Children.Add(userControl);

				ResetTimer();
			}
		}

		public void MainWorkflow()
		{
			_path.Clear();

			NextWorkflow(new MainControl(), isRemember: false);
		}

		private async void TimerTick(Object sender, EventArgs e)
		{
			MainWorkflow();

			IsServiceTime = await _mediator.Send(
				new TimeIsServiceQuery()
			);
		}

		private void Window_KeyUp(Object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F11)
			{
				_serviceMode = !_serviceMode;

				if (_serviceMode)
				{
					Cursor = Cursors.Arrow;
					WindowStyle = WindowStyle.SingleBorderWindow;
				}
				else
				{
					Cursor = Cursors.None;
					WindowStyle = WindowStyle.None;
				}
			}

			if (e.Key == Key.F12)
			{
				Close();
			}
		}

		private void Window_MouseUp(Object sender, MouseButtonEventArgs e)
		{
			ResetTimer();
		}
	}
}