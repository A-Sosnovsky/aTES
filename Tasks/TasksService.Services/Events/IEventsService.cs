using System;
using System.Threading.Tasks;

namespace TasksService.Services.Events;

public interface IEventsService
{
    public Task TaskCreated(Guid taskPublicId, Guid taskAssignedToId, string description);
    public Task TaskCompleted();
    public Task TaskAssigned();
}