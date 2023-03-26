using StockProduct.Application.Configuration;

namespace StockProduct.Application.Interfaces
{
    public interface IApplicationSettings
    {
        KafkaSettings Kafka { get; set; }
    }
}