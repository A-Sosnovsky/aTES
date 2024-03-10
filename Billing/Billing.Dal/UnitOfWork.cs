using System.Threading;
using System.Threading.Tasks;
using Billing.DAL.Context;

namespace Billing.DAL;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly BillingDbContext _dbContext;

    public UnitOfWork(BillingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}