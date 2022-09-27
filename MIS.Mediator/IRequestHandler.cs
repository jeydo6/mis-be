namespace MIS.Mediator;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	TResponse Handle(TRequest request);
}

public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
	void Handle(TRequest request);
}