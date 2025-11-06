namespace CortexFilter.Operations;

/// <summary>
/// Used to create specific operations like <see cref="Equals{T}"/> or <see cref="Contains"/> to evaluate provided data.
/// </summary>
/// <typeparam name="T">Type of evaluated data.</typeparam>
public interface IOperation<T>
{
    /// <summary>
    /// Unique code that allows LLM to select certain operation.
    /// </summary>
    abstract static string Code { get; }

    /// <summary>
    /// Used to evaluate if the value is valid.
    /// </summary>
    /// <param name="value">Value to be evaluated.</param>
    /// <returns><see cref="bool"/> indicating if provided value is valid.</returns>
    public bool Evaluate(T? value);
}
