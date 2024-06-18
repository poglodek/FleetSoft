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
    
    
    private Vehicle(){}
    public Vehicle(Guid id ,Brand brand, Model model, VehicleType vehicleType,LicensePlate licensePlate, ProductionYear productionYear, Mileage mileage)
    {
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
        }
        
        Id = id;
        Brand = brand;
        Model = model;
        VehicleType = vehicleType;
        LicensePlate = licensePlate;
        ProductionYear = productionYear;
        Mileage = mileage;
    }
    

    
}