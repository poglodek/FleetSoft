namespace Messaging.Kafka.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MessageTopicAttribute(string topicName) : Attribute
{
    public string TopicName { get; } = topicName;
}