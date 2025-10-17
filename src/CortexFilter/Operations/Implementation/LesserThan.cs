namespace CortexFilter.Operations;

public class LesserThan<T> : IOperation<T>
{
    public static string Code => "lt";

    private readonly T _value;
    public LesserThan(T value)
    {
        _value = value;
    }

    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) < 0;
}
