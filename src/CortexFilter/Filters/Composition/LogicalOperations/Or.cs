namespace CortexFilter.Filters.Composition.LogicalOperations;

/// <summary>
/// Composite filter to apply <strong>OR</strong> logic operator for multiple instances of <see cref="IConcreteFilter{T}"/>.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
internal class Or<T> : ICollectionFilter<T>
{
    private readonly ICollectionFilter<T>[] _filters;
    public Or(IEnumerable<ICollectionFilter<T>> filters)
    {
        _filters = filters.ToArray();
    }
    /// <inheritdoc/>
    public IEnumerable<T> Filter(IEnumerable<T> collection)
    {
        if (_filters.Length == 0)
            return Array.Empty<T>();

        var filteredItems = _filters.Select(x => x.Filter(collection)).ToArray();
        var current = filteredItems.First();
        for (int i = 1; i < filteredItems.Length; i++)
        {
            current = current.Union(filteredItems[i]);
        }
        return current.ToArray();
    }
}
