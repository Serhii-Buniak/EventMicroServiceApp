using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace IdentityMicroService.BLL.Exceptions;

public class ValidationModelException : Exception
{
    public ValidationModelException()
    : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationModelException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }    
    
    public ValidationModelException(IEnumerable<IdentityError> errors)
        : this()
    {
        Errors = errors
            .GroupBy(e => e.Code, e => e.Description)
            .ToDictionary(errorGroup => errorGroup.Key, errorGroup => errorGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
