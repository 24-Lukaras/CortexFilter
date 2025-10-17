
using OpenAI.Chat;

namespace CortexFilter.Filters;

public abstract class AmbiguousFilter<T> : ICollectionFilter<T>, IFilterInitializer<T>
{
    public abstract string Name { get; }
    public abstract string? Description { get; }

    public abstract IEnumerable<T> Filter(IEnumerable<T> collection);

    public Task InitAsync(string query, ChatClient client, IEnumerable<T> collection) =>
        InitInternalAsync(query, client, collection);

    protected abstract Task InitInternalAsync(string query, ChatClient client, IEnumerable<T> collection);
}
