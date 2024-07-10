using System.Reflection;
using Messaging.Kafka.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Kafka;

public static class Extensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IIntegrationProcessor, IIntegrationProcessor>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        
        
        
          return services;
    }
}