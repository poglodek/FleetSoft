namespace Shared.Core;

public record Id(Guid Value)
{
    public static implicit operator Id(Guid value) => new(value);
    public static implicit operator Guid(Id id) => id.Value; 
    
    public static Id CreateId() => new(Guid.NewGuid());
}