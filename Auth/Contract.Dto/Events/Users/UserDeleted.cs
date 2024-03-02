using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Users;

public class UserDeleted
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}