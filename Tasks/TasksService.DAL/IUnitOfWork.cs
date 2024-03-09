using System;
using System.Threading;
using System.Threading.Tasks;

namespace TasksService.DAL;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> Commit(CancellationToken cancellationToken = default);
}