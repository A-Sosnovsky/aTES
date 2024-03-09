using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksService.DAL.Context;

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