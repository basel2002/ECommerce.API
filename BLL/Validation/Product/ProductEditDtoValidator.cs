using FluentValidation;

namespace BLL
{
    public class ProductEditDtoValidator : AbstractValidator<ProductEditDto>
    {
        public ProductEditDtoValidator()
        {
            RuleFor(p => p.Name)
              .NotEmpty()
              .WithMessage("Name Can not be empty")
              .WithErrorCode("ERR-01")

              .MinimumLength(3)
              .WithMessage("Name Must be at least 3 character")
              .WithErrorCode("ERR-02");


            RuleFor(p => p.Description)
                .MinimumLength(5)
                .WithMessage("Description is at least of 5 character")
                .WithErrorCode("ERR-03");

            RuleFor(p => p.StockQty)
                .GreaterThan(0)
                .WithMessage("Stock Quantity Must Be Greater Than 0")
                .WithErrorCode("ERR-04");

            RuleFor(p => p.Price)
                .GreaterThan(0)
                .WithMessage("Price Must Be Greater Than 0")
                .WithErrorCode("ERR-05");

            //RuleFor(p => p.CategoryId)
            //    .GreaterThan(0)
            //    .WithMessage("Category ID Must Be Greater Than 0")
            //    .WithErrorCode("ERR-06");



        }
    }
}
