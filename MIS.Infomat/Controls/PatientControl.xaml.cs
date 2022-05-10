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

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Extensions;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
using MIS.Infomat.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MIS.Infomat.Controls
{
	/// <summary>
	/// Логика взаимодействия для PatientControl.xaml
	/// </summary>
	public partial class PatientControl : UserControl
	{
		private readonly MainWindow _mainWindow;

		private readonly IMediator _mediator;

		internal PatientControl()
		{
			var app = System.Windows.Application.Current as App;

			_mainWindow = app.MainWindow as MainWindow;

			_mediator = app.ServiceProvider.GetService<IMediator>();

			InitializeComponent();
		}

		private void UserControl_Loaded(Object sender, RoutedEventArgs e)
		{
			numberTextBox.Focus();
		}

		private void InputButton_Click(Object sender, RoutedEventArgs e)
		{
			if (Keyboard.FocusedElement is TextBox textBox && e.OriginalSource is Button button && button.Content is TextBlock buttonContent)
			{
				textBox.Text = textBox.Text.Trim();

				if (textBox.Text.Length < textBox.MaxLength)
				{
					textBox.AppendText(buttonContent.Text);
					textBox.CaretIndex = textBox.Text.Length;
				}

				if (textBox == numberTextBox && textBox.Text.Length == textBox.MaxLength)
				{
					birthdateTextBox.Focus();
				}
			}

			_mainWindow.ResetTimer();
		}

		private void RemoveButton_Click(Object sender, RoutedEventArgs e)
		{
			if (Keyboard.FocusedElement is TextBox textBox && e.OriginalSource is Button button && button.Content is TextBlock buttonContent)
			{
				textBox.Text = textBox.Text.Trim();

				if (textBox.Text.Length > 0)
				{
					textBox.Text = textBox.Text[0..^1];
					textBox.CaretIndex = textBox.Text.Length;
				}
			}

			_mainWindow.ResetTimer();
		}

		private void PrevButton_Click(Object sender, RoutedEventArgs e)
		{
			_mainWindow.PrevWorkflow();
		}

		private void NextButton_Click(Object sender, RoutedEventArgs e)
		{
			Boolean numberValidation;
			if (String.IsNullOrEmpty(numberTextBox.Text))
			{
				numberValidation = false;
			}
			else if (numberTextBox.Text.Length != numberTextBox.MaxLength)
			{
				numberValidation = false;
			}
			else
			{
				numberValidation = true;
			}

			Boolean birthdateValidation;
			if (String.IsNullOrEmpty(birthdateTextBox.Text))
			{
				birthdateValidation = false;
			}
			else if (birthdateTextBox.Text.Length != birthdateTextBox.MaxLength)
			{
				birthdateValidation = false;
			}
			else
			{
				birthdateValidation = true;
			}

			if (numberValidation && birthdateValidation)
			{
				var patient = _mediator.SendSync(new PatientFirstQuery(
						numberTextBox.Text,
						new DateTime(Int32.Parse(birthdateTextBox.Text), 1, 1)
					)
				);

				if (patient != null)
				{
					_mainWindow.NextWorkflow(new ActionsControl(patient));
				}
				else
				{
					numberValidation = false;
					birthdateValidation = false;
				}
			}

			numberTextBox.Foreground = numberValidation ? Brushes.Black : Brushes.Red;
			birthdateTextBox.Foreground = birthdateValidation ? Brushes.Black : Brushes.Red;
		}
	}
}