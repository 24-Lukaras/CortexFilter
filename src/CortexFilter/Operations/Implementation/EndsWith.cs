namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{string}"/> used evaluate if <see cref="string"/> ends with certain value.
/// </summary>
public class EndsWith : IOperation<string>
{
    /// <inheritdoc/>
    public static string Code => "endsWith";
    private readonly string _value;
    public EndsWith(string value)
    {
        _value = value;
    }

    /// <inheritdoc/>
    public bool Evaluate(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.Contains(_value);
    }
}
