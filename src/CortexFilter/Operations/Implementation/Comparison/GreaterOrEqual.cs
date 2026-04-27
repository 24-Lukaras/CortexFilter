namespace CortexFilter.Operations.Implementation.Comparison;

/// <summary>
/// <see cref="IOperation{T}"/> used evaluate if provided <see cref="T"/> is greater or equal to certain value.
/// </summary>
public class GreaterOrEqual<T> : IOperation<T>
{
    /// <inheritdoc/>
    public static string Code => "ge";

    private readonly T _value;
    public GreaterOrEqual(T value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(T? value) => value is not null
        && value is IComparable comparable
        && comparable.CompareTo(_value) >= 0;
}
