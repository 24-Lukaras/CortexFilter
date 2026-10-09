using CortexFilter.Operations;
using CortexFilter.Operations.Implementation.String;

namespace CortexFilter.Filters;

public class StringFilter<T> : IConcreteFilterFactory<T>
{
    public virtual bool Available => true;
    public string Name { get; }
    public string? Description { get; }
    private readonly Func<T, string> _propertyGetter;
    public StringFilter(string name, string? description, Func<T, string> propertyGetter)
    {
        Name = name;
        Description = description;
        _propertyGetter = propertyGetter;
    }

    public string[] SupportedOperations => [
        Equals<string>.Code,
        Contains.Code,
        StartsWith.Code,
        EndsWith.Code,
    ];

    public IConcreteFilter<T> CreateFilter(string operation, string value) =>
        new DynamicStringFilter<T>(
            OperationFactory.CreateFromCode(operation, value),
            _propertyGetter);
}
internal class DynamicStringFilter<T> : IConcreteFilter<T>
{
    private readonly IOperation<string> _operation;
    private readonly Func<T, string> _propertyGetter;
    public DynamicStringFilter(IOperation<string> operation, Func<T, string> propertyGetter)
    {
        _operation = operation;
        _propertyGetter = propertyGetter;
    }
    public IEnumerable<T> Filter(IEnumerable<T> collection) =>
        collection.Where(x => _operation.Evaluate(_propertyGetter(x)));
}