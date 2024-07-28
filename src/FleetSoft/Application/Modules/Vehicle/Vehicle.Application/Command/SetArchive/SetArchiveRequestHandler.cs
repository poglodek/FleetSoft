using MediatR;
using Vehicle.Application.Exceptions;
using Vehicle.Core.Repositories;

namespace Vehicle.Application.Command.SetArchive;

public class SetArchiveRequestHandler(IVehicleRepository repository, TimeProvider timeProvider) : IRequestHandler<SetArchiveRequest, Unit>
{
    public async Task<Unit> Handle(SetArchiveRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (vehicle is null)
        {
            throw new VehicleNotFoundException($"Vehicle with id {request.Id} not found");
        }
        
        vehicle.SetVehicleAsArchived(timeProvider);
        
        
        return Unit.Value;
    }
}