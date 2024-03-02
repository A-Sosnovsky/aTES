using System;
using System.Threading.Tasks;
using Contract.Dto.Events.Users;
using JwtRoleAuthentication.Services;
using KafkaFlow;
using KafkaFlow.Producers;

namespace JwtRoleAuthentication.Events;

internal sealed class EventsService : IEventsService
{
    private readonly IMessageProducer _producer;

    public EventsService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("auth-service");
    }

    public async Task UserCreated(Guid publicId, string name, string[] roles)
    {
        await _producer.ProduceAsync("user_created", new UserCreated
        {
            Id = publicId,
            Name = name, 
            Roles = roles
        });
    }

    public async Task UserRolesChanged(Guid publicId, string[] roles)
    {   
        await _producer.ProduceAsync("user_role_changed", new UserRoleChanged
        {
            Id = publicId,
            Roles = roles
        });
    }
}