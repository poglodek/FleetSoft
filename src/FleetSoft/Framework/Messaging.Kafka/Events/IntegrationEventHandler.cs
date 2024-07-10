using System.Text.Json;
using MediatR;
using Messaging.Kafka.Attributes;
using Messaging.Kafka.Producer;

namespace Messaging.Kafka.Events;

internal class IntegrationEventHandler(IKafkaProducer kafkaProducer) : INotificationHandler<IIntegrationEvent>
{
    public Task Handle(IIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var topicAttribute = (MessageTopicAttribute)notification.GetType()
            .GetCustomAttributes(typeof(MessageTopicAttribute), false)
            .FirstOrDefault()!;

        if (topicAttribute is null)
        {
            throw new InvalidOperationException("Event does not have a MessageTopicAttribute.");
        }

        var topicName = topicAttribute.TopicName;
        if(string.IsNullOrWhiteSpace(topicName))
        {
            throw new InvalidOperationException("Event does not have a valid TopicName.");
        }

        var serializedEvent = JsonSerializer.Serialize(notification);
        

        return kafkaProducer.ProduceAsync(topicName, serializedEvent, cancellationToken);;
    }
}