using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;

namespace TasksService.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventsService(this IServiceCollection services)
    {
        services.AddScoped<IEventsService, EventsService>();
        AddKafka(services
        );
        return services;
    }

    private static void AddKafka(IServiceCollection services)
    {
        const string topicName = "sample-topic";
        const string producerName = "say-hello";

        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(new[] { "localhost:9092" })
                        .CreateTopicIfNotExists(topicName, 1, 1)
                        .AddProducer(
                            producerName,
                            producer => producer
                                .DefaultTopic(topicName)
                                .AddMiddlewares(m =>
                                    m.AddSerializer<JsonCoreSerializer>()
                                )
                        )
                )
        );
    }
}