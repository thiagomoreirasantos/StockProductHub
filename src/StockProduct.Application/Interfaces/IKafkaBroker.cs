using StockProduct.Application.Dtos;

namespace StockProduct.Application.Interfaces
{
    public interface IKafkaBroker
    {
        Task Produce(StockProductData entity, string? partition = null);
        void Consume();
        void Pause();
        void Resume();
    }
}