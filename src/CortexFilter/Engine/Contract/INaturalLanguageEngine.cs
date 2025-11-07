namespace CortexFilter;

/// <summary>
/// Interface for engines that filters data sets based on natural language query.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
public interface INaturalLanguageEngine<T>
{
    /// <summary>
    /// Async function that filters data based on provided query.
    /// </summary>
    /// <param name="query">Users query what to filter.</param>
    /// <returns>Filtered collection.</returns>
    public Task<IEnumerable<T>> SearchAsync(string query);
}
