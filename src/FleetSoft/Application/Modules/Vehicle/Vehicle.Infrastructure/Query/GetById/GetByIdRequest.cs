using MediatR;
using Vehicle.Application.Dto;

namespace Vehicle.Infrastructure.Query.GetById;

public record GetByIdRequest(Guid Id) : IRequest<VehicleDto>;
