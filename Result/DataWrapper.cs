using SpreadSheet.Value;

namespace SpreadSheet.Result;

public class DataWrapper
{
    public Type.Integrated Type;
    public IValue Value;

    public override string ToString()
    {
        return $"{Type} {Value}";
    }
}