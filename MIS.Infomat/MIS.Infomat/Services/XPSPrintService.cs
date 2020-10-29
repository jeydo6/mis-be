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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;

namespace MIS.Infomat.Services
{
    internal class XPSPrintService : IPrintService
    {
        public async Task Print(Object obj)
        {
            if (obj is UserControl userControl)
            {
                using (LocalPrintServer ps = new LocalPrintServer())
                {
                    PrintQueue pq = ps.DefaultPrintQueue;
                    PageMediaSize pageMediaSize = pq.UserPrintTicket.PageMediaSize;

                    FixedDocument doc = new FixedDocument();

                    if (pageMediaSize.Width.HasValue && pageMediaSize.Height.HasValue)
                    {
                        doc.DocumentPaginator.PageSize = new Size(pageMediaSize.Width.Value, pageMediaSize.Height.Value);
                    }

                    FixedPage page = new FixedPage()
                    {
                        Width = doc.DocumentPaginator.PageSize.Width,
                        Height = doc.DocumentPaginator.PageSize.Height,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    if (userControl.Content is Viewbox viewBox)
                    {
                        viewBox.Width = page.Width;
                    }

                    page.Children.Add(userControl);

                    doc.Pages.Add(new PageContent
                    {
                        Child = page
                    });

                    XpsDocumentWriter xpsdw = PrintQueue.CreateXpsDocumentWriter(pq);
                    xpsdw.Write(doc.DocumentPaginator);
                }
            }

            await Task.CompletedTask;
        }
    }
}
