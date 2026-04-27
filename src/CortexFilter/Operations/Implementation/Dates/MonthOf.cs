namespace CortexFilter.Operations.Implementation.Dates;

public class MonthOf : IOperation<DateTime?>
{
    public static string Code => "month";

    private readonly int _value;
    public MonthOf(int value)
    {
        _value = value;
    }

    public bool Evaluate(DateTime? value) =>
        value?.Month == _value;
}
