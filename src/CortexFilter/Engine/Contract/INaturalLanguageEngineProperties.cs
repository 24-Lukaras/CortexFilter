using CortexFilter.Engine;
using CortexFilter.Filters;

namespace CortexFilter;

/// <summary>
/// Interface that bundles items required by <see cref="NaturalLanguageEngine{T}"/>. For internal use only.
/// </summary>
/// <typeparam name="T">Type of data to be filtered by <see cref="NaturalLanguageEngine{T}"/></typeparam>
public interface INaturalLanguageEngineProperties<T>
{
    /// <summary>
    /// Registered <see cref="IConcreteFilterFactory{T}"/> instances.
    /// </summary>
    internal IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }

    /// <summary>
    /// Registered <see cref="AmbiguousFilter{T}"/> instances.
    /// </summary>
    internal IEnumerable<AmbiguousFilter<T>> AmbiguousFilters { get; }

    /// <summary>
    /// Registered <see cref="ICortexResource{T}"/> instances.
    /// </summary>
    internal IEnumerable<ICortexResource<T>> Resources { get; }

    /// <summary>
    /// Custom chat client provider.
    /// </summary>
    internal IChatClientProvider ClientProvider { get; }

    /// <summary>
    /// Cortex instance for resource filtering.
    /// </summary>
    internal ICortex Cortex { get; }
}
