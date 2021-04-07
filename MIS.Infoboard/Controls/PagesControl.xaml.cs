#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
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
