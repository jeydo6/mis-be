using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Pagination;
using MIS.Application.Queries;
using MIS.Application.ViewModels;
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
		private static readonly RoutedEvent _doneEvent = EventManager.RegisterRoutedEvent("Done", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SpecialtiesControl));

		private static readonly Int32 _headerHeight = 90 + 20;
		private static readonly Int32 _itemHeight = 80;

		private readonly IMediator _mediator;

		private PageViewModel[] _pages;
		private Int32 _pageIndex;

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

			_pages = specialties.GetPages(ActualHeight, _itemHeight, _headerHeight);
			_pageIndex = -1;

			NextPage();
		}

		private void UserControl_KeyUp(Object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Right)
			{
				NextPage();
			}

			if (e.Key == System.Windows.Input.Key.Left)
			{
				PrevPage();
			}
		}

		private void NextPage()
		{
			if (_pages.Length == 0)
			{
				RaiseEvent(new RoutedEventArgs(_doneEvent));
				return;
			}

			if (_pageIndex >= _pages.Length - 1)
			{
				RaiseEvent(new RoutedEventArgs(_doneEvent));
				_pageIndex = -1;
			}

			var page = _pages[++_pageIndex];
			list.ItemsSource = page.Objects;
		}

		private void PrevPage()
		{
			if (_pages.Length == 0)
			{
				return;
			}

			if (_pageIndex <= 0)
			{
				_pageIndex = _pages.Length;
			}

			var page = _pages[--_pageIndex];
			list.ItemsSource = page.Objects;
		}

		public event RoutedEventHandler Done
		{
			add
			{
				AddHandler(_doneEvent, value);
			}
			remove
			{
				RemoveHandler(_doneEvent, value);
			}
		}
	}
}
