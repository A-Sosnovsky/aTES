using System;
using Billing.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace Billing.Services.Events;

public interface IEventsService
{
    Task TransactionCompleted(Guid transactionId, Guid userId, decimal amount, TransactionType transactionType);
    Task AccountCreated(Guid accountId, Guid userId);
}