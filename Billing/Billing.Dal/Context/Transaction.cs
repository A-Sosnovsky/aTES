using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.DAL.Context;

[Table("transactions")]
public class Transaction : IDbEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("public_id")] 
    public Guid PublicId { get; set; }

    [Column("account_id")]
    public long AccountId { get; set; }

    [Column("billing_cycle_id")]
    public long BillingCycleId { get; set; }
    
    [Column("type")] 
    public TransactionType Type { get; set; }

    [Column("debit")]
    public decimal Debit { get; set; }

    [Column("credit")]
    public decimal Credit { get; set; }
    
    [Column("description")]
    public required string Description { get; set; } = null!;
    
    [Column("timestamp")]
    public DateTime Timespamp { get; set; }
    
    public Account Account { get; set; } = null!;
    
    public BillingCycle BillingCycle { get; set; } = null!;
}