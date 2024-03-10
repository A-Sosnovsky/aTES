using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Contract.Dto.References;
using Microsoft.EntityFrameworkCore;
using TasksService.DAL;
using TasksService.DAL.Context;
using TasksService.Services.Events;
using Task = System.Threading.Tasks.Task;
using TaskStatus = TasksService.DAL.Context.TaskStatus;

namespace TasksService.Services.Tasks;

internal sealed class TasksService : ITasksService
{
    private readonly IEventsService _eventsService;
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string[] _assignAllowedRoles = [Roles.Administrator, Roles.Manager];

    public TasksService(IEventsService eventsService, IRepository repository, IUnitOfWork unitOfWork)
    {
        _eventsService = eventsService;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateTask(string jiraId, string description, CancellationToken cancellationToken)
    {
        if (jiraId.Contains('[') || jiraId.Contains(']'))
        {
            throw new InvalidOperationException("JiraId contains invalid characters");
        }

        var assignedToId = await GetRandomPopugId(cancellationToken);
        var task = new DAL.Context.Task
        {
            JiraId = jiraId,
            PublicId = Guid.NewGuid(),
            AssignedToId = assignedToId,
            Description = description,
            Status = TaskStatus.Assigned
        };

        await _repository.InsertAsync(task, cancellationToken);
        await _eventsService.TaskCreated(task.PublicId, task.AssignedToId, description);
        await _unitOfWork.Commit(cancellationToken);
    }

    public async Task CompleteTask(int id, Guid userId, CancellationToken cancellationToken)
    {
        var task = await _repository.Query<DAL.Context.Task>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException("Task not found");
        }

        if (task.AssignedToId != userId)
        {
            throw new InvalidOperationException("Task is not assigned to current user");
        }

        if (task.Status == TaskStatus.Completed)
        {
            throw new InvalidOperationException("Task already completed");
        }

        task.Status = TaskStatus.Completed;
        _repository.Update(task, cancellationToken);
        await _eventsService.TaskCompleted(task.PublicId, userId);
        await _unitOfWork.Commit(cancellationToken);
    }

    public async Task AssignTasks(Guid userId, CancellationToken cancellationToken)
    {
        var currentUserHasAccess = await _repository.Query<User>()
            .AnyAsync(u => u.Id == userId && u.Roles.Any(ur => _assignAllowedRoles.Contains(ur)), cancellationToken);
        if (!currentUserHasAccess)
        {
            throw new UnauthorizedAccessException();
        }

        var openedTasks = await _repository.Query<DAL.Context.Task>().Where(t => t.Status == TaskStatus.Assigned).ToListAsync(cancellationToken);
        foreach (var openedTask in openedTasks)
        {
            openedTask.AssignedToId = await GetRandomPopugId(cancellationToken);
            await _eventsService.TaskAssigned(openedTask.PublicId, openedTask.AssignedToId);
            await _unitOfWork.Commit(cancellationToken);
        }
    }

    private async Task<Guid> GetRandomPopugId(CancellationToken cancellationToken)
    {
        return await _repository.Query<User>().Where(u => u.Roles.Contains(Roles.Popug))
            .OrderBy(u => Guid.NewGuid())
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}