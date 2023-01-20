﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MIS.Be.Application.ViewModels;
using MIS.Be.Infomat.Windows;

namespace MIS.Be.Infomat.Controls
{
	/// <summary>
	/// Логика взаимодействия для DoctorsControl.xaml
	/// </summary>
	public partial class ResourcesControl : UserControl
	{
		private readonly PatientViewModel _patient;
		private readonly SpecialtyViewModel _specialty;

		private readonly MainWindow _mainWindow;

		internal ResourcesControl()
		{
			throw new ArgumentNullException($"Fields '{nameof(_patient)}', '{nameof(_specialty)}' can't be empty!");
		}

		internal ResourcesControl(PatientViewModel patient, SpecialtyViewModel specialty)
		{
			_patient = patient;
			_specialty = specialty;

			var app = System.Windows.Application.Current as App;

			_mainWindow = app.MainWindow as MainWindow;

			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			header.Content = _specialty.SpecialtyName;
			list.ItemsSource = _specialty.Resources;
		}

		private void ListItemButton_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is Button button && button.DataContext is ResourceViewModel resourceListItem)
			{
				_mainWindow.NextWorkflow(new TimeItemsControl(_patient, resourceListItem));
			}
		}

		private void UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (VisualTreeHelper.GetChild(list, 0) is ScrollViewer scrollViewer)
			{
				scrollViewer.LineUp();
			}
		}

		private void DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (VisualTreeHelper.GetChild(list, 0) is ScrollViewer scrollViewer)
			{
				scrollViewer.LineDown();
			}
		}

		private void PrevButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.PrevWorkflow();
		}
	}
}
