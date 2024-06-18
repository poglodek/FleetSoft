namespace Vehicle.Core.ValueObject;

public record VehicleType
{
    public string Type { get; private set; }
    public VehicleType(string type)
    {
        if (type != Car && type != Motorbike && type != Van)
        {
            throw new ArgumentException("Invalid vehicle type");
        }

        Type = type;
    }

    public static string Car => nameof(Car);
    public static string Motorbike => nameof(Motorbike);
    public static string Van => nameof(Van);
};