using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Shouldly;
using Vehicle.Application.Command.AddMileage;
using Vehicle.Application.Command.SetArchive;
using Vehicle.Application.Exceptions;
using Vehicle.Core.Repositories;
using Vehicle.Core.ValueObject;

namespace Vehicle.Application;

public class SetArchive
{
    [Fact]
    public async Task SetArchive_ShouldAddMileageToVehicle()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var vehicleRepository = Substitute.For<IVehicleRepository>();
        var id = Guid.NewGuid();
        var vehicle = new Core.Entity.Vehicle(id, "Porsche", "911 997 GT3",
            new VehicleType(VehicleType.Car), "SK 911",
            new ProductionYear(2024), new Mileage(1.0));

        vehicleRepository.GetByIdAsync(id).Returns(vehicle);
        var command = new SetArchiveRequest(id);
        var handler = new SetArchiveRequestHandler(vehicleRepository,timeProvider);

        // Act
        var exception = await Record.ExceptionAsync(async () => await handler.Handle(command, CancellationToken.None));

        // Assert
        exception.ShouldBeNull();
    }
    [Fact]
    public async Task SetArchive_VehicleNotFound_ShouldThrownException()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        var vehicleRepository = Substitute.For<IVehicleRepository>();
        var id = Guid.NewGuid();
        
        var command = new SetArchiveRequest(id);
        var handler = new SetArchiveRequestHandler(vehicleRepository, timeProvider);

        // Act
        var exception = await Record.ExceptionAsync(async () => await handler.Handle(command, CancellationToken.None));

        // Assert
        exception.ShouldBeOfType<VehicleNotFoundException>();
    }
}