using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Users.Deleted;

public class UserDeleted
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}