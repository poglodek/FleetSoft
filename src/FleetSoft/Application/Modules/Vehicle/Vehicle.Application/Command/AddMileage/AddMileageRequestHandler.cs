using MediatR;
using Vehicle.Application.Exceptions;
using Vehicle.Core.Repositories;

namespace Vehicle.Application.Command.AddMileage;

public class AddMileageRequestHandler(IVehicleRepository repository): IRequestHandler<AddMileageRequest,Unit>
{
    
    public async Task<Unit> Handle(AddMileageRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (vehicle is null)
        {
            throw new VehicleNotFoundException($"Vehicle not found with the given id ({request.Id}).");
        }
        
        vehicle.AddMileage(request.Mileage);

        return Unit.Value;
    }
}