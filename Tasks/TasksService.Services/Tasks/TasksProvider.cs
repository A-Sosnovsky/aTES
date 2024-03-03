using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using TasksService.DAL;
using TasksService.DAL.Context;

namespace TasksService.Services.Tasks;

internal sealed class TasksProvider : ITasksProvider
{
    private readonly IRepository _repository;

    public TasksProvider(IRepository repository)
    {
        _repository = repository;
    }

    public async IAsyncEnumerable<TaskModel> GetAssignedTasks(Guid userId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var tasks = await _repository.Query<Task>().Where(t => t.AssignedToId == userId && t.Status == TaskStatus.Assigned).ToListAsync(cancellationToken);
        foreach (var task in tasks)
        {
            yield return new TaskModel
            {
                Id = task.Id,
                Description = task.Description,
                PublicId = task.PublicId,
                Status = task.Status
            };
        }
    }
}