using System;
using Billing.DAL;
using Billing.DAL.Context;
using Billing.Services.Events;
using Contract.Dto.Events.Tasks.Assigned;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace Billing.Services.EventHandlers.Tasks;

public class TaskAssignedEventHandler : IMessageHandler<TaskAssigned>
{
    private readonly IServiceProvider _serviceProvider;

    public TaskAssignedEventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(IMessageContext context, TaskAssigned message)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var account = await repository.Query<Account>().FirstOrDefaultAsync(a => a.UserId == message.AssignedToId);
        
        // если еще не доехал пользователь или таска, то падаем
        // можно будет сделать перекладывание в БД или в топик с ошибками для дальнейшей обработки
        if (account == null)
        {
            throw new NullReferenceException("Account not found");
        }
        
        var task = await repository.Query<DAL.Context.Task>().FirstOrDefaultAsync(t => t.PublicId == message.Id);
        if (task == null)
        {
            throw new NullReferenceException("Task not found");
        }

        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var billingCycleService = scope.ServiceProvider.GetRequiredService<IBillingCycleService>();
        var eventsService = scope.ServiceProvider.GetRequiredService<IEventsService>();

        var price = TaskPriceCalculator.CalculateAssignedPrice();
        var billingCycleId = await billingCycleService.GetCurrentBillingCycleId();

        task.AssignedToId = message.AssignedToId;
        
        account.Balance -= price;
        var transaction = new Transaction
        {
            Description = "Task assigned to user",
            Credit = price,
            Debit = 0,
            PublicId = Guid.NewGuid(),
            BillingCycleId = billingCycleId,
            Type = TransactionType.Withdrawal,
            AccountId = account.Id,
            Timespamp = DateTime.UtcNow, 
        };
        
        repository.Update(task);
        repository.Update(account);
        await repository.InsertAsync(transaction);

        await unitOfWork.Commit();
        
        await eventsService.TransactionCompleted(transaction.PublicId, account.PublicId, price, TransactionType.Withdrawal);
    }
}