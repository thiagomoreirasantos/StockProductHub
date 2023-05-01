using FluentValidation.Results;
using StockProduct.Application.Validator;

namespace StockProduct.Application.Dtos
{
    public class StockProductInput
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int Size { get; set; } 
        public string Department { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Color { get; set; } = string.Empty;
        public DateTime EstimatedDelivery { get; set; }
        public string Audience { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Season { get; set; } = string.Empty;
    }
}