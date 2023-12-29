using Alvz.Data.Extensions.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Alvz.Data.Extensions.DependencyInjection;

public static class DatabaseRegistration
{
    public static IServiceCollection RegisterMonitoredDatabase(this IServiceCollection services)
    {
        return services
            .RegisterDatabaseServices()
            .AddScoped<IDbConnection>(s =>
            {
                //todo
                throw new NotImplementedException();
            })
            .AddScoped<IDatabaseManager, DatabaseManager>();
    }


    private static IServiceCollection RegisterDatabaseServices(this IServiceCollection services)
    {
        return services
                .AddTransient<IMonitoredDbConnectionBuilder, MonitoredDbConnectionBuilder>()
                .AddTransient<RepositoryProvider>();
    }

}
