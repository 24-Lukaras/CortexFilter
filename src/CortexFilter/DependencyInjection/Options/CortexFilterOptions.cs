using CortexFilter.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.DependencyInjection.Options;

public class CortexFilterOptions
{
    private readonly IServiceCollection _services;
    public CortexFilterOptions(IServiceCollection services)
    {
        _services = services;
    }

    public CortexFilterOptions WithClientProvider<T>() where T : class, IChatClientProvider
    {
        _services.AddScoped<IChatClientProvider, T>();
        return this;
    }

    public CortexFilterOptions AddEngine<TEngine, TEntity>(Action<NaturalLanguageEngineOptions<TEntity>> engineOptions) where TEngine : class, INaturalLanguageEngine<TEntity>
    {
        _services.AddScoped<INaturalLanguageEngine<TEntity>, TEngine>();
        _services.AddScoped<INaturalLanguageEngineProperties<TEntity>, NaturalLanguageEngineProperties<TEntity>>();
        var options = new NaturalLanguageEngineOptions<TEntity>(_services);
        engineOptions(options);
        return this;
    }
}
