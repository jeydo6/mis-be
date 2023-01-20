using MIS.Be.Domain.Services;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Commands.PrintForm.Print;

public class PrintFormPrintHandler : IRequestHandler<PrintFormPrintCommand>
{
	private readonly IPrintService _printService;

	public PrintFormPrintHandler(IPrintService printService)
	{
		_printService = printService;
	}

	public void Handle(PrintFormPrintCommand request)
	{
		_printService.Print(request.PrintForm);
	}
}
