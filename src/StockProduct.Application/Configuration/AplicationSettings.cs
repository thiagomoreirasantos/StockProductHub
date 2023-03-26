using StockProduct.Application.Interfaces;

namespace StockProduct.Application.Configuration
{
    public class ApplicationSettings : IApplicationSettings
    {
        public KafkaSettings Kafka { get; set; } = null!;
    }
}