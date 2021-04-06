using MIS.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infoboard.TemplateSelectors
{
	internal class PageContentTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(Object item, DependencyObject container)
		{
			var app = System.Windows.Application.Current;

			var result = item switch
			{
				SpecialtyViewModel => app.TryFindResource("specialtyListItemTemplate"),
				DepartmentViewModel => app.TryFindResource("departmentListItemTemplate"),
				_ => null
			};

			return result as DataTemplate ?? base.SelectTemplate(item, container);
		}
	}
}
