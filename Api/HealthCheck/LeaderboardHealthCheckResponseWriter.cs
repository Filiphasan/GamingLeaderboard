using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.HealthCheck;

public class LeaderboardHealthCheckResponseWriter
{
    public static async Task WriteResponse(HttpContext httpContext, HealthReport report)
    {
        httpContext.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            results = report.Entries.Select(entry => new 
            {
                service = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                data = entry.Value.Data,
                error = entry.Value.Exception?.Message
            })
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}