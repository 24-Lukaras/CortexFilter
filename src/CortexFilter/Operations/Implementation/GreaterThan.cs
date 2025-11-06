namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{T}"/> used evaluate if provided <see cref="T"/> is greater than certain value.
/// </summary>
public class GreaterThan<T> : IOperation<T>
{
    /// <inheritdoc/>
    public static string Code => "gt";

    private readonly T _value;
    public GreaterThan(T value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) > 0;
}
