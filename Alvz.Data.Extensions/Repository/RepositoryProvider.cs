using Microsoft.Extensions.DependencyInjection;

namespace Alvz.Data.Extensions.Repository;

internal sealed class RepositoryProvider
{
    private readonly IServiceProvider _serviceProvider;

    internal RepositoryProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TRepository GetRepository<TRepository>()
        where TRepository : IRepository
    {
        return _serviceProvider.GetRequiredService<TRepository>();
    }
}