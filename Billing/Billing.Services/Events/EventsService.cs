using System;
using System.Threading.Tasks;
using KafkaFlow;
using KafkaFlow.Producers;

namespace Billing.Services.Events;

internal sealed class EventsService : IEventsService
{
    private readonly IMessageProducer _producer;

    public EventsService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("billing-service");
    }

    
}