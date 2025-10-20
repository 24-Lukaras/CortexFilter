using CortexFilter.Filters;

namespace CortexFilter.Engine;

internal class NaturalLanguageEngineProperties<T> : INaturalLanguageEngineProperties<T>
{
    public IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }
    public IEnumerable<AmbiguousFilter<T>> AmbiguousFilters { get; }
    public IEnumerable<ICortexResource<T>> Resources { get; }
    public IChatClientProvider ClientProvider { get; }
    public ICortex Cortex { get; }

    public NaturalLanguageEngineProperties(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IEnumerable<AmbiguousFilter<T>> ambiguousFilters,
        IEnumerable<ICortexResource<T>> resources,
        IChatClientProvider clientProvider,
        ICortex cortex)
    {
        ConcreteFilterFactories = concreteFilterFactories;
        AmbiguousFilters = ambiguousFilters;
        Resources = resources;
        ClientProvider = clientProvider;
        Cortex = cortex;
    }
}
