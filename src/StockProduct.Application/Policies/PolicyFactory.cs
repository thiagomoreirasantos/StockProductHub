using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using StockProduct.Application.Interfaces;

namespace StockProduct.Application.Policies
{
    public class PolicyFactory: IPolicyFactory
    {
        private readonly object randomLock = new();
        private readonly Random random = new();
        private readonly IKafkaBroker _kafkaBroker;
        private readonly IApplicationSettings _applicationSettings;
        private readonly ILogger<PolicyFactory> _logger;

        public PolicyFactory(IApplicationSettings applicationSettings,
            IKafkaBroker kafkaBroker,
            ILogger<PolicyFactory> logger)
        {
            _kafkaBroker = kafkaBroker;
            _applicationSettings = applicationSettings;
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            HttpStatusCode[] retriableCodes = _applicationSettings?.Kafka?.Destination?.Codes ?? Array.Empty<HttpStatusCode>();

            int jitter = 0;
            lock (randomLock)
                jitter = random.Next(0, 100);

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy
                            .HandleResult<HttpResponseMessage>(res => retriableCodes.Contains(res.StatusCode))
                            .WaitAndRetryAsync(
                                Backoff.ExponentialBackoff(
                                TimeSpan.FromSeconds(
                                _applicationSettings.Kafka.Destination.BaseExp)
                                + TimeSpan.FromMilliseconds(jitter),
                                retryCount: _applicationSettings.Kafka.Destination.RetryCount,
                                factor: _applicationSettings.Kafka.Destination.BaseExp),
                                onRetry: (response, sleepDuration, attemptNumber, context) =>
                                {
                                    _logger.LogError($"SleepDuration {sleepDuration} attempt {attemptNumber} statuscode {response.Result.StatusCode}");
                                    if (attemptNumber == 1)
                                    {
                                        if (!response.Result.IsSuccessStatusCode)
                                        {
                                            _kafkaBroker.Pause();
                                        }
                                    }
                                }
                            );

            IAsyncPolicy<HttpResponseMessage> fallbackPolicy =
                Policy.HandleResult<HttpResponseMessage>(res => retriableCodes.Contains(res.StatusCode))
                .FallbackAsync<HttpResponseMessage>(FallbackAction());

            return fallbackPolicy.WrapAsync(retryPolicy);
        }

        private HttpResponseMessage FallbackAction()
        {
            _kafkaBroker.Resume();

            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Retry policy finished")
            };
        }
    }
}