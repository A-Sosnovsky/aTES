using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TasksService.DAL;
using TasksService.Services.Events;

namespace TasksService.Services.Tasks;

internal sealed class TasksService : ITasksService
{
    private readonly IEventsService _eventsService;
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public TasksService(IEventsService eventsService, IRepository repository, IUnitOfWork unitOfWork)
    {
        _eventsService = eventsService;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateTask(string description, CancellationToken cancellationToken)
    {
        var task = new DAL.Context.Task
        {
            PublicId = Guid.NewGuid(),
            AssignedToId = Guid.NewGuid(),
            Description = description,
            Status = DAL.Context.TaskStatus.Assigned
        };
        await _repository.InsertAsync(task, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);
        await _eventsService.TaskCreated(task.PublicId, task.AssignedToId, description);
    }

    public async Task CompleteTask(int id, CancellationToken cancellationToken)
    {
        var task = await _repository.Query<DAL.Context.Task>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException("Task not found");
        }

        task.Status = DAL.Context.TaskStatus.Completed;
        _repository.Update(task, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        await _eventsService.TaskCompleted();
    }

    public async Task AssignTasks()
    {
        await _eventsService.TaskAssigned();
    }
}