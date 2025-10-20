using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.Engine;

internal class Cortex : ICortex
{
    private readonly IServiceProvider _services;
    public Cortex(IServiceProvider services)
    {
        _services = services;
    }

    public Task<IEnumerable<T>> Search<T>(string query)
    {
        var engine = _services.GetService<INaturalLanguageEngine<T>>();

        if (engine is null)
            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());

        return engine.SearchAsync(query);
    }
}
