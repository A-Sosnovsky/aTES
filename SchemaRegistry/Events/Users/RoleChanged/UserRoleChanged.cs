using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Users.RoleChanged;

public class UserRoleChanged
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("roles")]
    public required string[] Roles { get; set; }
}