namespace StockProduct.Application.Configuration
{
    public class KafkaProducerSettings
    {        
        public string Topic { get; set; }
        public string Acks { get; set; }
    }
}