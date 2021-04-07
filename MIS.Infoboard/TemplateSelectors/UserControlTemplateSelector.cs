#region Copyright © 2020-2021 Vladimir Deryagin. All rights reserved
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
using System.Windows;
using System.Windows.Controls;

namespace MIS.Infoboard.TemplateSelectors
{
	internal class UserControlTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(Object item, DependencyObject container)
		{
			var app = System.Windows.Application.Current;

			var result = item switch
			{
				String => app.TryFindResource("stringListItemTemplate"),
				PageViewModel => app.TryFindResource("pageListItemTemplate"),
				_ => null
			};

			return result as DataTemplate ?? base.SelectTemplate(item, container);
		}
	}
}
