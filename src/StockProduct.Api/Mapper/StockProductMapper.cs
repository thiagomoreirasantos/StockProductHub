using StockProduct.Application.Dtos;

namespace StockProduct.Api.Mapper
{
    public static class StockProductMapper
    {
        public static StockProductOutput Map(this StockProductInput input, string topic)
        {
            return new StockProductOutput
            {
                Audience = input.Audience,
                Brand = input.Brand,
                Color = input.Color,
                Department = input.Department,
                EstimatedDelivery = input.EstimatedDelivery,
                Name = input.Name,
                Price = input.Price,
                Season = input.Season,
                Size = input.Size,
                Stock = input.Stock,
                Topic = topic,
            };
        }
    }
}