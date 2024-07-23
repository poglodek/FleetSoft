using Dal.Postgres.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Vehicle.Infrastructure.Database;

internal class VehicleDbContext : DbContext, IUnitOfWork
{
    /// <inheritdoc />
    public VehicleDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Vehicle");
    }

    public required DbSet<Core.Entity.Vehicle> Vehicles { get; init; }
    
    public ChangeTracker GetChangeTracker() => ChangeTracker;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) => Database.BeginTransactionAsync(cancellationToken);
}