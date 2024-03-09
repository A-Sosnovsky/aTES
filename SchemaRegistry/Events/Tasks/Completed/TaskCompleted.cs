using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Tasks.Completed;

public class TaskCompleted
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("completed_by")]
    public Guid CompletedBy { get; set; }
}