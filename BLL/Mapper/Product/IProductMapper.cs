using Common;
using FluentValidation.Results;


namespace BLL
{
    public interface IProductMapper
    {
        Dictionary<string, List<Errors>> MapError(ValidationResult validationResult);

    }
}
