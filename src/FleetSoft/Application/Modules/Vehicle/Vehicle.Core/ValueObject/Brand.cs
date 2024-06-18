namespace Vehicle.Core.ValueObject;

public record Brand(string Value)
{
    public static implicit operator Brand(string value) => new(value);
    public static implicit operator string(Brand brand) => brand.Value;
}