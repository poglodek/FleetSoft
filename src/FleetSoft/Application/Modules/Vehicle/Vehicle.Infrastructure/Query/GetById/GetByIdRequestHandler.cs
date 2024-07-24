using MediatR;
using Shared.Core;
using Vehicle.Application.Dto;
using Vehicle.Core.Repositories;
using VehicleNotFoundException = Vehicle.Infrastructure.Exceptions.VehicleNotFoundException;

namespace Vehicle.Infrastructure.Query.GetById;

internal class GetByIdRequestHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetByIdRequest, VehicleDto>
{
    
    public async Task<VehicleDto> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(new Id(request.Id), cancellationToken);

        if (vehicle is null)
        {
            throw new VehicleNotFoundException($"Vehicle with id {request.Id} not found");
        }

        return vehicle;
    }
}