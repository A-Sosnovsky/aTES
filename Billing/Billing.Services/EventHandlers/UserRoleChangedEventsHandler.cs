using System;
using KafkaFlow;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace Billing.Services.EventHandlers;

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