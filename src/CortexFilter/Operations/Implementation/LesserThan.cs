namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{T}"/> used evaluate if provided <see cref="T"/> is lesser than certain value.
/// </summary>
public class LesserThan<T> : IOperation<T>
{
    /// <inheritdoc/>
    public static string Code => "lt";

    private readonly T _value;
    public LesserThan(T value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) < 0;
}
