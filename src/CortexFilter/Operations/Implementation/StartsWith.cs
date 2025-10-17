namespace CortexFilter.Operations;

public class StartsWith : IOperation<string>
{
    public static string Code => "startsWith";
    private readonly string _value;
    public StartsWith(string value)
    {
        _value = value;
    }

    public bool Evaluate(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.StartsWith(_value);
    }
}
