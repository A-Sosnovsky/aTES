using System;
using Contract.Dto.Events.Users;
using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using TasksService.DAL;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.Services.EventHandlers;

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
        await repository.InsertAsync(new User
        {
            Id = message.Id,
            Name = message.Name,
            Roles = message.Roles
        });
        
        await unitOfWork.Commit();
    }
}