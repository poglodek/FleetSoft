using Shared.Core;

namespace Vehicle.Core.Repositories;

public interface IVehicleRepository
{
    Task AddAsync(Entity.Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entity.Vehicle vehicle, CancellationToken cancellationToken = default);
    Task<Entity.Vehicle> GetByIdAsync(Id id, CancellationToken cancellationToken = default);
}