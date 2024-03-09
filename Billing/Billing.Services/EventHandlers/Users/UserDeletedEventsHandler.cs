using System.Threading.Tasks;
using Contract.Dto.Events.Users.Deleted;
using KafkaFlow;

namespace Billing.Services.EventHandlers.Users;

public class UserDeletedEventsHandler : IMessageHandler<UserDeleted>
{
    public Task Handle(IMessageContext context, UserDeleted message)
    {
        throw new System.NotImplementedException();
    }
}