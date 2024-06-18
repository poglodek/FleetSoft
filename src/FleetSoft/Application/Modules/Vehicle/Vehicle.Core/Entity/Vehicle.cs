using Vehicle.Core.ValueObject;

namespace Vehicle.Core.Entity;

public class Vehicle : Shared.Core.Entity
{
    
    public Brand Brand { get; private set; }
    public Model Model { get; private set; }
    public VehicleType VehicleType { get; private set; }
    public LicensePlate LicensePlate { get; private set; }
    public ProductionYear ProductionYear { get; private set; }
    public Mileage Mileage { get; private set; }
    public Vehicle(Guid id ,Brand brand, Model model, VehicleType vehicleType,LicensePlate licensePlate, ProductionYear productionYear, Mileage mileage)
    {
        Id = id;
        Brand = brand;
        Model = model;
        VehicleType = vehicleType;
        LicensePlate = licensePlate;
        ProductionYear = productionYear;
        Mileage = mileage;
    }

    
}