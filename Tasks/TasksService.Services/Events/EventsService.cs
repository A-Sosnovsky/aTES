using System;
using System.Threading.Tasks;
using KafkaFlow;
using KafkaFlow.Producers;

namespace TasksService.Services.Events;

internal sealed class EventsService : IEventsService
{
    private readonly IMessageProducer _producer;

    public EventsService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("say-hello");
    }

    public async Task TaskCreated(Guid taskPublicId, Guid taskAssignedToId, string description)
    {
        await _producer.ProduceAsync("sample-topic", new { Message = "TaskCreated" });
    }

    public async Task TaskCompleted()
    {
        await _producer.ProduceAsync("sample-topic", new { Message = "TaskCompleted" });
    }

    public async Task TaskAssigned()
    {
        await _producer.ProduceAsync("sample-topic", new { Message = "TaskAssigned" });
    }
}