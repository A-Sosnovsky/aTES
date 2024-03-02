using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Tasks;

public class TaskAssigned
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("assigned_to")]
    public Guid AssignedToId { get; set; }
}