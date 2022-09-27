namespace MIS.Mediator;

internal abstract class RequestHandlerWrapper : HandlerBase
{
	public abstract void Handle(IRequest request, ServiceFactory serviceFactory);
}

internal abstract class RequestHandlerWrapper<TResponse> : HandlerBase
{
	public abstract TResponse Handle(IRequest<TResponse> request, ServiceFactory serviceFactory);
}

internal class RequestHandlerWrapperImpl<TRequest> : RequestHandlerWrapper
	where TRequest : IRequest
{
	public override void Handle(IRequest request, ServiceFactory serviceFactory) =>
		GetHandler<IRequestHandler<TRequest>>(serviceFactory).Handle((TRequest)request);
}

internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
	where TRequest : IRequest<TResponse>
{
	public override TResponse Handle(IRequest<TResponse> request, ServiceFactory serviceFactory) =>
		GetHandler<IRequestHandler<TRequest, TResponse>>(serviceFactory).Handle((TRequest)request);
}
