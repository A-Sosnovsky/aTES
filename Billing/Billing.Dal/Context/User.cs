using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.DAL.Context;

[Table("users")]
public class User : IDbEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("roles")]
    public string[] Roles { get; set; }
}