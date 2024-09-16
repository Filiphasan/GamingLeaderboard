namespace Api.Middleware;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IMiddleware
{
    private const string CorrelationIdHeader = "x-correlation-id";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = await GetCorrelationIdAsync(context);
        await LogRequestAsync(context, correlationId);

        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await next(context);

        await LogResponseAsync(context, correlationId);

        await responseBodyStream.CopyToAsync(originalBodyStream);
    }

    private static Task<string> GetCorrelationIdAsync(HttpContext context)
    {
        var hasCorrelationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId);
        if (hasCorrelationId)
        {
            return Task.FromResult<string>(correlationId);
        }

        correlationId = Guid.NewGuid().ToString();
        context.Request.Headers[CorrelationIdHeader] = correlationId;
        return Task.FromResult<string>(correlationId);
    }

    private async Task LogRequestAsync(HttpContext context, string correlationId)
    {
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        string requestBodyLog = requestBody;
        if (context.Request.ContentType != null && context.Request.ContentType.Contains("multipart/form-data"))
        {
            requestBodyLog = "Upload file Request";
        }

        var fullUrl =
            $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

        logger.LogInformation(
            "Request {CorrelationId} {Scheme} {Host} {Path} {FullPath} {QueryString} {@RequestBody}",
            correlationId,
            context.Request.Scheme,
            context.Request.Host,
            context.Request.Path,
            fullUrl,
            context.Request.QueryString,
            requestBodyLog
        );
    }

    private async Task LogResponseAsync(HttpContext context, string correlationId)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        logger.LogInformation(
            "Response {CorrelationId} {StatusCodee} {@ResponseBody}",
            correlationId,
            context.Response.StatusCode,
            responseBody
        );
    }
}