namespace CortexFilter.Filters;

/// <summary>
/// Filter type for filtering from different <see cref="NaturalLanguageEngine{T}"/>.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
internal interface ICortexResource<T> : IFilterInitializer<T>, ICollectionFilter<T>
{
    /// <summary>
    /// Indicates if the resource can be used.
    /// </summary>
    bool Available { get; }

    /// <summary>
    /// Resource name sent to LLM.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Optional resource description sent to LLM.
    /// </summary>
    string? Description { get; }
}