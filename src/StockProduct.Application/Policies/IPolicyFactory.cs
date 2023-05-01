using Polly;
using Polly.Contrib.Simmy.Outcomes;

namespace StockProduct.Application.Policies
{
    public interface IPolicyFactory
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
        AsyncInjectOutcomePolicy<HttpResponseMessage> GetChaosPolicy();
    }
}