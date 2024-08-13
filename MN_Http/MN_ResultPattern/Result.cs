using Microsoft.AspNetCore.Mvc;

namespace MN_ResultPattern;

public class Result<T>
{
    public T Body { get; set; }
    public bool IsSuccess => ProblemDetails is null;
    public ProblemDetails? ProblemDetails { get; set; }
}