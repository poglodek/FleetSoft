using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Kafka;

public static class Extensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    { 
          return services;
    }
}