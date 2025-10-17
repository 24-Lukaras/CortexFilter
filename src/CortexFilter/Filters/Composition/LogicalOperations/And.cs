namespace CortexFilter.Filters.Composition.LogicalOperations;

internal class And<T> : ICollectionFilter<T>
{
    private readonly ICollectionFilter<T>[] _filters;
    public And(IEnumerable<ICollectionFilter<T>> filters)
    {
        _filters = filters.ToArray();
    }
    public IEnumerable<T> Filter(IEnumerable<T> collection)
    {
        if (_filters.Length == 0)
            return Array.Empty<T>();

        var filteredItems = _filters.Select(x => x.Filter(collection)).ToArray();
        var current = filteredItems.First();
        for (int i = 1; i < filteredItems.Length; i++)
        {
            current = current.Intersect(filteredItems[i]);
        }
        return current.ToArray();
    }
}
