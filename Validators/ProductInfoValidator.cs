using FluentValidation;
using Minimal_APIValidators.Models;

namespace Minimal_APIValidators.Validators
{
    public class ProductInfoValidator : AbstractValidator<ProductInfo>
    {
        public ProductInfoValidator()
        {
            RuleFor(p => p.ProductId).GreaterThan(0);
            RuleFor(p=>p.ProductName).NotEmpty();
            RuleFor(p => p.CategoryName).NotEmpty();
            RuleFor(p => p.Manufacturer).NotEmpty();
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.Price).GreaterThan(0);
        }
    }
}
