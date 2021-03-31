using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Extensions;
using MIS.Application.Queries;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infoboard.Controls
{
	/// <summary>
	/// Логика взаимодействия для SliderControl.xaml
	/// </summary>
	public partial class SliderControl : UserControl
	{
		private readonly IMediator _mediator;

		private readonly Func<Task>[] _actions;

		private Int32 _index;

		public SliderControl()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetService<IMediator>();
			_actions = new Func<Task>[]
			{
				LoadSpecialtiesPages
			};

			_index = -1;

			InitializeComponent();
		}

		private void UserControl_Loaded(Object sender, RoutedEventArgs e)
		{
			MoveNext(sender, e);
		}

		private void Pages_Done(Object sender, RoutedEventArgs e)
		{
			MoveNext(sender, e);
		}

		private async void MoveNext(Object sender, EventArgs e)
		{
			if (_actions == null || _actions.Length == 0)
			{
				return;
			}

			if (_index >= _actions.Length - 1)
			{
				_index = -1;
			}

			await _actions[++_index]();
		}

		private async Task LoadSpecialtiesPages()
		{
			var specialties = await _mediator.Send(
				new SpecialtyListItemsQuery(patient: null)
			);

			var actualHeight = ActualHeight;
			var itemHeight = 80;
			var headerHeight = 90 + 20;

			pages.Content = "Расписание приёма врачей";
			pages.Items = specialties.GetPages(actualHeight, itemHeight, headerHeight);
		}
	}
}
