namespace TasksService.Api.Models;

public class CreateTaskModel
{
    public required string Description { get; set; }
    public required string JiraId { get; set; }
}