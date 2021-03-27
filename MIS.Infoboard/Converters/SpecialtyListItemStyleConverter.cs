using MIS.Application.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MIS.Infoboard.Converters
{
	public class SpecialtyListItemStyleConverter : IValueConverter
	{
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			if (value is not SpecialtyViewModel specialty)
			{
				return DependencyProperty.UnsetValue;
			}

			if (parameter is not String option || String.IsNullOrEmpty(option))
			{
				return DependencyProperty.UnsetValue;
			}

			if (specialty.Resources == null || specialty.Resources.Length == 0)
			{
				return DependencyProperty.UnsetValue;
			}

			var resourceKey = (specialty.Count / specialty.Resources.Length) switch
			{
				var count when count > 50 => "success",
				var count when count > 25 => "warning",
				var count when count > 0 => "danger",
				var count when count == 0 => "secondary",
				_ => "default"
			};

			var result = System.Windows.Application.Current.TryFindResource($"{resourceKey}{option}ListItem");

			return result;
		}

		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
