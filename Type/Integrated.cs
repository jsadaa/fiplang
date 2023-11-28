using SpreadSheet.Value;

namespace SpreadSheet.Type;

public enum Integrated
{
    Integer,
    String,
    Void
}

public static class IntegratedExtensions
{
    public static string ToString(this Integrated integrated)
    {
        return integrated switch
        {
            Integrated.Integer => "int",
            Integrated.String => "string",
            Integrated.Void => "void",
            _ => throw new ArgumentOutOfRangeException(nameof(integrated), integrated, null)
        };
    }
    
    public static Integrated ToType(this string str)
    {
        return str switch
        {
            "int" => Integrated.Integer,
            "string" => Integrated.String,
            "void" => Integrated.Void,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
        };
    }
    
    public static Integrated FromValue(this IValue value)
    {
        return value switch
        {
            IntegerValue _ => Integrated.Integer,
            StringValue _ => Integrated.String,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}