using Polly;

namespace StockProduct.Application.Policies
{
    public interface IPolicyFactory
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    }
}