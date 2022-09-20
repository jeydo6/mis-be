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
using System.Collections.Generic;
using System.Linq;
using MIS.Application.ViewModels;

namespace MIS.Application.Extensions
{
	public static class PagesExtension
	{
		public static IEnumerable<PageViewModel> GetPages<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> groups, int maxHeight, int itemHeight, int headerHeight = 0)
		{
			return groups
				.GroupBy(maxHeight, itemHeight, headerHeight)
				.Select(p => new PageViewModel
				{
					Objects = p.Select(g => g switch
					{
						IGrouping<DepartmentViewModel, EmployeeViewModel> d => new DepartmentViewModel
						{
							DepartmentName = d.Key.DepartmentName,
							Employees = d.ToArray()
						},
						IGrouping<SpecialtyViewModel, ResourceViewModel> s => new SpecialtyViewModel
						{
							SpecialtyID = s.Key.SpecialtyID,
							SpecialtyName = s.Key.SpecialtyName,
							IsEnabled = s.Key.IsEnabled,
							Count = s.Key.Count,
							Resources = s.ToArray()
						},
						_ => default(object)
					})
				})
				.ToArray();
		}

		private static IEnumerable<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> groups, int maxHeight, int itemHeight, int headerHeight = 0)
		{
			var template = groups
				.Select(g => g.Count())
				.GetTemplate(maxHeight, itemHeight, headerHeight);

			return groups
				.GroupBy(template.SelectMany(t => t))
				.GroupBy(template);
		}

		private static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> groups, IEnumerable<int> template)
		{
			var result = new List<IGrouping<TKey, TElement>>();

			var items = groups
				.SelectMany(g => g.Select(item => (g.Key, Item: item)))
				.ToArray();

			var offset = 0;
			foreach (var length in template)
			{
				result.AddRange(
					items
						.Skip(offset)
						.Take(length)
						.GroupBy(t => t.Key, t => t.Item)
				);

				offset += length;
			}

			return result.ToArray();
		}

		private static IEnumerable<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> groups, IEnumerable<IEnumerable<int>> template)
		{
			var result = new List<IEnumerable<IGrouping<TKey, TElement>>>();

			var offset = 0;
			foreach (var length in template.Select(t => t.Count()))
			{
				result.Add(
					groups
						.Skip(offset)
						.Take(length)
				);

				offset += length;
			}

			return result.ToArray();
		}

		private static IEnumerable<IEnumerable<int>> GetTemplate(this IEnumerable<int> counts, int maxHeight, int itemHeight, int headerHeight = 0)
		{
			var result = new List<IEnumerable<int>>();

			var stack = new Stack<int>(
				counts.Reverse()
			);

			var current = new List<int>();
			var currentHeight = 0;
			while (stack.Count > 0)
			{
				var maxLength = (maxHeight - headerHeight - currentHeight) / itemHeight;
				if (maxLength == 0)
				{
					return Array.Empty<IEnumerable<int>>();
				}

				var length = stack.Pop();
				if (length > maxLength)
				{
					current.Add(maxLength);
					result.Add(current);
					stack.Push(length - maxLength);

					current = new List<int>();
					currentHeight = 0;
				}
				else
				{
					current.Add(length);
					currentHeight += headerHeight + itemHeight * length;
				}
			}

			if (current.Count > 0)
			{
				result.Add(current);
			}

			return result.ToArray();
		}


	}
}
