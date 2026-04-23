using Common;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ICategoryMapper
    {
        Dictionary<string, List<Errors>> MapError(ValidationResult validationResult);
    }
}
