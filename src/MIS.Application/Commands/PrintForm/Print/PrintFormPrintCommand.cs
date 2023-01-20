using MIS.Domain.Services;
using MIS.Mediator;

namespace MIS.Application.Commands;

public class PrintFormPrintCommand : IRequest
{
	public PrintFormPrintCommand(IPrintForm printForm)
	{
		PrintForm = printForm;
	}

	public IPrintForm PrintForm { get; }
}
