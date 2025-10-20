using CortexFilter.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.DependencyInjection.Options;

public class NaturalLanguageEngineOptions<T>
{
    private readonly IServiceCollection _services;
    public NaturalLanguageEngineOptions(IServiceCollection services)
    {
        _services = services;
    }

    public NaturalLanguageEngineOptions<T> AddConcreteFilterFactory<TFilterFactory>() where TFilterFactory : class, IConcreteFilterFactory<T>
    {
        _services.AddScoped<IConcreteFilterFactory<T>, TFilterFactory>();
        return this;
    }

    public NaturalLanguageEngineOptions<T> AddAmbiguousFilter<TFilter>() where TFilter : AmbiguousFilter<T>
    {
        _services.AddScoped<AmbiguousFilter<T>, TFilter>();
        return this;
    }

    public NaturalLanguageEngineOptions<T> AddResource<TFilter, TResource>() where TFilter : CortexResource<T, TResource>
    {
        _services.AddScoped<ICortexResource<T>, TFilter>();
        return this;
    }
}
