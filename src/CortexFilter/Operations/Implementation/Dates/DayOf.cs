namespace CortexFilter.Operations.Implementation.Dates;

public class DayOf : IOperation<DateTime?>
{
    public static string Code => "day";

    private readonly int _value;
    public DayOf(int value)
    {
        _value = value;
    }

    public bool Evaluate(DateTime? value) =>
        value?.Day == _value;
}
