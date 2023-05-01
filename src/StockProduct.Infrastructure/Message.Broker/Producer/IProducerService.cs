using StockProduct.Application.Dtos;

namespace StockProduct.Infrastructure.Message.Broker.Producer
{
    public interface IProducerService
    {
        Task ProduceMessageAsync(StockProductOutput entity, string? key = null);
    }
}