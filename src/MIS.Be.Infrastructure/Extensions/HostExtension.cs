using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MIS.Be.Infrastructure.Extensions;

public static class HostExtension
{
    public static IHost MigrateUp(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        scope.ServiceProvider
            .GetRequiredService<IMigrationRunner>()
            .MigrateUp();

        return host;
    }
}
