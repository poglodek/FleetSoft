namespace Vehicle.Application.Dto;

public record VehicleDto(
    Guid Id,
    string Name,
    string Model,
    string Brand,
    int Year,
    double Mileage,
    string VehicleType,
    string LicensePlate,
    string Plate)
{
    public static implicit operator VehicleDto(Core.Entity.Vehicle vehicle) 
        => new(vehicle.Id, vehicle.Brand.Value, vehicle.Model.Value, vehicle.Brand.Value, vehicle.ProductionYear.Year, 
            vehicle.Mileage.Value, vehicle.VehicleType.Type, vehicle.LicensePlate.Value, vehicle.LicensePlate.Value);
}