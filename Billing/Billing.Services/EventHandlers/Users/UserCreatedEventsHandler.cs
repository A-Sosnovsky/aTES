using System;
using Billing.DAL;
using Billing.DAL.Context;
using Billing.Services.Events;
using Contract.Dto.Events.Users.Created;
using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace Billing.Services.EventHandlers.Users;

public class UserCreatedEventsHandler : IMessageHandler<UserCreated>
{
    private readonly IServiceProvider _serviceProvider;
    
    public UserCreatedEventsHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(IMessageContext context, UserCreated message)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var eventService = scope.ServiceProvider.GetRequiredService<IEventsService>();
        
        var user = new User
        {
            Id = message.Id,
            Name = message.Name,
            Roles = message.Roles
        };
        
        await repository.InsertAsync(user);

        var accountId = Guid.NewGuid();
        await repository.InsertAsync(new Account
        {
            Balance = 0,
            PublicId = accountId,
            User = user,
        });
        
        await unitOfWork.Commit();
        await eventService.AccountCreated(accountId, user.Id);
    }
}