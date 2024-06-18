namespace Vehicle.Core.ValueObject;

public record LicensePlate(string Value)
{
    public static implicit operator LicensePlate(string value) => new(value);
    public static implicit operator string(LicensePlate licensePlate) => licensePlate.Value;
}