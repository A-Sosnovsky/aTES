using System.Threading;
using System.Threading.Tasks;

namespace TasksService.Services.Tasks;

public interface ITasksService
{
    Task CreateTask(string description, CancellationToken cancellationToken);
    Task CompleteTask(int taskId, CancellationToken cancellationToken);
    Task AssignTasks();
}