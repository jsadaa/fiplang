namespace FipLang.Value;

public class StringValue : IValue
{
    public string Content { get; set; }
    public int Length => Content.Length;
    
    public StringValue(string content)
    {
        Content = content;
    }
    
    public override string ToString()
    {
        return Content;
    }
}