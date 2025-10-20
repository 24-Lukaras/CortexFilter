using CortexFilter.Engine;
using CortexFilter.Filters;

namespace CortexFilter;

public interface INaturalLanguageEngineProperties<T>
{
    internal IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }
    internal IEnumerable<AmbiguousFilter<T>> AmbiguousFilters { get; }
    internal IEnumerable<ICortexResource<T>> Resources { get; }
    internal IChatClientProvider ClientProvider { get; }
    internal ICortex Cortex { get; }
}
