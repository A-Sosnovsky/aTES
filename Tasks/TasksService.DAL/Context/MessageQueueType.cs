namespace TasksService.DAL.Context;

public enum MessageQueueType
{
    TaskCreated = 1,
    TaskAssigned = 2,
    TaskCompleted = 3,
    TaskUpdated = 4
}