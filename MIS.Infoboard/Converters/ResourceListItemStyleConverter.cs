using MIS.Application.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MIS.Infoboard.Converters
{
	internal class ResourceListItemStyleConverter : IValueConverter
	{
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			if (value is not ResourceViewModel resource)
			{
				return DependencyProperty.UnsetValue;
			}

			if (parameter is not String option || String.IsNullOrEmpty(option))
			{
				return DependencyProperty.UnsetValue;
			}

			var count = resource.Count;
			var resourceKey = count switch
			{
				> 50 => "success",
				> 25 => "warning",
				> 0 => "danger",
				0 => "secondary",
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
