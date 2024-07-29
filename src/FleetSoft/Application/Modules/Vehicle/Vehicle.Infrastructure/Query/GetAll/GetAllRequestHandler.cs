using MediatR;
using Vehicle.Core.Dto;
using Vehicle.Core.Repositories;

namespace Vehicle.Infrastructure.Query.GetAll;

public class GetAllRequestHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetAllRequest,IReadOnlyCollection<VehicleId>>
{
    public async Task<IReadOnlyCollection<VehicleId>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        return await vehicleRepository.GetAll(cancellationToken);
    }
}