namespace SpreadSheet.Value;

public class FunctionValue : IValue
{
    public string Name { get; }
    public IValue[] Arguments { get; }
    public int Length => Name.Length + Arguments.Length;
}