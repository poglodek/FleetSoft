using MediatR;
using Shared.Core;
using Vehicle.Application.Dto;
using Vehicle.Application.Exceptions;
using Vehicle.Core.Repositories;

namespace Vehicle.Infrastructure.Query.GetById;

public class GetByIdRequestHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetByIdRequest, VehicleDto>
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