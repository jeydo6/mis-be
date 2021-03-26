using MIS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Application.Pagination
{
	public static class Pagination
	{
		public static PageViewModel[] GetPages<T>(this T[] items, Double maxHeight, Int32 itemHeight, Int32 headerHeight = 0) where T : IPaginable<T>
		{
			if (headerHeight + itemHeight > maxHeight)
			{
				return Array.Empty<PageViewModel>();
			}

			var stack = new Stack<T>(items.Reverse());
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

				var length = (Int32)(maxHeight - pageHeight - headerHeight) / itemHeight;

				var item = stack.Pop();
				(T current, T next) = item.Paginate(ref length);

				page.Objects.Add(current);
				pageHeight += headerHeight + itemHeight * length;

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
	}
}
