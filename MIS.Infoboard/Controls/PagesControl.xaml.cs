using MIS.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MIS.Infoboard.Controls
{
	/// <summary>
	/// Логика взаимодействия для PagesControl.xaml
	/// </summary>
	public partial class PagesControl : UserControl
	{
		private static readonly RoutedEvent _doneEvent = EventManager.RegisterRoutedEvent("Done", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(PagesControl));

		private readonly DispatcherTimer _timer;

		private PageViewModel[] _items;
		private Int32 _index;

		public PagesControl()
		{
			_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromTicks(Int32.MaxValue)
			};
			_timer.Tick += MoveNext;
			_timer.Start();

			InitializeComponent();
		}

		public PageViewModel[] Items
		{
			get
			{
				return _items;
			}
			set
			{
				_items = value;
				_index = -1;
				_timer.Interval = new TimeSpan(0, 0, 3);
			}
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

		private void UserControl_MouseUp(Object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				MoveNext(this, e);
			}
		}

		private void MoveNext(Object sender, EventArgs e)
		{
			if (_items == null || _items.Length == 0)
			{
				RaiseEvent(new RoutedEventArgs(_doneEvent));
				return;
			}

			if (_index >= _items.Length - 1)
			{
				RaiseEvent(new RoutedEventArgs(_doneEvent));
				return;
			}

			if (_index == -1)
			{
				_timer.Interval = new TimeSpan(0, 0, 12);
			}

			Content = _items[++_index];
		}
	}
}
