#region Copyright © 2018-2022 Vladimir Deryagin. All rights reserved
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
using System.Windows;
using System.Windows.Data;
using MIS.Application.ViewModels;

namespace MIS.Infoboard.Converters
{
	internal class ResourceListItemStyleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not ResourceViewModel resource)
			{
				return DependencyProperty.UnsetValue;
			}

			if (parameter is not string option || string.IsNullOrEmpty(option))
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

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
