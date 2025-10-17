namespace CortexFilter.Operations;

public interface IOperation<T>
{
    abstract static string Code { get; }
    public bool Evaluate(T? value);
}
