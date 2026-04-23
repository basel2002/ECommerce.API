using FluentValidation;

namespace BLL
{
    public class CategoryEditDtoValidator:AbstractValidator<CategoryEditDto>
    {
        public CategoryEditDtoValidator()
        {
            RuleFor(c => c.Name)
             .NotEmpty()
             .MinimumLength(5);

        }
    }
}
