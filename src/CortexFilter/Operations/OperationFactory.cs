namespace CortexFilter.Operations;

/// <summary>
/// Static class for creation of operations for filters. Use <see cref="CreateFromCode{T}(string, T)"/> to create an operation.
/// </summary>
public static class OperationFactory
{
    /// <summary>
    /// Creates a string operation based on provided code.
    /// </summary>
    /// <param name="code">Code of operation. For example "<i>eq</i>" for <see cref="Equals{T}"/> or "<i>contains</i>" for <see cref="Contains"/>.</param>
    /// <param name="value">String used in filtration.</param>
    /// <returns>Operation used for filtration.</returns>
    public static IOperation<string> CreateFromCode(string code, string value)
    {
        if (code == Contains.Code)
            return new Contains(value);
        else if (code == StartsWith.Code)
            return new StartsWith(value);
        else if (code == EndsWith.Code)
            return new EndsWith(value);

        return CreateFromCode<string>(code, value);
    }

    /// <summary>
    /// Creates an operation based on provided code and value.
    /// </summary>
    /// <typeparam name="T">Type of to be evaluated value.</typeparam>
    /// <param name="code">Code of operation. For example "<i>eq</i>" for <see cref="Equals{T}"/> or "<i>le</i>" for <see cref="LesserThan{T}"/>.</param>
    /// <param name="value">Value used in filtration.</param>
    /// <returns>Operation used for filter evaluation.</returns>
    public static IOperation<T> CreateFromCode<T>(string code, T value)
    {
        if (code == Equals<T>.Code)
            return new Equals<T>(value);
        else if (code == GreaterThan<T>.Code)
            return new GreaterThan<T>(value);
        else if (code == GreaterOrEqual<T>.Code)
            return new GreaterOrEqual<T>(value);
        else if (code == LesserThan<T>.Code)
            return new LesserThan<T>(value);
        else if (code == LesserOrEqual<T>.Code)
            return new LesserOrEqual<T>(value);

        return new InvalidOperation<T>();
    }
}
