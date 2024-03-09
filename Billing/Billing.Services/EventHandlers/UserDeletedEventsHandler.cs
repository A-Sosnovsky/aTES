using System.Threading.Tasks;
using KafkaFlow;

namespace Billing.Services.EventHandlers;

public class UserDeletedEventsHandler : IMessageHandler<UserDeleted>
{
    public Task Handle(IMessageContext context, UserDeleted message)
    {
        throw new System.NotImplementedException();
    }
}