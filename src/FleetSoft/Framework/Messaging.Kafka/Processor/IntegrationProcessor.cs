using MediatR;
using Messaging.Kafka.Events;

namespace Messaging.Kafka.Processor;

internal class IntegrationProcessor(IPublisher publisher) : IIntegrationProcessor
{
    public Task Publish(IIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return publisher.Publish(@event, cancellationToken);
    }
}