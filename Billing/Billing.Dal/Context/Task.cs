using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.DAL.Context;

[Table("tasks")]
public class Task : IDbEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("public_id")]
    public Guid PublicId { get; set; }
    
    [Column("jira_id")]
    public string? JiraId { get; set; }

    [Column("description")]
    public required string Description { get; set; } = null!;

    [Column("status")]
    public TaskStatus Status { get; set; }

    [Column("assigned_to_id")]
    public Guid AssignedToId { get; set; }
    
    [Column("cost")]
    public decimal Cost { get; set; }
    
    public User AssignedTo { get; set; } = null!;
}