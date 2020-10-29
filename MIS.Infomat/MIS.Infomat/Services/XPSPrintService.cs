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
