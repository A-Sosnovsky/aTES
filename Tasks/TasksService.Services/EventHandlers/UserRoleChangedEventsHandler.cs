using System.Threading.Tasks;
using Contract.Dto.Events.Users;
using KafkaFlow;

namespace TasksService.Services.EventHandlers;

public class UserRoleChangedEventsHandler : IMessageHandler<UserRoleChanged>
{
    public Task Handle(IMessageContext context, UserRoleChanged message)
    {
        throw new System.NotImplementedException();
    }
}