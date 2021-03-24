using MIS.Domain.Providers;
using System;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using MIS.Application.ViewModels;
using System.Linq;

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

		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			if (value is not IEnumerable<DateItemViewModel> dates)
			{
				return "нет приёма";
			}

			var date = dates.FirstOrDefault(di => di.Date.Date == _dateTimeProvider.Now.Date);
			if (date == null)
			{
				return "нет приёма";
			}

			var result = $"{date.BeginTime:H:mm} - {date.EndTime:H:mm}";
			if (String.IsNullOrEmpty(result))
			{
				return "нет приёма";
			}

			return result;
		}

		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
