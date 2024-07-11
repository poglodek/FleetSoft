using Shouldly;
using Vehicle.Core.Exceptions;
using Vehicle.Core.ValueObject;

namespace Vehicle.Core;

public class VehicleUnitTest
{

    public static Brand Brand = new("Porsche");
    public static Model Model = new("911");
    public static VehicleType VehicleType = new(VehicleType.Car);
    public static LicensePlate LicensePlate = new("SK 911");
    public static ProductionYear ProductionYear = new(2024);
    public static Mileage Mileage = new(1);
    
    private static Entity.Vehicle ReturnValidVehicle(Guid id)
        => new(id, Brand, Model, VehicleType, LicensePlate, ProductionYear, Mileage);
    
    
    [Fact]
    public void CreateVehicle_AllValidData_ShouldReturnObject()
    {
        
        var id = Guid.NewGuid();
        var vehicle = ReturnValidVehicle(id);

        vehicle.Id.Value.ShouldBe(id);
        vehicle.Brand.Value.ShouldBe(Brand.Value);
        vehicle.Model.Value.ShouldBe(Model.Value);
        vehicle.VehicleType.Type.ShouldBe(VehicleType.Type);
        vehicle.LicensePlate.Value.ShouldBe(LicensePlate.Value);
        vehicle.ProductionYear.Year.ShouldBe(ProductionYear.Year);
        vehicle.Mileage.Value.ShouldBe(Mileage.Value);
    }

    
    [Fact]
    public void CreateVehicle_AllValidDataWithEmptyId_ShouldReturnObject()
    {
        var id = Guid.Empty;
        var vehicle = ReturnValidVehicle(id);
        
        vehicle.Id.Value.ShouldNotBe(id);
        vehicle.Brand.Value.ShouldBe(Brand.Value);
        vehicle.Model.Value.ShouldBe(Model.Value);
        vehicle.VehicleType.Type.ShouldBe(VehicleType.Type);
        vehicle.LicensePlate.Value.ShouldBe(LicensePlate.Value);
        vehicle.ProductionYear.Year.ShouldBe(ProductionYear.Year);
        vehicle.Mileage.Value.ShouldBe(Mileage.Value);
    }

    [Fact]
    public void CreateVehicle_NullBrand_ShouldThrownAnException()
    {
        Should.Throw<InvalidVerificationException>(() => new Entity.Vehicle(Guid.NewGuid(), null, Model, VehicleType, LicensePlate, ProductionYear, Mileage));
    }
    
    [Fact]
    public void CreateVehicle_NullModel_ShouldThrownAnException()
    {
        Should.Throw<InvalidVerificationException>(() => new Entity.Vehicle(Guid.NewGuid(), Brand, null, VehicleType, LicensePlate, ProductionYear, Mileage));
    }
    
    [Fact]
    public void CreateVehicle_NullVehicleType_ShouldThrownAnException()
    {
        Should.Throw<InvalidVerificationException>(() => new Entity.Vehicle(Guid.NewGuid(), Brand, Model, null, LicensePlate, ProductionYear, Mileage));
    }
    
    [Fact]
    public void CreateVehicle_NullLicensePlate_ShouldThrownAnException()
    {
        Should.Throw<InvalidVerificationException>(() => new Entity.Vehicle(Guid.NewGuid(), Brand, Model, VehicleType, null, ProductionYear, Mileage));
    }
    
    [Fact]
    public void CreateVehicle_NullProductionYear_ShouldThrownAnException()
    {
        Should.Throw<InvalidVerificationException>(() => new Entity.Vehicle(Guid.NewGuid(), Brand, Model, VehicleType, LicensePlate, null, Mileage));
    }
    
    [Fact]
    public void CreateVehicle_NullMileage_ShouldThrownAnException()
    {
        Should.Throw<InvalidVerificationException>(() => new Entity.Vehicle(Guid.NewGuid(), Brand, Model, VehicleType, LicensePlate, ProductionYear, null));
    }
    
    [Fact]
    public void CreateVehicleType_InvalidType_ShouldThrownAnException()
    {
        Should.Throw<ArgumentException>(() => new VehicleType("Bike"));
    }
    
    [Fact]
    public void CreateProductionYear_YearTooLow_ShouldThrownAnException()
    {
        Should.Throw<InvalidYearException>(() => new ProductionYear(1420));
    }
    
    [Fact]
    public void CreateProductionYear_YearTooHigh_ShouldThrownAnException()
    {
        Should.Throw<InvalidYearException>(() => new ProductionYear(2069));
    }
}