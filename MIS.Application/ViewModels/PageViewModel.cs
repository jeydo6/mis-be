using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
	public class PageViewModel
	{
		public PageViewModel()
		{
			Objects = new List<object>();
		}

		public ICollection<object> Objects { get; set; }
	}
}
