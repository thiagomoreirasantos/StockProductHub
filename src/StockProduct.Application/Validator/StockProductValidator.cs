using FluentValidation;
using StockProduct.Application.Dtos;

namespace StockProduct.Application.Validator
{
    public class StockProductValidator : AbstractValidator<StockProductOutput>
    {
        public StockProductValidator()
        {
            RuleFor(x => x.Audience).NotNull().NotEmpty();
            RuleFor(x => x.Brand).NotNull().NotEmpty();
            RuleFor(x => x.Color).NotNull().NotEmpty().Length(10);
            RuleFor(x => x.Department).NotNull().NotEmpty().Length(10);
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0).IsInEnum();
            RuleFor(x => x.EstimatedDelivery).NotNull().NotEmpty().GreaterThan(x => DateTime.Now);
            RuleFor(x => x.Season).NotNull().NotEmpty().Length(15);
            RuleFor(x => x.Size).NotEmpty().IsInEnum();
            RuleFor(x => x.Stock).NotNull().IsInEnum();
        }
    }
}