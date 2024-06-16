using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dal.Postgres.UnitOfWork;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public ChangeTracker GetChangeTracker();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}