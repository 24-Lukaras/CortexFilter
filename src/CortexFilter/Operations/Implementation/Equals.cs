namespace CortexFilter.Operations;

public class Equals<T> : IOperation<T>
{
    public static string Code => "eq";

    private readonly T _value;
    public Equals(T value)
    {
        _value = value;
    }
    public bool Evaluate(T? value)
    {
        return _value?.Equals(value) ?? false;
    }
}
