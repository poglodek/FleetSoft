using System.Reflection;
using Messaging.Kafka.Processor;
using Messaging.Kafka.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Kafka;

public static class Extensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIntegrationProcessor, IIntegrationProcessor>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        var kafkaConnectionString = configuration.GetConnectionString("kafka");
        if (string.IsNullOrWhiteSpace(kafkaConnectionString))
        {
            throw new ArgumentNullException(nameof(kafkaConnectionString));
        }
        
        services.AddSingleton<IKafkaProducer>(_ => new KafkaProducer());
        
        
          return services;
    }
}