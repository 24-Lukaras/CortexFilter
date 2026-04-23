using CortexFilter.Engine;
using OpenAI.Chat;

namespace CortexFilter.Filters;

/// <summary>
/// Indicates a filter needs further initialization after being created.
/// </summary>
/// <typeparam name="T">Type of fitered data.</typeparam>
internal interface IFilterInitializer<T>
{
    /// <summary>
    /// Initializes the filter before filtering.
    /// </summary>
    /// <param name="properties">Bundled properties provided for initialization.</param>
    /// <returns>Asynchronous task.</returns>
    public Task InitAsync(FilterInitializerProperties<T> properties);
}


/// <summary>
/// Bundle of item required by <see cref="IFilterInitializer{T}"/>.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
/// <param name="Query">Original user query.</param>
/// <param name="Client">Instance of chat client.</param>
/// <param name="Collection">Data that will be filtered.</param>
/// <param name="Cortex">Mediator used for resource filtering.</param>
public record FilterInitializerProperties<T>(
    string Query,
    ChatClient Client,
    IEnumerable<T> Collection,
    ICortex Cortex
);
