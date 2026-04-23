namespace CortexFilter.Filters;

/// <summary>
/// Factory used for creation of <see cref="IConcreteFilter{T}"/>.
/// </summary>
/// <typeparam name="T">Type of filtered data.</typeparam>
public interface IConcreteFilterFactory<T>
{
    /// <summary>
    /// Filter name sent to LLM.
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Optional filter description sent to LLM.
    /// </summary>
    string? Description { get; }
    /// <summary>
    /// Collection of <see cref="Operations.IOperation{T}.Code"/> specifying which operations are supported.
    /// </summary>
    string[] SupportedOperations { get; }

    /// <summary>
    /// Creates <see cref="IConcreteFilter{T}"/> based on provided parameters.
    /// </summary>
    /// <param name="operation"><see cref="Operations.IOperation{T}.Code"/> to specify which operation will be used for filtering.</param>
    /// <param name="value">Value provided for the filtering operation.</param>
    /// <returns>Instance of <see cref="IConcreteFilter{T}"/>.</returns>
    IConcreteFilter<T> CreateFilter(string operation, string value);
}
