namespace Vehicle.Core.ValueObject;

public record Mileage(double Value)
{
    public static implicit operator Mileage(double value) => new(value);
    public static implicit operator double(Mileage mileage) => mileage.Value;
}