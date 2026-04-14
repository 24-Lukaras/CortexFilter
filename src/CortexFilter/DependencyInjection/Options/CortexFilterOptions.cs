using CortexFilter.Engine;
using CortexFilter.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.AccessControl;

namespace CortexFilter.DependencyInjection.Options;

/// <summary>
/// Class used to configure and register instances of <see cref="INaturalLanguageEngine{T}"/>.
/// </summary>
public class CortexFilterOptions
{
    private readonly IServiceCollection _services;
    public CortexFilterOptions(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Specifies OpenAI chat client provider.
    /// </summary>
    /// <typeparam name="T">Implementation of <see cref="IChatClientProvider"/>.</typeparam>
    /// <returns>The original <see cref="CortexFilterOptions"/>.</returns>
    public CortexFilterOptions WithClientProvider<T>() where T : class, IChatClientProvider
    {
        _services.AddScoped<IChatClientProvider, T>();
        return this;
    }

    /// <summary>
    /// Adds implementation of <see cref="INaturalLanguageEngine{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEngine">Implementation of <see cref="INaturalLanguageEngine{TEntity}"/></typeparam>
    /// <typeparam name="TEntity">Type of filtered data.</typeparam>
    /// <param name="engineOptions">Action used for further registration of specific filters.</param>
    /// <returns>The original <see cref="CortexFilterOptions"/>.</returns>
    public CortexFilterOptions AddEngine<TEngine, TEntity>(Action<NaturalLanguageEngineOptions<TEntity>> engineOptions) where TEngine : class, INaturalLanguageEngine<TEntity>
    {
        _services.AddScoped<INaturalLanguageEngine<TEntity>, TEngine>();
        _services.AddScoped<INaturalLanguageEngineProperties<TEntity>, NaturalLanguageEngineProperties<TEntity>>();
        var options = new NaturalLanguageEngineOptions<TEntity>(_services);
        engineOptions(options);
        return this;
    }

    /// <summary>
    /// Registers client provider, engines and filters from assembly of provided type.
    /// </summary>
    /// <typeparam name="T">Assembly marker</typeparam>
    /// <returns>The original <see cref="CortexFilterOptions"/>.</returns>
    public CortexFilterOptions RegisterFromAssembly<T>()
    {
        var assembly = typeof(T).Assembly;
        var registration = new FromAssemblyDependencyRegistration(_services, assembly);
        registration.Register();
        return this;
    }
}
