using CortexFilter.Filters;

namespace CortexFilter.Engine;

internal class NaturalLanguageEngineProperties<T> : INaturalLanguageEngineProperties<T>
{
    public IEnumerable<IConcreteFilterFactory<T>> ConcreteFilterFactories { get; }

    public IChatClientProvider ClientProvider { get; }

    public NaturalLanguageEngineProperties(IEnumerable<IConcreteFilterFactory<T>> concreteFilterFactories,
        IChatClientProvider clientProvider)
    {
        ConcreteFilterFactories = concreteFilterFactories;
        ClientProvider = clientProvider;

    }
}
