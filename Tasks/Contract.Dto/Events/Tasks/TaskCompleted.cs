using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Tasks;

public class TaskCompleted
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}