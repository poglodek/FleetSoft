using Microsoft.EntityFrameworkCore;
using Shared.Core;
using Vehicle.Core.Dto;
using Vehicle.Core.Repositories;
using Vehicle.Infrastructure.Database;

namespace Vehicle.Infrastructure.Repositories;

internal class VehicleRepository(VehicleDbContext dbContext) : IVehicleRepository
{

    public async ValueTask AddAsync(Core.Entity.Vehicle vehicle,
        CancellationToken cancellationToken = default)
    {
         await dbContext.Vehicles.AddAsync(vehicle,cancellationToken);
    }
    

    public Task<Core.Entity.Vehicle?> GetByIdAsync(Id id, CancellationToken cancellationToken = default) 
        => dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id.Value == id.Value, cancellationToken);

    public async Task<IReadOnlyCollection<VehicleId>> GetAll(CancellationToken cancellationToken = default)
        => await dbContext.Vehicles.Where(x => !x.Archived.Value).Select(x=> new VehicleId(x.Id.Value)).ToListAsync(cancellationToken);
}