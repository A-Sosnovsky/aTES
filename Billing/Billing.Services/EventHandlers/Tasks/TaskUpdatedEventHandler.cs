using System;
using System.Threading.Tasks;
using Billing.DAL;
using Contract.Dto.Events.Tasks.Updated;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Services.EventHandlers.Tasks;

public class TaskUpdatedEventHandler : IMessageHandler<TaskUpdated>
{
    private readonly IServiceProvider _serviceProvider;

    public TaskUpdatedEventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(IMessageContext context, TaskUpdated message)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        
        var task = await repository.Query<DAL.Context.Task>().FirstOrDefaultAsync(t => t.PublicId == message.Id);
        if (task == null)
        {
            throw new NullReferenceException("Task not found");
        }
        
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        task.Description = message.Description;
        task.JiraId = message.JiraId;
        repository.Update(task);
        await unitOfWork.Commit();
    }
}