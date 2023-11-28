using FipLang.Type;
using FipLang.Value;

namespace FipLang.Data;

public class Wrapper
{
    public Integrated Type;
    public IValue Value;

    public override string ToString()
    {
        return $"{Type} {Value}";
    }
}