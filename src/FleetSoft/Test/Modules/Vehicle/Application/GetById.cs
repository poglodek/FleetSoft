using NSubstitute;
using Shouldly;
using Vehicle.Core.Repositories;
using Vehicle.Core.ValueObject;
using Vehicle.Infrastructure.Exceptions;
using Vehicle.Infrastructure.Query.GetById;

namespace Vehicle.Application;

public class GetById
{
    [Fact]
    public async Task GetById_WithValidId_ReturnsVehicleDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var vehicle = ReturnVehicle(id);
        var repository = Substitute.For<IVehicleRepository>();
        repository.GetByIdAsync(id).Returns(vehicle);
        var handler = new GetByIdRequestHandler(repository);
        
        // Act
        
        var response = await handler.Handle(new GetByIdRequest(id), CancellationToken.None);
        
        // Assert
        
        response.Id.ShouldBe(id);
        

    }
    
    
    [Fact]
    public async Task GetById_NonExists_ShouldThrownAnException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var vehicle = ReturnVehicle(id);
        var repository = Substitute.For<IVehicleRepository>();
        repository.GetByIdAsync(id).Returns(vehicle);
        var handler = new GetByIdRequestHandler(repository);
        
        // Act
        
        var exception = await Record.ExceptionAsync(async ()=> await handler.Handle(new GetByIdRequest(Guid.NewGuid()), CancellationToken.None));
        
        // Assert

        exception.ShouldBeOfType<VehicleNotFoundException>();


    }

    private static Core.Entity.Vehicle ReturnVehicle(Guid id) =>
        new(id, "Porsche", "911 997 GT3", new VehicleType(VehicleType.Car), "SK 911",
            new ProductionYear(2024), new Mileage(1.0));
}