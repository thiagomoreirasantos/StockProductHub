using Confluent.Kafka;
using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;

namespace StockProduct.Infrastructure.Message.Broker.Producer
{
    public class ProducerService : IProducerService
    {
        private readonly string _servers;
        private readonly string _topicName;
        private readonly ProducerConfig _producerConfig = null!;
        private readonly IApplicationSettings _applicationSettings;

        public ProducerService(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
            _servers = String.Join(",",applicationSettings.Kafka.Brokers.ToArray());
            _topicName = _applicationSettings.Kafka.Consumers.Topic;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _servers,
            };
        }

        public async Task ProduceMessageAsync(StockProductData entity, string? key = null)
        {
            using var p = new ProducerBuilder<string, StockProductData>(_producerConfig)
                .SetKeySerializer(new JsonSerializerUTF8<string>())
                .SetValueSerializer(new JsonSerializerUTF8<StockProductData>())
                .Build();

            var message = new Message<string, StockProductData> { Key = key ??= string.Empty, Value = entity };

            await p.ProduceAsync(_topicName, message);
        }
    }
}