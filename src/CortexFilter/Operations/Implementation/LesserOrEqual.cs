namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{T}"/> used evaluate if provided <see cref="T"/> is lesser or equal to certain value.
/// </summary>
public class LesserOrEqual<T> : IOperation<T>
{
    /// <inheritdoc/>
    public static string Code => "le";

    private readonly T _value;
    public LesserOrEqual(T value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) <= 0;
}
