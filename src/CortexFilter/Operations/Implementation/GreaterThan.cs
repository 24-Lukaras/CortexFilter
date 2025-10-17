namespace CortexFilter.Operations;

public class GreaterThan<T> : IOperation<T>
{
    public static string Code => "gt";

    private readonly T _value;
    public GreaterThan(T value)
    {
        _value = value;
    }

    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) > 0;
}
