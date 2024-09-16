using Api.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = Result<NoContentResult>.Error("An error occurred while processing the request.");
            logger.LogError(ex, "An error occurred while processing the request. RequestId: {RequestId}", response.RequestId);
            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}