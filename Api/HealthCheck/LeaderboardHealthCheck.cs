using Api.Data;
using Api.Service.Interface;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.HealthCheck;

public class LeaderboardHealthCheck(
    LeaderboardContext dbContext,
    IRedisCacheService redisCacheService
) : IHealthCheck
{
    public const string Name = "leaderboard_health_check";
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var healthCheckData = new Dictionary<string, object>();
        try
        {
            await redisCacheService.PingAsync();
            healthCheckData["Redis"] = true;
        }
        catch (Exception e)
        {
            healthCheckData["Redis"] = false;
            Console.WriteLine(e);
        }

        try
        {
            await dbContext.Database.CanConnectAsync(cancellationToken);
            healthCheckData["Database"] = true;
        }
        catch (Exception e)
        {
            healthCheckData["Database"] = false;
            Console.WriteLine(e);
        }

        return healthCheckData.All(x => x.Value is true)
            ? HealthCheckResult.Healthy("Leaderboard is healthy.", healthCheckData)
            : HealthCheckResult.Unhealthy("Leaderboard is unhealthy.", data: healthCheckData);
    }
}