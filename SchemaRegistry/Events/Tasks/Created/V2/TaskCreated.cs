using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Tasks.Created.V2;

public class TaskCreated
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("jira_id")]
    public required string JiraId { get; set; }
    
    [JsonPropertyName("assigned_to")]
    public Guid AssignedToId { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}