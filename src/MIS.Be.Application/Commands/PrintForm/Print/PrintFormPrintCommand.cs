using MIS.Be.Domain.Services;
using MIS.Be.Mediator;

namespace MIS.Be.Application.Commands;

public class PrintFormPrintCommand : IRequest
{
	public PrintFormPrintCommand(IPrintForm printForm)
	{
		PrintForm = printForm;
	}

	public IPrintForm PrintForm { get; }
}
