using CortexFilter.Filters;

namespace CortexFilter.Engine;

/// <summary>
/// Bundle of services used by <see cref="NaturalLanguageEngine{T}"/>.
/// </summary>
/// <typeparam name="T">Type of data filtered by <see cref="NaturalLanguageEngine{T}"/>.</typeparam>
internal class NaturalLanguageEngineProperties<T> : INaturalLanguageEngineProperties<T>
{
    /// <inheritdoc/>
    public IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }
    /// <inheritdoc/>
    public IEnumerable<AmbiguousFilter<T>> AmbiguousFilters { get; }
    /// <inheritdoc/>
    public IEnumerable<ICortexResource<T>> Resources { get; }
    /// <inheritdoc/>
    public IChatClientProvider ClientProvider { get; }
    /// <inheritdoc/>
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
