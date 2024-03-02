using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksService.DAL;
using TaskStatus = TasksService.DAL.Context.TaskStatus;
using Task = System.Threading.Tasks.Task;
using TasksService.Api.Models;
using TasksService.Services;

namespace TasksService.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class TasksController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventsService _eventsService;

    public TasksController(IRepository repository, IUnitOfWork unitOfWork, IEventsService eventsService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventsService = eventsService;
    }

    [HttpGet]
    public async Task<IEnumerable<Models.Task>> Get(CancellationToken cancellationToken)
    {
        var tasks = await _repository.Query<DAL.Context.Task>().ToListAsync(cancellationToken);
        return tasks.Select(t => new Models.Task(t.Id, t.Description, t.Status));
    }

    [HttpPost]
    public async Task<int> Create(CreateTaskModel model, CancellationToken cancellationToken)
    {
        var task = new DAL.Context.Task
        {
            AssignedToId = model.AssignedToId,
            Description = model.Description,
            Status = TaskStatus.Assigned
        };
        await _repository.InsertAsync(task, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);
        await _eventsService.TaskCreated();
        return 1;
    }

    public async Task Done(int id, CancellationToken cancellationToken)
    {
        var task = await _repository.Query<DAL.Context.Task>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException("Task not found");
        }

        task.Status = TaskStatus.Completed;
        _repository.Update(task, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        await _eventsService.TaskCompleted();
    }

    public async Task AssignTasks(CancellationToken cancellationToken)
    {
        await _eventsService.TaskAssigned();
    }
}