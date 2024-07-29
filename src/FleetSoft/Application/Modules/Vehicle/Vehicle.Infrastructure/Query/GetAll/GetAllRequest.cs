using MediatR;
using Vehicle.Core.Dto;

namespace Vehicle.Infrastructure.Query.GetAll;

public record GetAllRequest() : IRequest<IReadOnlyCollection<VehicleId>>;