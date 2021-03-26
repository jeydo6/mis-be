using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Queries;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infoboard.Controls
{
	/// <summary>
	/// Логика взаимодействия для SpecialtiesControl.xaml
	/// </summary>
	public partial class SpecialtiesControl : UserControl
	{
		private readonly IMediator _mediator;

		public SpecialtiesControl()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();

			InitializeComponent();
		}

		private async void UserControl_Loaded(Object sender, RoutedEventArgs e)
		{
			var specialties = await _mediator.Send(
				new SpecialtyListItemsQuery(patient: null)
			);

			list.ItemsSource = specialties;
		}
	}
}
