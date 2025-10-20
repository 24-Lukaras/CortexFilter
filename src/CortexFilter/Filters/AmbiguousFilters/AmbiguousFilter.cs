
using OpenAI.Chat;

namespace CortexFilter.Filters;

public abstract class AmbiguousFilter<T> : ICollectionFilter<T>, IFilterInitializer<T>
{
    public abstract string Name { get; }
    public abstract string? Description { get; }

    public abstract IEnumerable<T> Filter(IEnumerable<T> collection);

    public Task InitAsync(FilterInitializerProperties<T> properties) =>
        InitInternalAsync(properties.Query, properties.Client, properties.Collection);

    protected abstract Task InitInternalAsync(string query, ChatClient client, IEnumerable<T> collection);
}
