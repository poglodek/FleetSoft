using MediatR;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using Vehicle.Application.Command.CreateVehicle;
using Vehicle.Core.Repositories;
using Vehicle.Core.ValueObject;

namespace Vehicle.Application;

public class CreateVehicle
{
    [Fact]
    public async Task CreateVehicle_AllValid_ShouldCreateVehicle()
    {
        // Arrange
        var repository = Substitute.For<IVehicleRepository>();
        var request = new CreateVehicleRequest("BMW", "X5", VehicleType.Car, "ABC123", 2021);
        var handler = new CreateVehicleRequestHandler(repository);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Id.ShouldNotBe(Guid.Empty);
        await repository.Received(1).AddAsync(Arg.Any<Core.Entity.Vehicle>());
    }
    
    [Fact]
    public async Task CreateVehicle_Invalid_ShouldThrownAnException()
    {
        // Arrange
        var repository = Substitute.For<IVehicleRepository>();
        var request = new CreateVehicleRequest("BMW", "X5", "SUV", "ABC123", 2021);
        var handler = new CreateVehicleRequestHandler(repository);
        
        // Act
        var exception = await Record.ExceptionAsync(async () => await handler.Handle(request, CancellationToken.None));

        // Assert

        exception.ShouldNotBeNull();
        await repository.Received(0).AddAsync(Arg.Any<Core.Entity.Vehicle>());
    }
}