using System;
using System.Collections.Generic;
using System.Linq;
using MIS.Application.ViewModels;

namespace MIS.Application.Extensions
{
	public static class GroupingExtension
	{
		public static IEnumerable<IEnumerable<DepartmentViewModel>> GroupBy(
			this DepartmentViewModel[] items, int maxHeight, int itemHeight, int headerHeight = 0)
		{
			var template = items
				.Select(i => i.Employees.Length)
				.GetTemplate(maxHeight, itemHeight, headerHeight)
				.ToArray();

			return items
				.SelectMany(key => key.Employees.Select(element => (key, element)))
				.GroupBy(template.SelectMany(t => t))
				.GroupBy(template)
				.Select(groups => groups.Select(g => new DepartmentViewModel
				{
					DepartmentName = g.Key.DepartmentName,
					Employees = g.ToArray()
				}))
				.ToArray();
		}

		public static IEnumerable<IEnumerable<SpecialtyViewModel>> GroupBy(
			this SpecialtyViewModel[] items, int maxHeight, int itemHeight, int headerHeight = 0)
		{
			var template = items
				.Select(i => i.Resources.Length)
				.GetTemplate(maxHeight, itemHeight, headerHeight)
				.ToArray();

			return items
				.SelectMany(key => key.Resources.Select(element => (key, element)))
				.GroupBy(template.SelectMany(t => t))
				.GroupBy(template)
				.Select(groups => groups.Select(g => new SpecialtyViewModel
				{
					SpecialtyID = g.Key.SpecialtyID,
					SpecialtyName = g.Key.SpecialtyName,
					IsEnabled = g.Key.IsEnabled,
					Count = g.Key.Count,
					Resources = g.ToArray()
				}))
				.ToArray();
		}

		private static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(
			this IEnumerable<(TKey Key, TElement Element)> items, IEnumerable<int> template)
		{
			var result = new List<IGrouping<TKey, TElement>>();

			var offset = 0;
			var itemsArray = items.ToArray();
			foreach (var length in template)
			{
				result.AddRange(
					itemsArray[offset..(offset + length)]
						.GroupBy(t => t.Key, t => t.Element)
				);

				offset += length;
			}

			return result.ToArray();
		}

		private static IEnumerable<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TKey, TElement>(
			this IEnumerable<IGrouping<TKey, TElement>> items, IEnumerable<IEnumerable<int>> template)
		{
			var result = new List<IEnumerable<IGrouping<TKey, TElement>>>();

			var offset = 0;
			var itemsArray = items.ToArray();
			foreach (var length in template.Select(t => t.Count()))
			{
				result.Add(
					itemsArray[offset..(offset + length)]
				);

				offset += length;
			}

			return result.ToArray();
		}

		private static IEnumerable<IEnumerable<int>> GetTemplate(
			this IEnumerable<int> counts, int maxHeight, int itemHeight, int headerHeight = 0)
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

				var length = stack.Pop();
				if (length > maxLength)
				{
					if (maxLength > 0)
						current.Add(maxLength);
					
					stack.Push(length - maxLength);

					result.Add(current);
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
