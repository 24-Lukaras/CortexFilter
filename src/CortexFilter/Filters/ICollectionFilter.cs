namespace CortexFilter.Filters;

/// <summary>
/// Interface for specific collection filtering.
/// </summary>
/// <typeparam name="T">Type of data to be filtered.</typeparam>
public interface ICollectionFilter<T>
{
    /// <summary>
    /// Filters collection based on the filter definition.
    /// </summary>
    /// <param name="collection">Data that will be filtered.</param>
    /// <returns>Filtered collection.</returns>
    public IEnumerable<T> Filter(IEnumerable<T> collection);
}
