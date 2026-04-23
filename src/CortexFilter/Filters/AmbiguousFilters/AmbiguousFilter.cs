
using OpenAI.Chat;

namespace CortexFilter.Filters;

/// <summary>
/// Filter type that requires further clarification before being used.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
public abstract class AmbiguousFilter<T> : ICollectionFilter<T>, IFilterInitializer<T>
{
    /// <summary>
    /// Name of filter sent to LLM.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Optional description sent to LLM.
    /// </summary>
    public abstract string? Description { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<T> Filter(IEnumerable<T> collection);

    /// <inheritdoc/>
    public Task InitAsync(FilterInitializerProperties<T> properties) =>
        InitInternalAsync(properties.Query, properties.Client, properties.Collection);

    /// <summary>
    /// Initializes the filter before filtering with limited parameters.
    /// </summary>
    /// <param name="query">Original user query.</param>
    /// <param name="client">Instance of chat client.</param>
    /// <param name="collection">Data that will be filtered.</param>
    /// <returns>Asynchronous task.</returns>
    protected abstract Task InitInternalAsync(string query, ChatClient client, IEnumerable<T> collection);
}
