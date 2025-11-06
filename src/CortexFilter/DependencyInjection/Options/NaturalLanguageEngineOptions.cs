using CortexFilter.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.DependencyInjection.Options;

/// <summary>
/// Class used to configure <see cref="NaturalLanguageEngine{T}"/>.
/// </summary>
/// <typeparam name="T">Type of data filtered by <see cref="NaturalLanguageEngine{T}"/>.</typeparam>
public class NaturalLanguageEngineOptions<T>
{
    private readonly IServiceCollection _services;
    public NaturalLanguageEngineOptions(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Adds a filter factory for creation of filters.
    /// </summary>
    /// <typeparam name="TFilterFactory">Implementation of <see cref="IConcreteFilterFactory{T}"/>.</typeparam>
    /// <returns>The original <see cref="NaturalLanguageEngineOptions{T}"/>.</returns>
    public NaturalLanguageEngineOptions<T> AddConcreteFilterFactory<TFilterFactory>() where TFilterFactory : class, IConcreteFilterFactory<T>
    {
        _services.AddScoped<IConcreteFilterFactory<T>, TFilterFactory>();
        return this;
    }

    /// <summary>
    /// Adds a filter that requires further evaluation from the LLM.
    /// </summary>
    /// <typeparam name="TFilter">Implementation of <see cref="AmbiguousFilter{T}"/>.</typeparam>
    /// <returns>The original <see cref="NaturalLanguageEngineOptions{T}"/>.</returns>
    public NaturalLanguageEngineOptions<T> AddAmbiguousFilter<TFilter>() where TFilter : AmbiguousFilter<T>
    {
        _services.AddScoped<AmbiguousFilter<T>, TFilter>();
        return this;
    }

    /// <summary>
    /// Adds a resource for filtering from different <see cref="NaturalLanguageEngine{T}"/>.
    /// </summary>
    /// <typeparam name="TFilter">Implementation of <see cref="CortexResource{TSource, TResult}"/>.</typeparam>
    /// <typeparam name="TResource">Type of resource.</typeparam>
    /// <returns>The original <see cref="NaturalLanguageEngineOptions{T}"/>.</returns>
    public NaturalLanguageEngineOptions<T> AddResource<TFilter, TResource>() where TFilter : CortexResource<T, TResource>
    {
        _services.AddScoped<ICortexResource<T>, TFilter>();
        return this;
    }
}
