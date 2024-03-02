using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TasksService.DAL.Context;

[PrimaryKey("UserId","Role")]
[Table("user_roles")]
public class UserRole : IDbEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }
    [Column("role")]
    public Role Role { get; set; }
}