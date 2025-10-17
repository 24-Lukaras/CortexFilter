namespace CortexFilter.Operations;

public class InvalidOperation<T> : IOperation<T>
{
    public static string Code => "";

    public bool Evaluate(T? value) => false;
}
