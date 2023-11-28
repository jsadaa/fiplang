namespace SpreadSheet.Value;

public class IntegerValue : IValue
{
    public int Content { get; }
    public int Length => Content.ToString().Length;
    
    public IntegerValue(int content)
    {
        Content = content;
    }
    
    public override string ToString()
    {
        return Content.ToString();
    }
}