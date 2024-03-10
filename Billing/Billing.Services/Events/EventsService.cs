using System;
using Billing.DAL.Context;
using Contract.Dto.Events.Billing.AccountCreated;
using Contract.Dto.Events.Billing.TransactionCompleted;
using KafkaFlow;
using KafkaFlow.Producers;
using Task = System.Threading.Tasks.Task;

namespace Billing.Services.Events;

internal sealed class EventsService : IEventsService
{
    private readonly IMessageProducer _producer;

    public EventsService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("billing-service");
    }

    public async Task TransactionCompleted(Guid transactionId, Guid accountId, decimal amount, TransactionType transactionType)
    {
        await _producer.ProduceAsync("transaction_completed", new TransactionCompleted
        {
            Id = transactionId,
            AccountId = accountId,
            Amount = amount,
            Type = transactionType.ToString()
        });
    }
    
    public async Task AccountCreated(Guid accountId, Guid userId)
    {
        await _producer.ProduceAsync("account_created", new AccountCreated
        {
            AccountId = accountId,
            UserId = userId
        });
    }
}