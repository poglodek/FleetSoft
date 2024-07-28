using NSubstitute;
using Shouldly;
using Vehicle.Application.Command.AddMileage;
using Vehicle.Core.Repositories;
using Vehicle.Core.ValueObject;
using Vehicle.Infrastructure.Exceptions;
using VehicleNotFoundException = Vehicle.Application.Exceptions.VehicleNotFoundException;

namespace Vehicle.Application;

public class AddMileage
{
    [Fact]
    public async Task AddMileage_ShouldAddMileageToVehicle()
    {
        // Arrange
        var vehicleRepository = Substitute.For<IVehicleRepository>();
        var id = Guid.NewGuid();
        var vehicle = new Core.Entity.Vehicle(id, "Porsche", "911 997 GT3",
            new VehicleType(VehicleType.Car), "SK 911",
            new ProductionYear(2024), new Mileage(1.0));

        vehicleRepository.GetByIdAsync(id).Returns(vehicle);
        var command = new AddMileageRequest(id, 1000);
        var handler = new AddMileageRequestHandler(vehicleRepository);

        // Act
        var exception = await Record.ExceptionAsync(async () => await handler.Handle(command, CancellationToken.None));

        // Assert
        exception.ShouldBeNull();
    }
    [Fact]
    public async Task AddMileage_VehicleNotFound_ShouldThrownException()
    {
        // Arrange
        var vehicleRepository = Substitute.For<IVehicleRepository>();
        var id = Guid.NewGuid();
        
        var command = new AddMileageRequest(id, 1000);
        var handler = new AddMileageRequestHandler(vehicleRepository);

        // Act
        var exception = await Record.ExceptionAsync(async () => await handler.Handle(command, CancellationToken.None));

        // Assert
        exception.ShouldBeOfType<VehicleNotFoundException>();
    }
}