using System;
using System.Threading.Tasks;

namespace JwtRoleAuthentication.Events;

public interface IEventsService
{
    Task UserCreated(Guid publicId, string name, string[] roles);
    Task UserRolesChanged(Guid publicId, string[] roles);
}