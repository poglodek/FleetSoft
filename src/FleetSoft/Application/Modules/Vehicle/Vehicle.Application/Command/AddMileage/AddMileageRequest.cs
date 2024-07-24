using MediatR;

namespace Vehicle.Application.Command.AddMileage;

public record AddMileageRequest(Guid Id, double Mileage) : IRequest<Unit>;