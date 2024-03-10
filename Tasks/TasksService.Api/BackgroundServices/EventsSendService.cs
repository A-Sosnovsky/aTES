using System;
using System.Linq;
using System.Threading;
using Contract.Dto;
using Contract.Dto.Events.Tasks.Assigned;
using Contract.Dto.Events.Tasks.Completed;
using Contract.Dto.Events.Tasks.Created.V2;
using Contract.Dto.Events.Tasks.Updated;
using KafkaFlow.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TasksService.DAL;
using TasksService.DAL.Context;
using Task = System.Threading.Tasks.Task;

namespace TasksService.Api.BackgroundServices;

public class EventsSendService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public EventsSendService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private const int BatchSize = 10;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var producer = scope.ServiceProvider.GetRequiredService<IProducerAccessor>().GetProducer("tasks-service");

            var queueItems = await repository.Query<MessageQueue>()
                .OrderBy(q => q.Id)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            foreach (var item in queueItems)
            {
                switch (item.Type)
                {
                    case MessageQueueType.TaskCreated:
                        await SchemaValidator.Validate<TaskCreated>(item.Value);
                        await producer.ProduceAsync("task_created", item.Value);
                        break;
                    case MessageQueueType.TaskAssigned:
                        await SchemaValidator.Validate<TaskAssigned>(item.Value);
                        await producer.ProduceAsync("task_assigned", item.Value);
                        break;
                    case MessageQueueType.TaskCompleted:
                        await SchemaValidator.Validate<TaskCompleted>(item.Value);
                        await producer.ProduceAsync("task_completed", item.Value);
                        break;
                    case MessageQueueType.TaskUpdated:
                        await SchemaValidator.Validate<TaskUpdated>(item.Value);
                        await producer.ProduceAsync("task_updated", item.Value);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                repository.Delete(item, stoppingToken);
            }
            
            await unitOfWork.Commit(stoppingToken);
            
            if (!queueItems.Any())
            {
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}