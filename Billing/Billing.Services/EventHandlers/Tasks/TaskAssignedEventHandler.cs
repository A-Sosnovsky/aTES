using System.Threading.Tasks;
using Contract.Dto.Events.Tasks.Assigned;
using KafkaFlow;

namespace Billing.Services.EventHandlers.Tasks;

public class TaskAssignedEventHandler : IMessageHandler<TaskAssigned>
{
    public Task Handle(IMessageContext context, TaskAssigned message)
    {
        throw new System.NotImplementedException();
    }
}