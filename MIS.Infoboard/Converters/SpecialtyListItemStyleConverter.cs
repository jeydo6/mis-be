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

using MIS.Application.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MIS.Infoboard.Converters
{
	internal class SpecialtyListItemStyleConverter : IValueConverter
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
			var count = (Int32)specialty.Resources.Average(r => r.Count);
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
