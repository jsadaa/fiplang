using FipLang.Value;

namespace FipLang.Type;

public enum Integrated
{
    Integer,
    Double,
    String,
    Void
}

public static class IntegratedExtensions
{
    public static string ToLowerStr(this Integrated integrated)
    {
        return integrated switch
        {
            Integrated.Integer => "int",
            Integrated.Double => "double",
            Integrated.String => "string",
            Integrated.Void => "void",
            _ => throw new ArgumentOutOfRangeException(nameof(integrated), integrated, null)
        };
    }
    
    public static Integrated ToFipType(this string str)
    {
        return str switch
        {
            "int" => Integrated.Integer,
            "double" => Integrated.Double,
            "string" => Integrated.String,
            "void" => Integrated.Void,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
        };
    }
    
    public static Integrated FromFipValue(this IValue value)
    {
        return value switch
        {
            IntegerValue _ => Integrated.Integer,
            DoubleValue _ => Integrated.Double,
            StringValue _ => Integrated.String,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}