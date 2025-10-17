using CortexFilter.Filters;

namespace CortexFilter.Engine;

internal class NaturalLanguageEngineProperties<T> : INaturalLanguageEngineProperties<T>
{
    public IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }
    public IEnumerable<AmbiguousFilter<T>> AmbiguousFilters { get; }
    public IChatClientProvider ClientProvider { get; }

    public NaturalLanguageEngineProperties(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IEnumerable<AmbiguousFilter<T>> ambiguousFilters,
        IChatClientProvider clientProvider)
    {
        ConcreteFilterFactories = concreteFilterFactories;
        AmbiguousFilters = ambiguousFilters;
        ClientProvider = clientProvider;
    }
}
