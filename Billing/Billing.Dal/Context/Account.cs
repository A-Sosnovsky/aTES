using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.DAL.Context;

[Table("accounts")]
public class Account : IDbEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("public_id")] 
    public Guid PublicId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("balance")]
    public decimal Balance { get; set; }
    
    public User User { get; set; } = null!;
}