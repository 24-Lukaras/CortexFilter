namespace CortexFilter.Operations;

public class GreaterOrEqual<T> : IOperation<T>
{
    public static string Code => "ge";

    private readonly T _value;
    public GreaterOrEqual(T value)
    {
        _value = value;
    }

    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) >= 0;
}
