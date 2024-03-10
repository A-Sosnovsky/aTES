using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Contract.Dto.Events.Tasks.Updated;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TasksService.DAL;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.Api.BackgroundServices;

public class TaskJiraIdGenerateService : BackgroundService
{
    private const int BatchSize = 100;
    private readonly IServiceProvider _serviceProvider;
    private const char Start = '[';
    private const char End = ']';

    public TaskJiraIdGenerateService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var tasks = await repository
                .Query<DAL.Context.Task>()
                .Where(t => t.JiraId.Length == 0)
                .OrderBy(t => t.Id)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            foreach (var task in tasks)
            {
                if (TryExtractJiraId(task.Description, out var jiraId))
                {
                    task.JiraId = jiraId!;
                    task.Description = task.Description.Replace(jiraId!, "");
                }
                else
                {
                    task.JiraId = $"UBEPOPUP-{task.Id}";
                }

                await repository.InsertAsync(new MessageQueue
                {
                    Value = JsonSerializer.Serialize(new TaskUpdated
                    {
                        Id = task.PublicId,
                        Description = task.Description,
                        JiraId = task.JiraId,
                    }),
                    Type = MessageQueueType.TaskUpdated
                }, stoppingToken);
            }

            await unitOfWork.Commit(stoppingToken);

            if (!tasks.Any())
            {
                return;
            }
        }
    }

    private static bool TryExtractJiraId(string token, out string? jiraId)
    {
        jiraId = null;
        if (!token.Contains(Start)) return false;

        var afterFirst = token.Split(new[] { Start }, StringSplitOptions.None)[1];

        if (!afterFirst.Contains(End)) return false;

        jiraId = afterFirst.Split(new[] { End }, StringSplitOptions.None)[0];
        return true;
    }
}