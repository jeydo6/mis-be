using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MIS.Domain.Services;

namespace MIS.Infomat.Services
{
	internal class XPSPrintService : IPrintService
	{
		public void Print(IPrintForm printForm)
		{
			if (printForm is UserControl userControl)
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
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					};

					page.Children.Add(userControl);

					document.Pages.Add(new PageContent
					{
						Child = page
					});

					PrintQueue
						.CreateXpsDocumentWriter(pq)
						.WriteAsync(document);
				}
			}
		}
	}
}
