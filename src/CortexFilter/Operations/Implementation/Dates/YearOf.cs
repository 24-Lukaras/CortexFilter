namespace CortexFilter.Operations.Implementation.Dates;

public class YearOf : IOperation<DateTime?>
{
    public static string Code => "year";

    private readonly int _value;
    public YearOf(int value)
    {
        _value = value;
    }

    public bool Evaluate(DateTime? value) =>
        value?.Month == _value;
}
