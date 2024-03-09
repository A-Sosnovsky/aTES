using System.Threading.Tasks;
using Contract.Dto.Events.Tasks.Created.V2;
using KafkaFlow;

namespace Billing.Services.EventHandlers.Tasks;

public class TaskCreatedV2EventHandler : IMessageHandler<TaskCreated>
{
    public Task Handle(IMessageContext context, TaskCreated message)
    {
        throw new System.NotImplementedException();
    }
}