namespace Messaging.Kafka.Producer;

internal interface IKafkaProducer
{
    Task ProduceAsync(string topic, string message, CancellationToken ct = default);
}