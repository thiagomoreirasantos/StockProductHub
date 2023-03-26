namespace StockProduct.Infrastructure.Message.Broker.Consumer
{
    public interface IConsumerService
    {
        void Consumer(CancellationTokenSource cancellationToken);
        void Pause();
        void Resume();
    }
}