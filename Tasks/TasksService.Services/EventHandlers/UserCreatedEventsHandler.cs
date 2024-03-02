using Contract.Dto.Events.Users;
using KafkaFlow;
using TasksService.DAL;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.Services.EventHandlers;

public class UserCreatedEventsHandler : IMessageHandler<UserCreated>
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UserCreatedEventsHandler(IRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(IMessageContext context, UserCreated message)
    {
        await _repository.InsertAsync(new User
        {
            Id = message.Id,
            Name = message.Name
        });
        
        await _unitOfWork.Commit();
    }
}