using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dal.Postgres.UnitOfWork;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public ChangeTracker GetChangeTracker();
}