using System;
using System.Text.Json.Serialization;

namespace Contract.Dto.Events.Billing.AccountCreated;

public class AccountCreated
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("account_id")]
    public Guid AccountId { get; set; }
}