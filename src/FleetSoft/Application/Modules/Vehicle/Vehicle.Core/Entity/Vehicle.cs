using Vehicle.Core.ValueObject;

namespace Vehicle.Core.Entity;

public class Vehicle : Shared.Core.Entity
{
    
    public Brand Brand { get; private set; }
    public Model Model { get; private set; }
    public VehicleType VehicleType { get; private set; }
    public Vehicle(Guid id ,Brand brand, Model model, VehicleType vehicleType)
    {
        Id = id;
        Brand = brand;
        Model = model;
        VehicleType = vehicleType;
    }

    
}