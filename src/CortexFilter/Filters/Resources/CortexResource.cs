namespace CortexFilter.Filters;

/// <summary>
/// Filter type for filtering from different <see cref="NaturalLanguageEngine{T}"/>.
/// </summary>
/// <typeparam name="TSource">Type of originaly filtered data.</typeparam>
/// <typeparam name="TResult">Type of resource connected to filtered data.</typeparam>
public abstract class CortexResource<TSource, TResult> : ICortexResource<TSource>
{
    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract string? Description { get; }

    protected IEnumerable<TResult>? _resourceItems;

    /// <inheritdoc/>
    public abstract IEnumerable<TSource> Filter(IEnumerable<TSource> collection);

    /// <inheritdoc/>
    public async Task InitAsync(FilterInitializerProperties<TSource> properties)
    {
        _resourceItems = await properties.Cortex.SearchAsync<TResult>(properties.Query);
    }
}
