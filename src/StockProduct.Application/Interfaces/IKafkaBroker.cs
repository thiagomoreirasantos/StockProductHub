using StockProduct.Application.Dtos;

namespace StockProduct.Application.Interfaces
{
    public interface IKafkaBroker
    {
        Task Produce(StockProductOutput entity, string? partition = null);
        void Consume();
        void Pause();
        void Resume();
    }
}