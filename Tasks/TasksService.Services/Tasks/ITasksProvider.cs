using System;
using System.Collections.Generic;
using System.Threading;

namespace TasksService.Services.Tasks;

public interface ITasksProvider
{
    IAsyncEnumerable<TaskModel> GetAssignedTasks(Guid userId, CancellationToken cancellationToken);
}