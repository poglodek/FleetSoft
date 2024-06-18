namespace Vehicle.Core.ValueObject;

public record Model(string Value)
{
    public static implicit operator Model(string value) => new(value);
    public static implicit operator string(Model model) => model.Value;
}