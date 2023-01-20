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
		foreach (var markerType in handlerAssemblyMarkerTypes)
		{
			var serviceDescriptors = Enumerable
				.Union(
					GetClassesImplementingInterface(markerType.Assembly, typeof(IRequestHandler<>)),
					GetClassesImplementingInterface(markerType.Assembly, typeof(IRequestHandler<,>))
				)
				.Select(s => new ServiceDescriptor(s.ServiceType, s.ImplementationType, lifetime));

			services.TryAdd(serviceDescriptors);
		}

		return services
			.AddTransient<ServiceFactory>(sp => sp.GetRequiredService)
			.AddSingleton<IMediator, Mediator>();
	}

	public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] handlerAssemblyMarkerTypes) =>
		AddMediator(services, ServiceLifetime.Scoped, handlerAssemblyMarkerTypes);

	private static (Type ServiceType, Type ImplementationType)[] GetClassesImplementingInterface(Assembly assembly, Type typeToImplement) =>
		assembly.ExportedTypes
			.Where(type => !type.IsInterface && !type.IsAbstract && type
				.GetInterfaces()
				.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeToImplement)
			)
			.Select(type => (type.GetInterface(typeToImplement.Name)!, type))
			.ToArray();
}
