using Messaging.Kafka.Events;

namespace Messaging.Kafka.Processor;

public interface IIntegrationProcessor
{
    Task Publish(IIntegrationEvent @event, CancellationToken cancellationToken = default);
}