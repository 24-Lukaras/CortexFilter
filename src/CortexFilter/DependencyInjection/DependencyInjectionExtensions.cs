using CortexFilter.DependencyInjection.Options;
using CortexFilter.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.DependencyInjection;

/// <summary>
/// Extensions class used for registration of CortexFilter services in IoC container.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds CortexFilter services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="options">Action used for further registration of <see cref="NaturalLanguageEngine{T}"/> or <see cref="IChatClientProvider"/>.</param>
    /// <returns>The original <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCortexFilter(this IServiceCollection services, Action<CortexFilterOptions> options)
    {
        services.AddScoped<ICortex, Cortex>();
        var optionsInstace = new CortexFilterOptions(services);
        options(optionsInstace);
        return services;
    }
}