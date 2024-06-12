using System.Text.Json;
using MediatR;
using Messaging.Kafka.Attributes;
using Shared.Core;

namespace Messaging.Kafka.Handlers;

internal class DomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    public Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var topicAttribute = (MessageTopicAttribute)domainEvent.GetType()
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
        
        var serializedEvent = JsonSerializer.Serialize(domainEvent);

        //TODO Implement Kafka Producer to send serialized event to Kafka
        Console.WriteLine($"Serialized Event on {topicName}: {serializedEvent}");

        return Task.CompletedTask;
    }
}