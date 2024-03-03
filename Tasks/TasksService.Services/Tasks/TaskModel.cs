using System;
using TasksService.DAL.Context;

namespace TasksService.Services.Tasks;

public class TaskModel
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public required string Description { get; set; } = null!;
    public TaskStatus Status { get; set; }
}