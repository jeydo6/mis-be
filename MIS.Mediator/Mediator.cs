using System;
using System.Collections.Generic;

namespace MIS.Mediator;

public class Mediator : IMediator
{
	private readonly IServiceProvider _serviceProvider;
	private readonly Dictionary<Type, Type?> _handlerDetails;

	public Mediator(
		IServiceProvider serviceProvider,
		IDictionary<Type, Type?> handlerDetails)
	{
		_serviceProvider = serviceProvider;
		_handlerDetails = new Dictionary<Type, Type?>(handlerDetails);
	}		

	public TResponse Send<TResponse>(IRequest<TResponse> request)
	{
		var requestType = request.GetType();
		if (!_handlerDetails.ContainsKey(requestType) || _handlerDetails[requestType] is null)
		{
			throw new Exception($"No handler to handle request of type: {requestType.Name}");
		}

		var handler = _serviceProvider.GetService(requestType);
		if (handler is null)
		{
			throw new Exception($"No service to handle request of type: {requestType.Name}");
		}

		return (TResponse)handler
			.GetType()
			.GetMethod("Handle")!
			.Invoke(handler, new object[] { request })!;
	}

	public void Send(IRequest request)
	{
		var requestType = request.GetType();
		if (!_handlerDetails.TryGetValue(requestType, out var requestHandlerType) || requestHandlerType is null)
		{
			throw new Exception($"No handler type to handle request of type: {requestType.Name}");
		}

		var handler = _serviceProvider.GetService(requestHandlerType);
		if (handler is null)
		{
			throw new Exception($"No handler to handle request of type: {requestType.Name}");
		}

		handler
			.GetType()
			.GetMethod("Handle")!
			.Invoke(handler, new object[] { request });
	}
}
