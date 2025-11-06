namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{T}"/> fallback, that evaluates all provided values as false.
/// </summary>
public class InvalidOperation<T> : IOperation<T>
{
    public static string Code => "";

    public bool Evaluate(T? value) => false;
}
