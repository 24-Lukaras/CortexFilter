namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{T}"/> used evaluate if provided <see cref="T"/> is equal to certain value.
/// </summary>
public class Equals<T> : IOperation<T>
{
    /// <inheritdoc/>
    public static string Code => "eq";

    private readonly T _value;
    public Equals(T value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(T? value)
    {
        return _value?.Equals(value) ?? false;
    }
}
