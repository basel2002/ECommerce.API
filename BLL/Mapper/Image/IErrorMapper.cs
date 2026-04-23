using Common;
using FluentValidation.Results;

namespace BLL
{
    public interface IErrorMapper 
    {
        Dictionary<string, List<Errors>> MapError(ValidationResult validationResult);
    }
}
