using System.Linq;
using System.Threading;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.DAL;

internal sealed class Repository : IRepository
{
    private readonly TasksDbContext _dbDbContext;

    public Repository(TasksDbContext dbDbContext)
    {
        _dbDbContext = dbDbContext;
    }

    public IQueryable<T> Query<T>() where T : class, IDbEntity
    {
        return _dbDbContext.Set<T>();
    }

    public async Task InsertAsync<T>(T entity, CancellationToken cancellationToken) where T : class, IDbEntity
    {
        await _dbDbContext.AddAsync(entity, cancellationToken);
    }

    public void Update<T>(T entity, CancellationToken cancellationToken) where T : class, IDbEntity
    {
        _dbDbContext.Update(entity);
    }
}