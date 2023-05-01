using StockProduct.Application.Dtos;

namespace StockProduct.Application.Interfaces
{
    public interface IDeliveryService
    {
        Task DispatchAsync(StockProductInput stockProductData);
    }
}