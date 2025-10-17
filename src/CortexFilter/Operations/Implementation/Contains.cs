using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortexFilter.Operations;

public class Contains : IOperation<string>
{
    public static string Code => "contains";
    private readonly string _value;
    public Contains(string value)
    {
        _value = value;
    }

    public bool Evaluate(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        return value.Contains(_value);
    }
}
