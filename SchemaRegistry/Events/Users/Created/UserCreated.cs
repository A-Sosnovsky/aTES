using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Users.Created;

public class UserCreated
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("roles")]
    public required string[] Roles { get; set; }
}