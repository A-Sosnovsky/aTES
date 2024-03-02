using System.Threading.Tasks;
using Contract.Dto.Events.Users;
using KafkaFlow;

namespace TasksService.Services.EventHandlers;

public class UserDeletedEventsHandler : IMessageHandler<UserDeleted>
{
    public Task Handle(IMessageContext context, UserDeleted message)
    {
        throw new System.NotImplementedException();
    }
}