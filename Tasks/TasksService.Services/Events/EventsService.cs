using System;
using System.Threading.Tasks;
using Contract.Dto.Events.Tasks;
using KafkaFlow;
using KafkaFlow.Producers;

namespace TasksService.Services.Events;

internal sealed class EventsService : IEventsService
{
    private readonly IMessageProducer _producer;

    public EventsService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("tasks-service");
    }

    public async Task TaskCreated(Guid taskPublicId, Guid taskAssignedToId, string description)
    {
        await _producer.ProduceAsync("task_created", new TaskCreated
        {
            Id = taskPublicId,
            AssignedToId = taskAssignedToId,
            Description = description,
        });
    }

    public async Task TaskCompleted(Guid taskPublicId, Guid taskCompletedById)
    {
        await _producer.ProduceAsync("task_completed", new TaskCompleted
        {
            Id = taskPublicId,
            CompletedBy = taskCompletedById
        });
    }

    public async Task TaskAssigned(Guid taskPublicId, Guid taskAssignedToId)
    {
        await _producer.ProduceAsync("task_assigned", new TaskAssigned
        {
            Id = taskPublicId,
            AssignedToId = taskAssignedToId
        });
    }
}