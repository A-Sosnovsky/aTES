using System;
using Contract.Dto.Events;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using TasksService.Services.EventHandlers;
using TasksService.Services.Events;
using TasksService.Services.Tasks;

namespace TasksService.Services;

public static class ServiceCollectionExtensions
{
    private const string ServiceName = "tasks-service";
    private static readonly string TopicName = Constants.TasksTopicName;
    private static readonly string[] BrokenAddress = ["localhost:9092"];

    public static IServiceCollection AddEventsService(this IServiceCollection services)
    {
        services.AddScoped<IEventsService, EventsService>();
        services.AddScoped<ITasksService, Tasks.TasksService>();
        AddKafka(services);
        return services;
    }

    private static void AddKafka(IServiceCollection services)
    {
        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(BrokenAddress)
                        .CreateTopicIfNotExists(TopicName, 1, 1)
                        .AddProducer(ServiceName,
                            producer => producer
                                .DefaultTopic(TopicName)
                                .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())
                        )
                )
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(BrokenAddress)
                        .AddConsumer(consumer => consumer
                            .Topic(Constants.UsersTopicName)
                            .WithGroupId(ServiceName)
                            .WithBufferSize(100)
                            .WithWorkersCount(1)
                            .WithAutoOffsetReset(AutoOffsetReset.Latest)
                            .AddMiddlewares(m => m
                                .AddDeserializer<JsonCoreDeserializer>()
                                .AddTypedHandlers(h =>
                                    h.AddHandler<UserCreatedEventsHandler>()
                                        .AddHandler<UserDeletedEventsHandler>()
                                        .AddHandler<UserRoleChangedEventsHandler>()
                                        .WhenNoHandlerFound(context => { Console.WriteLine($"No handler found for message: {context.Message}"); })
                                )
                            )
                        )
                )
        );
    }
}