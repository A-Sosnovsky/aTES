using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Users;

public class UserRoleChanged
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; }
}