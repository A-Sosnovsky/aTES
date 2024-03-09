using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksService.DAL.Context;

[Table("outbox_queue")]
public class MessageQueue : IDbEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }
    
    [Column("value")]
    public required string Value { get; set; }
    
    [Column("type")]
    public MessageQueueType Type { get; set; } 
}