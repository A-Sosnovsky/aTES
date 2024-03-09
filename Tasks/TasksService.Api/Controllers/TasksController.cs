using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;
using TasksService.Api.Models;
using TasksService.Services.Tasks;

namespace TasksService.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;
    private readonly ITasksProvider _tasksProvider;

    public TasksController(ITasksService tasksService, ITasksProvider tasksProvider)
    {
        _tasksService = tasksService;
        _tasksProvider = tasksProvider;
    }

    [HttpGet]
    public async IAsyncEnumerable<Models.Task> Get([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var tasks = _tasksProvider.GetAssignedTasks(this.GetUserId(), cancellationToken);
        await foreach (var task in tasks)
        {
            yield return new Models.Task(task.Id, task.Description, task.Status);
        }
    }

    [HttpPost]
    public async Task Create(CreateTaskModel model, CancellationToken cancellationToken)
    {
        await _tasksService.CreateTask(model.JiraId, model.Description, cancellationToken);
    }

    [HttpPut("{id:int}/done")]
    public async Task Done(int id, CancellationToken cancellationToken)
    {
        await _tasksService.CompleteTask(id, this.GetUserId(), cancellationToken);
    }

    [HttpPost("assign")]
    public async Task AssignTasks(CancellationToken cancellationToken)
    {
        await _tasksService.AssignTasks(this.GetUserId(), cancellationToken);
    }
}