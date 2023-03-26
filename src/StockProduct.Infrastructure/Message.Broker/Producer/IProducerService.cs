using StockProduct.Application.Dtos;

namespace StockProduct.Infrastructure.Message.Broker.Producer
{
    public interface IProducerService
    {
        Task ProduceMessageAsync(StockProductData entity, string? key = null);
    }
}