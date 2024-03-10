using System;
using System.Linq;
using Billing.DAL;
using Billing.DAL.Context;
using Billing.Services.Events;
using Contract.Dto.Events.Tasks.Created.V2;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Billing.DAL.Context.TaskStatus;

namespace Billing.Services.EventHandlers.Tasks;

public class TaskCreatedV2EventHandler : IMessageHandler<TaskCreated>
{
    private readonly IServiceProvider _serviceProvider;

    public TaskCreatedV2EventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public async Task Handle(IMessageContext context, TaskCreated message)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var account = await repository.Query<Account>().FirstOrDefaultAsync(a => a.UserId == message.AssignedToId);
        
        // если еще не доехал пользователей, то падаем
        // можно будет сделать перекладывание в БД или в топик с ошибками для дальнейшей обработки
        if (account == null)
        {
            throw new NullReferenceException("Account not found");
        }

        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var billingCycleService = scope.ServiceProvider.GetRequiredService<IBillingCycleService>();
        var eventsService = scope.ServiceProvider.GetRequiredService<IEventsService>();

        var price = TaskPriceCalculator.CalculateAssignedPrice();
        var billingCycleId = await billingCycleService.GetCurrentBillingCycleId();
        
        var task = new DAL.Context.Task
        {
            PublicId = message.Id,
            Description = message.Description,
            Status = TaskStatus.Assigned,
            Cost = price,
            AssignedToId = message.AssignedToId,
            JiraId = message.JiraId
        };
        
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
            Timespamp = DateTime.UtcNow
        };
        
        repository.Update(account);
        await repository.InsertAsync(task);
        await repository.InsertAsync(transaction);

        await unitOfWork.Commit();
        
        await eventsService.TransactionCompleted(transaction.PublicId, account.PublicId, price, TransactionType.Withdrawal);
    }
}