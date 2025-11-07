using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.Engine;

/// <summary>
/// Mediator service for <see cref="Filters.CortexResource{T,E}"/> to filter from multiple search engines.
/// </summary>
internal class Cortex : ICortex
{
    private readonly IServiceProvider _services;
    public Cortex(IServiceProvider services)
    {
        _services = services;
    }

    ///<inheritdoc/>
    public Task<IEnumerable<T>> SearchAsync<T>(string query)
    {
        var engine = _services.GetService<INaturalLanguageEngine<T>>();

        if (engine is null)
            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());

        return engine.SearchAsync(query);
    }
}
