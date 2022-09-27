using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MIS.Mediator;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMediator(this IServiceCollection services, ServiceLifetime lifetime, params Type[] handlerAssemblyMarkerTypes)
	{
		var handlerDetails = new Dictionary<Type, Type?>();

		foreach (var markerType in handlerAssemblyMarkerTypes)
		{
			var handlers = GetClassesImplementingInterface(markerType.Assembly, typeof(IRequestHandler<>));

			foreach (var r in GetClassesImplementingInterface(markerType.Assembly, typeof(IRequest<>)))
			{
				handlerDetails[r] = handlers.SingleOrDefault(h => r == h.GetInterface("IRequestHandler`2")!.GetGenericArguments()[0]);
			}

			foreach (var r in GetClassesImplementingInterface(markerType.Assembly, typeof(IRequest)))
			{
				handlerDetails[r] = handlers.SingleOrDefault(h => r == h.GetInterface("IRequestHandler`1")!.GetGenericArguments()[0]);
			}

			var serviceDescriptors = handlers.Select(h => new ServiceDescriptor(h, h, lifetime));
			services.TryAdd(serviceDescriptors);
		}

		return services
			.AddSingleton<IMediator>(sp => new Mediator(sp, handlerDetails));
	}

	public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] handlerAssemblyMarkerTypes) =>
		AddMediator(services, ServiceLifetime.Scoped, handlerAssemblyMarkerTypes);

	private static Type[] GetClassesImplementingInterface(Assembly assembly, Type typeToImplement)
	{
		return assembly.ExportedTypes
			.Where(type => !type.IsInterface && !type.IsAbstract && type
				.GetInterfaces()
				.Any(i =>
					i.IsGenericType && i.GetGenericTypeDefinition() == typeToImplement ||
					i.IsAssignableFrom(typeToImplement)
				)
			)
			.ToArray();
	}
}
