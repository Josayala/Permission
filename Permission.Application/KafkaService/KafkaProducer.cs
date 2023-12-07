using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Permission.Application.Abstractions;
using Permission.Infrastructure.Dtos;
using System.Runtime;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Text.Json;

namespace Permission.Application.KafkaService
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly KafkaSettings _settings;

        public KafkaProducer(IOptions<KafkaSettings> settings)
        {
            var config = new ProducerConfig { BootstrapServers = settings.Value.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _settings = settings.Value;
        }

        public async Task ProduceAsync(OperationMessageDto operatioMessage)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(operatioMessage)
            };

            try
            {
                var dr = await _producer.ProduceAsync(_settings.Topic, message);
                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
