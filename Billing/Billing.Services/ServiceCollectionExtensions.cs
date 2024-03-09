using System;
using Billing.Services.EventHandlers;
using Billing.Services.Events;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace Billing.Services;

public static class ServiceCollectionExtensions
{
    private const string ServiceName = "tasks-service";
    //private static readonly string TopicName = Constants.TasksStreamTopicName;
    private static readonly string[] BrokenAddress = ["localhost:9092"];

    public static IServiceCollection AddEventsService(this IServiceCollection services)
    {
        services.AddScoped<IEventsService, EventsService>();
        //services.AddScoped<ITasksService, Tasks.TasksService>();
        //services.AddScoped<ITasksProvider, TasksProvider>();
        AddKafka(services);
        return services;
    }

    private static void AddKafka(IServiceCollection services)
    {
        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                //.AddCluster(
                //    cluster => cluster
                //        .WithBrokers(BrokenAddress)
                //        .CreateTopicIfNotExists(TopicName, 1, 1)
                //        .AddProducer(ServiceName,
                //            producer => producer
                //                .DefaultTopic(TopicName)
                //                .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())
                //        )
                //)
                // users stream consumer
                //.AddCluster(
                //    cluster => cluster
                //        .WithBrokers(BrokenAddress)
                //        .AddConsumer(consumer => consumer
                //            .Topic(Constants.UsersStreamTopicName)
                //            .WithGroupId(ServiceName)
                //            .WithBufferSize(100)
                //            .WithWorkersCount(1)
                //            .WithAutoOffsetReset(AutoOffsetReset.Latest)
                //            .AddMiddlewares(m => m
                //                .AddDeserializer<JsonCoreDeserializer>()
                //                .AddTypedHandlers(h =>
                //                    h.AddHandler<UserCreatedEventsHandler>()
                //                        .AddHandler<UserDeletedEventsHandler>()
                //                        .AddHandler<UserRoleChangedEventsHandler>()
                //                        .WhenNoHandlerFound(context =>
                //                        {
                //                            Console.WriteLine($"No handler found for message: {context.Message}");
                //                        })
                //                )
                //            )
                //        )
                //)
                //.AddCluster(
                //    cluster => cluster
                //        .WithBrokers(BrokenAddress)
                //        .AddConsumer(consumer => consumer
                //            .Topic(Constants.UsersStreamTopicName)
                //            .WithGroupId(ServiceName)
                //            .WithBufferSize(100)
                //            .WithWorkersCount(1)
                //            .WithAutoOffsetReset(AutoOffsetReset.Latest)
                //            .AddMiddlewares(m => m
                //                .AddDeserializer<JsonCoreDeserializer>()
                //                .AddTypedHandlers(h =>
                //                    h.WhenNoHandlerFound(context =>
                //                        {
                //                            Console.WriteLine($"No handler found for message: {context.Message}");
                //                        })
                //                )
                //            )
                //        )
                //)
                
        );
    }
}