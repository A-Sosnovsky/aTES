using System.Threading.Tasks;

namespace TasksService.Services;

public interface IEventsService
{
    public Task TaskCreated();
    public Task TaskCompleted();
    public Task TaskAssigned();
}