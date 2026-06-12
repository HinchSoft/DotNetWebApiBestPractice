using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace AspPresentation.Exceptions;

internal sealed class ValidationExceptionHandler(IProblemDetailsService detailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if(exception is not ValidationException resultException)
            return false;

        var ext = resultException.Errors.ToDictionary<ValidationFailure?, string, object>(item => "Problem", item => item);

        httpContext.Response.StatusCode = resultException.GetStatus();

        return await detailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Type = resultException.GetStatusCode(),
                Title = $"A problem has occured",
                Detail = exception.Message,
                Extensions = ext
            }
        });
    }
}
