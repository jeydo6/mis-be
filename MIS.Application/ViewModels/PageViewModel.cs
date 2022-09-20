using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
	public class PageViewModel
	{
		public PageViewModel()
		{
			Objects = Array.Empty<object>();
		}

		public IEnumerable<object> Objects { get; set; }
	}
}
