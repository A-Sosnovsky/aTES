using System;
using Contract.Dto.Events.Users;
using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using TasksService.DAL;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.Services.EventHandlers;

public class UserRoleChangedEventsHandler : IMessageHandler<UserRoleChanged>
{
    private readonly IServiceProvider _serviceProvider;

    public UserRoleChangedEventsHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(IMessageContext context, UserRoleChanged message)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        // await repository.InsertAsync(new User
        // {
        //     Id = message.Id,
        //     Name = message.Name
        // });
        
        // await unitOfWork.Commit();
    }
}