using Vehicle.Core.Exceptions;
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
    public Archived Archived { get; private set; } = new(false, DateTimeOffset.MinValue);
    
    
    private Vehicle(){}
    public Vehicle(Guid id ,Brand brand, Model model, VehicleType vehicleType,LicensePlate licensePlate, ProductionYear productionYear, Mileage mileage)
    {
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
        }

        Id = id;
        Brand = brand ?? throw new InvalidVerificationException("Brand is null");
        Model = model  ?? throw new InvalidVerificationException("Model is null");
        VehicleType = vehicleType ?? throw new InvalidVerificationException("VehicleType is null");
        LicensePlate = licensePlate ?? throw new InvalidVerificationException("LicensePlate is null");
        ProductionYear = productionYear ?? throw new InvalidVerificationException("ProductionYear is null");
        Mileage = mileage ?? throw new InvalidVerificationException("Mileage is null");
    }

    public void AddMileage(double mileage)
    {
        Mileage += mileage;
    }

    public void SetVehicleAsArchived(TimeProvider timeProvider)
    {
        Archived = new(false, timeProvider.GetUtcNow());
    }
    

    
}