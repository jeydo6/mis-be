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

using MIS.Infoboard.Windows;
using Serilog;
using System;
using System.Windows.Threading;

namespace MIS.Infoboard
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application
	{
		public App()
		{
			ServiceProvider = Program.Run();
			if (ServiceProvider == null)
			{
				Shutdown();
			}
		}

		public IServiceProvider ServiceProvider { get; }

		private void App_DispatcherUnhandledException(Object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Log.Error(e.Exception, $"Unhandled exception of type '{e.Exception.GetType()}' was thrown.");
			e.Handled = true;

			MainWindow mainWindow = Current.MainWindow as MainWindow;
			mainWindow.MainWorkflow();
		}
	}
}
