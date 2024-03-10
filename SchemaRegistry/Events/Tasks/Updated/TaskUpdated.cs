using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Tasks.Updated;

public class TaskUpdated
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("jira_id")]
    public required string JiraId { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}