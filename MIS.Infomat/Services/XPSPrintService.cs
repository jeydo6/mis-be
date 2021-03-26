#region Copyright © 2020 Vladimir Deryagin. All rights reserved
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

using MIS.Domain.Services;
using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MIS.Infomat.Services
{
	internal class XPSPrintService : IPrintService
	{
		public void Print(Object obj)
		{
			if (obj is UserControl userControl)
			{
				using (var ps = new LocalPrintServer())
				{
					var pq = ps.DefaultPrintQueue;
					var pageMediaSize = pq.UserPrintTicket.PageMediaSize;

					var document = new FixedDocument();

					if (pageMediaSize.Width.HasValue && pageMediaSize.Height.HasValue)
					{
						document.DocumentPaginator.PageSize = new Size(pageMediaSize.Width.Value, pageMediaSize.Height.Value);
					}

					if (userControl.Content is Viewbox viewBox)
					{
						viewBox.Width = document.DocumentPaginator.PageSize.Width;
					}

					var page = new FixedPage
					{
						Width = document.DocumentPaginator.PageSize.Width,
						Height = document.DocumentPaginator.PageSize.Height,
						HorizontalAlignment = HorizontalAlignment.Center
					};

					page.Children.Add(userControl);

					document.Pages.Add(new PageContent
					{
						Child = page
					});

					var xpsdw = PrintQueue.CreateXpsDocumentWriter(pq);
					xpsdw.Write(document.DocumentPaginator);
				}
			}
		}
	}
}
