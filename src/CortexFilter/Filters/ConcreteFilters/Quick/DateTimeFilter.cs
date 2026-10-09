using CortexFilter.Operations;
using CortexFilter.Operations.Implementation.Comparison;
using CortexFilter.Operations.Implementation.Dates;

namespace CortexFilter.Filters;

public class DateTimeFilter<T> : IConcreteFilterFactory<T>
{
    public virtual bool Available => true;
    public string Name { get; }
    public string? Description { get; }
    private readonly Func<T, DateTime?> _propertyGetter;
    public DateTimeFilter(string name, string? description, Func<T, DateTime?> propertyGetter)
    {
        Name = name;
        Description = description;
        _propertyGetter = propertyGetter;
    }

    public string[] SupportedOperations => [
        Equals<DateTime>.Code,
        LesserThan<DateTime>.Code,
        LesserOrEqual<DateTime>.Code,
        GreaterThan<DateTime>.Code,
        GreaterOrEqual<DateTime>.Code,
        DayOf.Code,
        MonthOf.Code,
        YearOf.Code
    ];


    public IConcreteFilter<T> CreateFilter(string operation, string value)
    {
        IOperation<DateTime?> op;
        if (int.TryParse(value, out int number))
        {
            op = OperationFactory.CreateFromCodeForDateTime(operation, number);
        }
        else if (DateTime.TryParse(value, out DateTime dt))
        {
            op = OperationFactory.CreateFromCode(operation, dt);
        }
        else
        {
            op = new InvalidOperation<DateTime?>();
        }

        return new DynamicDateTimeFilter<T>(op, _propertyGetter);
    }
}
internal class DynamicDateTimeFilter<T> : IConcreteFilter<T>
{
    private readonly IOperation<DateTime?> _operation;
    private readonly Func<T, DateTime?> _propertyGetter;
    public DynamicDateTimeFilter(IOperation<DateTime?> operation, Func<T, DateTime?> propertyGetter)
    {
        _operation = operation;
        _propertyGetter = propertyGetter;
    }
    public IEnumerable<T> Filter(IEnumerable<T> collection) =>
        collection.Where(x => _operation.Evaluate(_propertyGetter(x)));
}
