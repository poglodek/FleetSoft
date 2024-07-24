using MediatR;
using Vehicle.Application.Dto;

namespace Vehicle.Application.Command.CreateVehicle;

public record CreateVehicleRequest(string Brand, string Model, string VehicleType, string LicensePlate, int ProductionYear) : IRequest<VehicleCreatedDto>;