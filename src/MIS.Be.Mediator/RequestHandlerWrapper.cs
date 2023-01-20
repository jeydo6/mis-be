namespace MIS.Be.Mediator;

internal abstract class RequestHandlerWrapperBase : HandlerBase
{
	public abstract void Handle(IRequest request, ServiceFactory serviceFactory);
}

internal abstract class RequestHandlerWrapperBase<TResponse> : HandlerBase
{
	public abstract TResponse Handle(IRequest<TResponse> request, ServiceFactory serviceFactory);
}

internal class RequestHandlerWrapper<TRequest> : RequestHandlerWrapperBase
	where TRequest : IRequest
{
	public override void Handle(IRequest request, ServiceFactory serviceFactory) =>
		GetHandler<IRequestHandler<TRequest>>(serviceFactory).Handle((TRequest)request);
}

internal class RequestHandlerWrapper<TRequest, TResponse> : RequestHandlerWrapperBase<TResponse>
	where TRequest : IRequest<TResponse>
{
	public override TResponse Handle(IRequest<TResponse> request, ServiceFactory serviceFactory) =>
		GetHandler<IRequestHandler<TRequest, TResponse>>(serviceFactory).Handle((TRequest)request);
}
