using System.Linq;
using System.Threading;
using Billing.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace Billing.DAL;

internal sealed class Repository : IRepository
{
    private readonly BillingDbContext _dbContext;

    public Repository(BillingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> Query<T>() where T : class, IDbEntity
    {
        return _dbContext.Set<T>();
    }

    public async Task InsertAsync<T>(T entity, CancellationToken cancellationToken) where T : class, IDbEntity
    {
        await _dbContext.AddAsync(entity, cancellationToken);
    }

    public void Update<T>(T entity, CancellationToken cancellationToken) where T : class, IDbEntity
    {
        _dbContext.Update(entity);
    }
}