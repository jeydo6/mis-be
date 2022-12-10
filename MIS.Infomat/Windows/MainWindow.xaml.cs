using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.Queries;
using MIS.Infomat.Controls;
using MIS.Mediator;

namespace MIS.Infomat.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IMediator _mediator;

		private readonly DispatcherTimer _timer;

		private readonly Stack<UserControl> _path;

		private bool _serviceMode = false;

		public MainWindow()
		{
			var app = System.Windows.Application.Current as App;

			_mediator = app.ServiceProvider.GetRequiredService<IMediator>();

			_path = new Stack<UserControl>();

			InitializeComponent();

			_timer = new DispatcherTimer()
			{
				Interval = new TimeSpan(0, 0, 60)
			};
			_timer.Tick += TimerTick;

			TimerTick(this, null);
		}

		public bool IsServiceTime { get; private set; }

		public void ResetTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
				_timer.Start();
			}
		}

		public void PrevWorkflow()
		{
			if (_path.Count > 0)
			{
				_path.Pop();
			}

			UserControl userControl = null;
			if (_path.Count > 0)
			{
				userControl = _path.Peek();
			}

			NextWorkflow(userControl ?? new MainControl(), isRemember: false);
		}

		public void PrevWorkflow<T>() where T : UserControl
		{
			UserControl userControl = null;
			while (_path.Count > 0)
			{
				if (_path.Peek() is T item)
				{
					userControl = item;
					break;
				}
				else
				{
					_path.Pop();
				}
			}

			NextWorkflow(userControl ?? new MainControl(), isRemember: false);
		}

		public void NextWorkflow(UserControl userControl, bool isRemember = true)
		{
			if (userControl != null)
			{
				if (isRemember)
				{
					_path.Push(userControl);
				}

				workflow.Children.Clear();
				workflow.Children.Add(userControl);

				ResetTimer();
			}
		}

		public void MainWorkflow()
		{
			_path.Clear();

			NextWorkflow(new MainControl(), isRemember: false);
		}

		private void TimerTick(object sender, EventArgs e)
		{
			IsServiceTime = _mediator.Send(
				new TimeIsServiceQuery()
			);

			MainWorkflow();
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

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			ResetTimer();
		}
	}
}
