using System;
using System.Collections.Generic;

namespace MIS.Mediator;

public class Mediator : IMediator
{
	private readonly ServiceFactory _serviceFactory;
	private readonly IDictionary<Type, HandlerBase> _requestHandlers = new Dictionary<Type, HandlerBase>();

	/// <summary>
	/// Initializes a new instance of the <see cref="Mediator"/> class.
	/// </summary>
	/// <param name="serviceFactory">The single instance factory.</param>
	public Mediator(ServiceFactory serviceFactory)
	{
		_serviceFactory = serviceFactory;
	}

	public TResponse Send<TResponse>(IRequest<TResponse> request)
	{
		if (request == null)
		{
			throw new ArgumentNullException(nameof(request));
		}

		var requestType = request.GetType();

		if (!_requestHandlers.ContainsKey(requestType))
		{
			_requestHandlers[requestType] =
				(HandlerBase)(Activator.CreateInstance(typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse))) ??
				throw new InvalidOperationException($"Could not create wrapper type for {requestType}"));
		}

		var handler = (RequestHandlerWrapperBase<TResponse>)_requestHandlers[requestType];

		return handler.Handle(request, _serviceFactory);
	}

	public void Send(IRequest request)
	{
		if (request == null)
		{
			throw new ArgumentNullException(nameof(request));
		}

		var requestType = request.GetType();

		if (!_requestHandlers.ContainsKey(requestType))
		{
			_requestHandlers[requestType] =
				(HandlerBase)(Activator.CreateInstance(typeof(RequestHandlerWrapper<>).MakeGenericType(requestType)) ??
				throw new InvalidOperationException($"Could not create wrapper type for {requestType}"));
		}

		var handler = (RequestHandlerWrapperBase)_requestHandlers[requestType];

		handler.Handle(request, _serviceFactory);
	}
}
