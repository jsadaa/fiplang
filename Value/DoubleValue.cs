using System.Globalization;

namespace FipLang.Value;

public class DoubleValue : IValue
{
    public DoubleValue(double content)
    {
        Content = content;
    }

    public double Content { get; }
    public int Length => Content.ToString(CultureInfo.InvariantCulture).Replace(".", "").Length;

    public override string ToString()
    {
        return Content.ToString(CultureInfo.InvariantCulture);
    }
}