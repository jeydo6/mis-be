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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MIS.Infoboard.Controls;

namespace MIS.Infoboard.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool _serviceMode = false;

		public MainWindow()
		{
			InitializeComponent();

			MainWorkflow();
		}

		public void NextWorkflow(UserControl userControl)
		{
			if (userControl != null)
			{
				workflow.Children.Clear();
				workflow.Children.Add(userControl);
			}
		}

		public void MainWorkflow()
		{
			NextWorkflow(new MainControl());
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
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