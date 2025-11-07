namespace CortexFilter.Engine;

/// <summary>
/// Interface used as a mediator service for <see cref="Filters.CortexResource{T,E}"/> to filter from multiple search engines.
/// </summary>
public interface ICortex
{
    /// <summary>
    /// Searches other registered <see cref="INaturalLanguageEngine{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of data to search.</typeparam>
    /// <param name="query">What to filter?</param>
    /// <returns>Filtered data.</returns>
    public Task<IEnumerable<T>> SearchAsync<T>(string query);
}
