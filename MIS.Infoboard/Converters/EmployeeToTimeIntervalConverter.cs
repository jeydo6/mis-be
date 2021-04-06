using MIS.Application.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MIS.Infoboard.Converters
{
	internal class EmployeeToTimeIntervalConverter : IValueConverter
	{
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			if (value is not EmployeeViewModel employee || employee == null)
			{
				return "нет приёма";
			}

			var result = $"{employee.BeginTime:H:mm} - {employee.EndTime:H:mm}";
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
