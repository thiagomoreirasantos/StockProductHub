using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;

namespace StockProduct.Application.Services
{
    public class StockProductService : IStockProductService
    {
        private readonly IKafkaBroker _kafkaBroker;

        public StockProductService(IKafkaBroker kafkaBroker)
        {
            _kafkaBroker = kafkaBroker;
        }

        public async Task HandleMessage(StockProductOutput stockProductOutput)
        {
            var result = await stockProductOutput.ValidateAsync();
            if (!result.IsValid)
                throw new InvalidOperationException();

            await _kafkaBroker.Produce(stockProductOutput);
        }
    }
}