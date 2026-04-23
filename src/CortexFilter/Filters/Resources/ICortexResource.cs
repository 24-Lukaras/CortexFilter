namespace CortexFilter.Filters;

/// <summary>
/// Filter type for filtering from different <see cref="NaturalLanguageEngine{T}"/>.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
internal interface ICortexResource<T> : IFilterInitializer<T>, ICollectionFilter<T>
{
    /// <summary>
    /// Resource name sent to LLM.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Optional resource description sent to LLM.
    /// </summary>
    public string? Description { get; }
}