using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MIS.Infomat.Converters
{
	internal class DateToBrushConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			DateTime date;
			try
			{
				date = (DateTime)values[0];
			}
			catch
			{
				date = new DateTime();
			}

			bool isEnabled;
			try
			{
				isEnabled = (bool)values[1];
			}
			catch
			{
				isEnabled = true;
			}

			if (date != new DateTime() && isEnabled)
			{
				return date.Month % 2 == 0 ? Brushes.Blue : Brushes.Green;
			}

			return Brushes.DarkGray;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return Array.Empty<DependencyProperty>();
		}
	}
}
