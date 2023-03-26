namespace StockProduct.Application.Configuration
{
    public class KafkaConsumerSettings
    {        
        public string Topic { get; set; }
        public string GroupId { get; set; }
        public int AutoCommitIntervalMs { get; set; }
        public string Name { get; set; }
        public int Workers { get; set; }
        public int BufferSize { get; set; }
        public bool EnsureMessageOrder { get; set; }
    }
}