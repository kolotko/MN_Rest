using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace MN_ResultPattern;

public static class ValidationResultExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        var problemDetails = new ValidationProblemDetails();

        foreach (var error in validationResult.Errors)
        {
            problemDetails.Errors.Add(error.PropertyName, new[]
            {
                error.ErrorMessage, 
                // error.ErrorCode
            });
        }

        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "One or more validation errors occurred.";
        problemDetails.Detail = "See the errors property for more details.";

        return problemDetails;
    }
}