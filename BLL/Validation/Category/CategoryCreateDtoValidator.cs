using FluentValidation;

namespace BLL
{
    public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(5);


        }
    }
}
