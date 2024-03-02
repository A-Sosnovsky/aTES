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
using TasksService.Services.Events;
using TasksService.Services.Tasks;

namespace TasksService.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class TasksController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventsService _eventsService;
    private readonly ITasksService _tasksService;

    public TasksController(IRepository repository, IUnitOfWork unitOfWork, IEventsService eventsService, ITasksService tasksService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventsService = eventsService;
        _tasksService = tasksService;
    }

    [HttpGet]
    public async Task<IEnumerable<Models.Task>> Get(CancellationToken cancellationToken)
    {
        var tasks = await _repository.Query<DAL.Context.Task>().ToListAsync(cancellationToken);
        return tasks.Select(t => new Models.Task(t.Id, t.Description, t.Status));
    }

    [HttpPost]
    public async Task Create(CreateTaskModel model, CancellationToken cancellationToken)
    {
        await _tasksService.CreateTask(model.Description, cancellationToken);
    }

    [HttpPut("{id:int}/done")]
    public async Task Done(int id, CancellationToken cancellationToken)
    {
        await _tasksService.CompleteTask(id, cancellationToken);
    }

    [HttpPost("assign")]
    public async Task AssignTasks(CancellationToken cancellationToken)
    {
        await _eventsService.TaskAssigned();
    }
}