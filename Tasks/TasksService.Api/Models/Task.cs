using TaskStatus = TasksService.DAL.Context.TaskStatus;

namespace TasksService.Api.Models;

public record Task(int Id, string Description, TaskStatus Status);