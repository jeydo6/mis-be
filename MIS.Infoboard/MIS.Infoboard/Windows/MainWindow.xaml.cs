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
using MIS.Infoboard.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MIS.Infoboard.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IMediator _mediator;

		private Boolean _serviceMode = false;

		public MainWindow()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();

			InitializeComponent();
		}

		public void NextWorkflow(UserControl userControl, Boolean isRemember = true)
		{
			if (userControl != null)
			{
				workflow.Children.Clear();
				workflow.Children.Add(userControl);
			}
		}

		public void MainWorkflow()
		{
			NextWorkflow(new MainControl(), isRemember: false);
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
	}
}