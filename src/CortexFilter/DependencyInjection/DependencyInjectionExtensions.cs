using CortexFilter.DependencyInjection.Options;
using Microsoft.Extensions.DependencyInjection;

namespace CortexFilter.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCortexFilter(this IServiceCollection services, Action<CortexFilterOptions> options)
    {
        var optionsInstace = new CortexFilterOptions(services);
        options(optionsInstace);
        return services;
    }
}