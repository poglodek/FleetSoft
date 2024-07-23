using Shared.Core;

namespace Vehicle.Core.Repositories;

public interface IVehicleRepository
{
    ValueTask AddAsync(Entity.Vehicle vehicle, CancellationToken cancellationToken = default);
    Task<Entity.Vehicle?> GetByIdAsync(Id id, CancellationToken cancellationToken = default);
}