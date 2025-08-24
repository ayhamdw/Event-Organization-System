using System.Diagnostics;
using System.Net;
using EventOrganizationSystem.Generic;

namespace EventOrganizationSystem.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next , ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception , "An unexpected error occurred");

        var response = exception switch
        {
            ApplicationException _ => GeneralApiResponse<object>.Failure(exception.Message),
            KeyNotFoundException _ => GeneralApiResponse<object>.Failure(exception.Message,
                (int)HttpStatusCode.NotFound),
            UnauthorizedAccessException _ => GeneralApiResponse<object>.Failure(exception.Message,
                (int)HttpStatusCode.Unauthorized),
            _ => GeneralApiResponse<object>.Failure(exception.Message, (int)HttpStatusCode.InternalServerError)
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}