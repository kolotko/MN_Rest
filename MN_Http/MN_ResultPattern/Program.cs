using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MN_ResultPattern;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Extensions["GlobalField"] = "na przykład trace id";
        context.ProblemDetails.Extensions["TraceId"] = context.HttpContext.TraceIdentifier;
    };
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature != null)
        {
            var exception = exceptionHandlerPathFeature.Error;

            var problemDetails = new ProblemDetails
            {
                Type = "https://example.com/internal-server-error",
                Title = "Wystąpił błąd wewnętrzny serwera",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    });
});

app.MapGet("/uservalidation", (int? path) =>
{
    UserDto? userDto = new UserDto
    {
        Profession = new Profession()
    };
    UserValidation validator = new UserValidation();
    switch (path)
    {
        case 1:
            var result = validator.Validate(userDto);
            var resultToProblemDetails = result!.ToProblemDetails();
            return Results.Problem(resultToProblemDetails);
        case 2:
            Dictionary<string, object?> extensions = new()
            {
                ["BadNumber"] = userDto.Age
            };
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"{userDto.Age} jest nieporpawnym wiekiem",
                extensions: extensions
                );
        case 3:
            var resultFromService = new ExampleService().ExampleMethod();
            if (!resultFromService.IsSuccess)
            {
                return Results.Problem(resultFromService.ProblemDetails);
            }
            return Results.Ok(resultFromService.Body);
    }
    throw new Exception("test");
});

app.Run();

// RFC 9457
//application/problem+json !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

// typ = dokumentacja
// title = tytuł błędu
// oba wyżej ustawiane automatycznie na podstawie status code



// detail = szczegóły błędu

// jak fluent validation na problem details przerobić
    // trance id globalnei

// global error handling


// może nawet w title lub description dawać kod błędu
// po kluczu można podpiąć zmienne z modelu