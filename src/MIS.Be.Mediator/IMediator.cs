namespace MIS.Be.Mediator;

public interface IMediator
{
	TResponse Send<TResponse>(IRequest<TResponse> request);

	void Send(IRequest request);
}
