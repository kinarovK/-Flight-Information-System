using FlightStorageService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace FlightStorageService;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
internal class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<CustomExceptionFilterAttribute> _logger;

    public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Exception occurred during request execution.");
        if (context.Exception is NotFoundException)
        {
            context.Result = new NotFoundObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        }
        else if (context.Exception is InvalidRequestException)
        {
            context.Result = new BadRequestObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        }
        else if (context.Exception is SqlException sqlException)
        {
            if (sqlException.Number == 2627)
            {
                context.Result = new ObjectResult("Invalid request, flight number already exists. Try Again")
                {
                    StatusCode = 400,
                };
                context.ExceptionHandled = true;
            }
            else if (sqlException.Number == 50000)
            {
                context.Result = new ObjectResult("The flight can be scheduled only for next 7 days")
                {
                    StatusCode = 400,
                };
                context.ExceptionHandled = true;
            }
        }
        else if (context.Exception is OperationCanceledException)
        {
            context.Result = new ObjectResult("Cancelled")
            {
                StatusCode = 499,
            };
            context.ExceptionHandled = true;
        }
        else
        {
            context.Result = new ObjectResult("An unexpected error has occurred. Try Again")
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = true;
        }

        base.OnException(context);
    }
}
