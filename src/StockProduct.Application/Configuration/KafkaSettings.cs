namespace StockProduct.Application.Configuration
{
    public class KafkaSettings
    {
        public IList<string> Brokers { get; set; }
        public DestinationSettings Destination { get; set; }
        public KafkaConsumerSettings Consumers { get; set; }
        public KafkaProducerSettings Producers { get; set; }
    }
}