using CortexFilter.Engine;
using CortexFilter.Filters;

namespace CortexFilter;

public interface INaturalLanguageEngineProperties<T>
{
    public IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }
    public IEnumerable<AmbiguousFilter<T>> AmbiguousFilters { get; }
    public IChatClientProvider ClientProvider { get; }
}
