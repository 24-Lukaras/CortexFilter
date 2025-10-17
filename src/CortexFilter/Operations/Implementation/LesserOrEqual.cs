namespace CortexFilter.Operations;

public class LesserOrEqual<T> : IOperation<T>
{
    public static string Code => "lt";

    private readonly T _value;
    public LesserOrEqual(T value)
    {
        _value = value;
    }

    public bool Evaluate(T? value) => value is not null
        && value is IComparable<T> comparable
        && comparable.CompareTo(_value) <= 0;
}
