using SpreadSheet.Value;

namespace FipLang;

public class DataRepository
{
    private readonly Dictionary<string?, IValue> _data = new();

    public IValue this[string? identifier]
    {
        get => _data[identifier];
        set => _data[identifier] = value;
    }
}
