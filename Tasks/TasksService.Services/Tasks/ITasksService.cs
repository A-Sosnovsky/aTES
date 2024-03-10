using System;
using System.Threading;
using System.Threading.Tasks;

namespace TasksService.Services.Tasks;

public interface ITasksService
{
    Task CreateTask(string jiraId, string description, CancellationToken cancellationToken);
    Task CompleteTask(int taskId, Guid userId, CancellationToken cancellationToken);
    Task AssignTasks(Guid getUserId, CancellationToken cancellationToken);
}