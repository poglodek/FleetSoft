using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Messaging.Kafka.Producer;

internal class KafkaProducer : IKafkaProducer
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(ILogger<KafkaProducer> logger, string kafkaConnectionString )
    {
        _logger = logger;
        var config = new ProducerConfig { BootstrapServers = kafkaConnectionString };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    
    public async Task ProduceAsync(string topic, string message, CancellationToken ct = default)
    {
        try
        {
            var result =
                await _producer.ProduceAsync(topic, new Message<string, string> { Key = null, Value = message }, cancellationToken: ct);
            _logger.LogInformation(
                $"Message sent to topic {topic}, partition {result.Partition}, offset {result.Offset}");
        }
        catch (ProduceException<string, string> e)
        {
            _logger.LogError(e, $"Delivery failed: {e.Error.Reason}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Delivery failed: {e.Message}");
        }
    }
}