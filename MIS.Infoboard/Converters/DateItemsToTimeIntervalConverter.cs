﻿#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
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

using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;

namespace MIS.Infoboard.Converters
{
	internal class DateItemsToTimeIntervalConverter : IValueConverter
	{
		private readonly IDateTimeProvider _dateTimeProvider;

		public DateItemsToTimeIntervalConverter()
		{
			var app = System.Windows.Application.Current as App;

			_dateTimeProvider = app.ServiceProvider.GetService<IDateTimeProvider>();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not DateItemViewModel[] dates)
			{
				return "нет приёма";
			}

			var date = dates.FirstOrDefault(di => di.Date.Date == _dateTimeProvider.Now.Date);
			if (date == null)
			{
				return "нет приёма";
			}

			var result = $"{date.BeginDateTime:H:mm} - {date.EndDateTime:H:mm}";
			if (string.IsNullOrEmpty(result))
			{
				return "нет приёма";
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
