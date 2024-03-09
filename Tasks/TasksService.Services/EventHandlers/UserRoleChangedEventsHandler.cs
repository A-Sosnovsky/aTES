using System;
using System.Linq;
using Contract.Dto.Events.Users;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
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
        var user = await repository.Query<User>().Where(u => u.Id == message.Id).FirstAsync();
        user.Roles = message.Roles;
        repository.Update(user);
        await unitOfWork.Commit();
    }
}