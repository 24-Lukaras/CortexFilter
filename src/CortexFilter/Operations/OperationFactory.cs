namespace CortexFilter.Operations;

public static class OperationFactory
{
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
