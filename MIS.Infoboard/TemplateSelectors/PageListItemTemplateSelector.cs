using MIS.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infoboard.TemplateSelectors
{
	public class PageListItemTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(Object item, DependencyObject container)
		{
			var app = System.Windows.Application.Current;

			var result = item switch
			{
				String => app.TryFindResource("stringListItemTemplate"),
				ResourceViewModel => app.TryFindResource("resourceListItemTemplate"),
				SpecialtyViewModel => app.TryFindResource("specialtyListItemTemplate"),
				_ => null
			};

			return result as DataTemplate ?? base.SelectTemplate(item, container);
		}
	}
}
