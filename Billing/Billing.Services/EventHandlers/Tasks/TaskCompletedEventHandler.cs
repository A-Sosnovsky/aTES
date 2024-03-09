using System.Threading.Tasks;
using Contract.Dto.Events.Tasks.Completed;
using KafkaFlow;

namespace Billing.Services.EventHandlers.Tasks;

public class TaskCompletedEventHandler : IMessageHandler<TaskCompleted>
{
    public Task Handle(IMessageContext context, TaskCompleted message)
    {
        throw new System.NotImplementedException();
    }
}