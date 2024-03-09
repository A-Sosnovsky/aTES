using System;
using System.Text.Json;
using Contract.Dto.Events.Tasks.Assigned;
using Contract.Dto.Events.Tasks.Completed;
using Contract.Dto.Events.Tasks.Created.V1;
using TasksService.DAL;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.Services.Events;

internal sealed class EventsService : IEventsService
{
    private readonly IRepository _repository;
    
    public EventsService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task TaskCreated(Guid taskPublicId, Guid taskAssignedToId, string description)
    {
        await _repository.InsertAsync(new MessageQueue
        {
            Type = MessageQueueType.TaskCreated,
            Value = JsonSerializer.Serialize(new TaskCreated
            {
                Id = taskPublicId,
                AssignedToId = taskAssignedToId,
                Description = description,
            })
        });
    }

    public async Task TaskCompleted(Guid taskPublicId, Guid taskCompletedById)
    {
        await _repository.InsertAsync(new MessageQueue
        {
            Type = MessageQueueType.TaskCompleted,
            Value = JsonSerializer.Serialize(new TaskCompleted
            {
                Id = taskPublicId,
                CompletedBy = taskCompletedById
            })
        });
    }

    public async Task TaskAssigned(Guid taskPublicId, Guid taskAssignedToId)
    {
        await _repository.InsertAsync(new MessageQueue
        {
            Type = MessageQueueType.TaskAssigned,
            Value = JsonSerializer.Serialize(new TaskAssigned
            {
                Id = taskPublicId,
                AssignedToId = taskAssignedToId
            })
        });
    }
}