namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{string}"/> used evaluate if <see cref="string"/> starts with certain value.
/// </summary>
public class StartsWith : IOperation<string>
{
    /// <inheritdoc/>
    public static string Code => "startsWith";
    private readonly string _value;
    public StartsWith(string value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.StartsWith(_value);
    }
}
