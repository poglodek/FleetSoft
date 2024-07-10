using System.Text.Json;
using MediatR;
using Messaging.Kafka.Attributes;

namespace Messaging.Kafka.Events;

internal class IntegrationEventHandler : INotificationHandler<IIntegrationEvent>
{
    public IntegrationEventHandler()
    {
        
    }
    
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

        //TODO Implement Kafka Producer to send serialized event to Kafka
        Console.WriteLine($"Serialized Event on {topicName}: {serializedEvent}");

        return Task.CompletedTask;
    }
}