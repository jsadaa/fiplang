namespace FipLang.Value;

public class BoolValue : IValue
{
    public BoolValue(bool content)
    {
        Content = content;
    }

    public bool Content { get; }
    public int Length => Content.ToString().Length;

    public override string ToString()
    {
        return Content.ToString().ToLower();
    }
}