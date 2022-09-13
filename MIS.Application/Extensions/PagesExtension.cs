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
using System.Collections.Generic;
using System.Linq;
using MIS.Application.ViewModels;

namespace MIS.Application.Extensions
{
	public static class PagesExtension
	{
		public static PageViewModel[] GetPages(this object[] items, double maxHeight, int itemHeight, int headerHeight = 0)
		{
			if (headerHeight + itemHeight > maxHeight)
			{
				return Array.Empty<PageViewModel>();
			}

			var stack = new Stack<object>(items.Reverse());
			var pages = new List<PageViewModel>();

			var page = new PageViewModel();
			var pageHeight = 0;

			while (stack.Count > 0)
			{
				if (pageHeight + headerHeight + itemHeight > maxHeight)
				{
					pages.Add(page);

					page = new PageViewModel();
					pageHeight = 0;
				}

				var length = (int)(maxHeight - pageHeight - headerHeight) / itemHeight;

				var item = stack.Pop();
				(object current, int currentLength, object next) = item switch
				{
					SpecialtyViewModel source => Separate(source, length),
					DepartmentViewModel source => Separate(source, length),
					_ => (null, 0, null)
				};

				if (current != null)
				{
					page.Objects.Add(current);
					pageHeight += headerHeight + itemHeight * currentLength;
				}

				if (next != null)
				{
					stack.Push(next);
				}
			}

			if (page.Objects.Count > 0)
			{
				pages.Add(page);
			}

			return pages
				.ToArray();
		}

		private static (object current, int currentLength, object next) Separate(SpecialtyViewModel source, int length)
		{
			if (source.Resources.Length > length)
			{
				SpecialtyViewModel current = new SpecialtyViewModel
				{
					SpecialtyID = source.SpecialtyID,
					SpecialtyName = source.SpecialtyName,
					IsEnabled = source.IsEnabled,
					Count = source.Count,
					Resources = source.Resources[..length]
				};

				SpecialtyViewModel next = new SpecialtyViewModel
				{
					SpecialtyID = source.SpecialtyID,
					SpecialtyName = source.SpecialtyName,
					IsEnabled = source.IsEnabled,
					Count = source.Count,
					Resources = source.Resources[length..]
				};

				return (current, current.Resources.Length, next);
			}

			return (source, source.Resources.Length, null);
		}

		private static (object current, int currentLength, object next) Separate(DepartmentViewModel source, int length)
		{
			if (source.Employees.Length > length)
			{
				DepartmentViewModel current = new DepartmentViewModel
				{
					DepartmentName = source.DepartmentName,
					Employees = source.Employees[..length]
				};

				DepartmentViewModel next = new DepartmentViewModel
				{
					DepartmentName = source.DepartmentName,
					Employees = source.Employees[length..]
				};

				return (current, current.Employees.Length, next);
			}

			return (source, source.Employees.Length, null);
		}
	}
}
