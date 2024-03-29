﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using MIS.Be.Application.Queries;
using MIS.Be.Infomat.Windows;
using MIS.Be.Mediator;

namespace MIS.Be.Infomat.Controls
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

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			numberTextBox.Focus();
		}

		private void InputButton_Click(object sender, RoutedEventArgs e)
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

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
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

		private void PrevButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.PrevWorkflow();
		}

		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			bool numberValidation;
			if (string.IsNullOrEmpty(numberTextBox.Text))
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

			bool birthdateValidation;
			if (string.IsNullOrEmpty(birthdateTextBox.Text))
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
				var patient = _mediator.Send(new PatientFirstQuery(
						numberTextBox.Text,
						new DateTime(int.Parse(birthdateTextBox.Text), 1, 1)
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
