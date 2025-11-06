namespace CortexFilter.Operations;

/// <summary>
/// <see cref="IOperation{string}"/> used evaluate if <see cref="string"/> contains certain value.
/// </summary>
public class Contains : IOperation<string>
{
    /// <inheritdoc/>
    public static string Code => "contains";
    private readonly string _value;
    public Contains(string value)
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
