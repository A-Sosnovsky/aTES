using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Users;

public class UserCreated
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}