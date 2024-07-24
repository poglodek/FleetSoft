using MediatR;
using Vehicle.Application.Dto;
using Vehicle.Core.Repositories;

namespace Vehicle.Application.Command.CreateVehicle;

public class CreateVehicleRequestHandler(IVehicleRepository repository) : IRequestHandler<CreateVehicleRequest, VehicleCreatedDto>
{
    public async Task<VehicleCreatedDto> Handle(CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = new Core.Entity.Vehicle(Guid.NewGuid(), 
            new Core.ValueObject.Brand(request.Brand), 
            new Core.ValueObject.Model(request.Model), 
            new Core.ValueObject.VehicleType(request.VehicleType), 
            new Core.ValueObject.LicensePlate(request.LicensePlate), 
            new Core.ValueObject.ProductionYear(request.ProductionYear), 
            new Core.ValueObject.Mileage(0));

        await repository.AddAsync(vehicle, cancellationToken);

        return new VehicleCreatedDto(vehicle.Id);
    }
}