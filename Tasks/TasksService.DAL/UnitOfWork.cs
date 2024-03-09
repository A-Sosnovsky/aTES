using System.Threading;
using System.Threading.Tasks;
using TasksService.DAL.Context;

namespace TasksService.DAL;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly TasksDbContext _dbDbContext;

    public UnitOfWork(TasksDbContext dbDbContext)
    {
        _dbDbContext = dbDbContext;
    }

    public void Dispose()
    {
        _dbDbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbDbContext.DisposeAsync();
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        return await _dbDbContext.SaveChangesAsync(cancellationToken);
    }
}