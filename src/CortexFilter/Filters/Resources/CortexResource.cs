using OpenAI.Chat;

namespace CortexFilter.Filters;

public abstract class CortexResource<TSource, TResult> : ICortexResource<TSource>
{
    public abstract string Name { get; }
    public abstract string? Description { get; }

    protected IEnumerable<TResult>? _resourceItems;
    public abstract IEnumerable<TSource> Filter(IEnumerable<TSource> collection);

    public async Task InitAsync(FilterInitializerProperties<TSource> properties)
    {
        _resourceItems = await properties.Cortex.Search<TResult>(properties.Query);
    }
}
