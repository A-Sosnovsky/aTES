using System;
using System.Threading;
using System.Threading.Tasks;

namespace Billing.DAL;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> Commit(CancellationToken cancellationToken = default);
}