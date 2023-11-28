namespace FipLang.Value;

public interface IValue
{
    public int Length { get; }
    
    public string ToString();
}