using FipLang.Value;

namespace FipLang.Data;

public class Repository
{
    private readonly Dictionary<string?, IValue> _data = new();

    public IValue this[string? identifier]
    {
        get => _data[identifier];
        set => _data[identifier] = value;
    }
    
    public Dictionary<string?, IValue> Values => _data;
    public void Free(string? identifier) => _data.Remove(identifier);
    public void FreeAll() => _data.Clear();
}
