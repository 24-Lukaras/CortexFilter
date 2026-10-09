using CortexFilter.Operations;
using CortexFilter.Operations.Implementation.Comparison;
using System.Globalization;

namespace CortexFilter.Filters;

public class NumberFilter<T, TNumber> : IConcreteFilterFactory<T> where TNumber : IConvertible
{
    public virtual bool Available => true;
    public string Name { get; }
    public string? Description { get; }
    private readonly Func<T, TNumber> _propertyGetter;
    public NumberFilter(string name, string? description, Func<T, TNumber> propertyGetter)
    {
        Name = name;
        Description = description;
        _propertyGetter = propertyGetter;
    }

    public string[] SupportedOperations => [
        Equals<TNumber>.Code,
        LesserThan<TNumber>.Code,
        LesserOrEqual<TNumber>.Code,
        GreaterThan<TNumber>.Code,
        GreaterOrEqual<TNumber>.Code,
    ];

    public IConcreteFilter<T> CreateFilter(string operation, string value)
    {
        IOperation<decimal> op;
        if (decimal.TryParse(value, CultureInfo.InvariantCulture, out decimal number))
        {
            op = OperationFactory.CreateFromCode(operation, number);
        }
        else
        {
            op = new InvalidOperation<decimal>();
        }
        return new DynamicNumberFilter<T>(op, (entity) => Convert.ToDecimal(_propertyGetter(entity)));
    }
}
internal class DynamicNumberFilter<T> : IConcreteFilter<T>
{
    private readonly IOperation<decimal> _operation;
    private readonly Func<T, decimal> _propertyGetter;
    public DynamicNumberFilter(IOperation<decimal> operation, Func<T, decimal> propertyGetter)
    {
        _operation = operation;
        _propertyGetter = propertyGetter;
    }
    public IEnumerable<T> Filter(IEnumerable<T> collection) =>
        collection.Where(x => _operation.Evaluate(_propertyGetter(x)));
}
