using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MIS.Be.Tests.Factories;

public interface IApplicationFactory
{
	IHost CreateHost();

	IHost CreateHost(Action<IServiceCollection> configuration);
}
