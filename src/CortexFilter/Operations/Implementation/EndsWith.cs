namespace CortexFilter.Operations;

public class EndsWith : IOperation<string>
{
    public static string Code => "endsWith";
    private readonly string _value;
    public EndsWith(string value)
    {
        _value = value;
    }

    public bool Evaluate(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.Contains(_value);
    }
}
