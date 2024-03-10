using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Billing.TransactionCompleted;

public class TransactionCompleted
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("user_id")]
    public Guid AccountId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }
}