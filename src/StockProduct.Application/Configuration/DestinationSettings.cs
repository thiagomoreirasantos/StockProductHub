using System.Net;

namespace StockProduct.Application.Configuration
{
    public class DestinationSettings
    {
        public string Host { get; set; }
        public int RetryCount { get; set; }
        public int BaseExp { get; set; }
        public HttpStatusCode[] Codes { get; set; }
    }
}