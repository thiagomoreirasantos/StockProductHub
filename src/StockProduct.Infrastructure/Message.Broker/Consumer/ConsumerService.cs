using Confluent.Kafka;
using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;
using StockProduct.Infrastructure.Message.Broker.Consumer;

namespace StockProduct.Infrastructure.Message.Broker
{
    public class ConsumerService: IConsumerService
    {
        private readonly string _topicName;
        private readonly string _server;
        private readonly string _groupId;
        private readonly ConsumerConfig _consumerConfig;
        private readonly JsonCreationConverter<StockProductData> JsonCreationConverter;
        private readonly IApplicationSettings _applicationSettings;

        public ConsumerService(
            IApplicationSettings applicationSettings)
        {
            JsonCreationConverter = new StockProductConverter();
            _applicationSettings = applicationSettings;
            _topicName = _applicationSettings.Kafka.Consumers.Topic;
            _server =  String.Join(",", _applicationSettings.Kafka.Brokers.ToArray());

            _consumerConfig = new ConsumerConfig
            {
                GroupId = _groupId,
                BootstrapServers = _server,
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                EnablePartitionEof = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ApiVersionRequest = false,
            };
        }

        public void Consumer(CancellationTokenSource cancellationToken)
        {
            using var consumer = new ConsumerBuilder<string, StockProductData>(_consumerConfig)
                .SetKeyDeserializer(new JsonDeserializerKeyUTF8<string>())
                .SetValueDeserializer(new JsonDeserializerValueUTF8<StockProductData>(JsonCreationConverter))
                .Build();

            consumer.Subscribe(new[] { _topicName });

            try
            {
                while (true)
                {
                    try
                    {
                        var consumerResult = consumer.Consume(cancellationToken.Token);

                        var entityJsonMessage = consumerResult.Message.Value;

                        try
                        {
                            consumer.Commit(consumerResult);
                        }
                        catch (KafkaException e)
                        {
                            Console.WriteLine($"Commit Error: {e.Error.Reason}");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }

        public void Pause()
        {
            using var consumer = new ConsumerBuilder<string, StockProductData>(_consumerConfig)
                .SetKeyDeserializer(new JsonDeserializerKeyUTF8<string>())
                .SetValueDeserializer(new JsonDeserializerValueUTF8<StockProductData>(JsonCreationConverter))
                .Build();

            consumer.Pause(consumer.Assignment.ToList());
        }

        public void Resume()
        {
            using var consumer = new ConsumerBuilder<string, StockProductData>(_consumerConfig)
                .SetKeyDeserializer(new JsonDeserializerKeyUTF8<string>())
                .SetValueDeserializer(new JsonDeserializerValueUTF8<StockProductData>(JsonCreationConverter))
                .Build();

            consumer.Resume(consumer.Assignment.ToList());
        }
    }
}