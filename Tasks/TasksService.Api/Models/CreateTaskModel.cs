using System;

namespace TasksService.Api.Models;

public class CreateTaskModel
{
    public Guid AssignedToId { get; set; }
    public string Description { get; set; }
}