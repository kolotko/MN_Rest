using Microsoft.AspNetCore.Mvc;

namespace MN_ResultPattern;

public class ExampleService : IExampleService
{
    public Result<UserDto> ExampleMethod()
    {
        var response = new Result<UserDto>();
        response.Body = new UserDto();
        response.ProblemDetails = CustomMaperFromSpecialClass();
        return response;
    }

    private ProblemDetails CustomMaperFromSpecialClass()
    {
        var problemDetails = new ProblemDetails();
        //ustaw error, np w try catch może być 500
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "Brak uprawnień dla użytkownika";
        problemDetails.Detail = "Brak pozwolenia na wykonanie akcji";
        problemDetails.Extensions = new Dictionary<string, object?>();
        problemDetails.Extensions.Add("Access", new[]
        {
            "denied", 
        });

        return problemDetails;
    }
}