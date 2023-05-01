using System.Text;
using Microsoft.Extensions.Logging;
using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;
using StockProduct.Application.Policies;

namespace StockProduct.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ILogger<DeliveryService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPolicyFactory _policyFactory;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IKafkaBroker _kafkaBroker;

        public DeliveryService(ILogger<DeliveryService> logger,
             IHttpClientFactory httpClientFactory,
             IPolicyFactory policyFactory,
             IApplicationSettings applicationSettings,
             IKafkaBroker kafkaBroker)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _policyFactory = policyFactory;
            _applicationSettings = applicationSettings;
            _kafkaBroker = kafkaBroker;
        }

        public async Task DispatchAsync(StockProductData stockProductData)
        {
            try
            {
                if (stockProductData is null)
                    throw new ArgumentNullException(nameof(stockProductData));

                var client = _httpClientFactory.CreateClient(_applicationSettings.Kafka.Destination.Host);

                CancellationTokenSource cancellationTokenSource = new();
                var cancellationTokenToken = cancellationTokenSource.Token;

                var response = await _policyFactory
                .GetChaosPolicy().ExecuteAsync(
                    async () => await _policyFactory
                .GetRetryPolicy().ExecuteAsync(
                    async () => await client.SendAsync(new HttpRequestMessage(
                        new HttpMethod("POST"), _applicationSettings.Kafka.Destination.Host)
                    {
                        Content = new StringContent(stockProductData.ToString(), Encoding.UTF8, "application/json"),
                    })));

                if (response.IsSuccessStatusCode)
                {
                    _kafkaBroker.Resume();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}